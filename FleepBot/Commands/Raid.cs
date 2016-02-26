using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace FleepBot.Commands
{
	public class RaidRoom
	{
		public enum RoleEnum
		{
			Tank = 0,
			Heal = 1,
			Attack = 2,
			Any = 8,
			Unknown = 9
		}

		public string Creator = "";
		public string RoomID = "";
		public string Password = "";
		public int[] RolesOpen = new int[3] { 1, 1, 2 };
		private Timer selfdestruct = new Timer();

		public RaidRoom(string convid, string user, string room, string pass, string role)
		{
			this.Creator = user;
			this.RoomID = room;
			this.Password = pass;

			string result = AddRole(role);
			if (!String.IsNullOrEmpty(result))
			{
				FleepBot.Program.SendErrorMessage(convid, result);
				return;
			}

			FleepBot.Program.RAIDS.Add(this);

			selfdestruct = new Timer(7200000); // auto expire after 2 hours
			selfdestruct.Elapsed += new ElapsedEventHandler(_destroy);
			selfdestruct.Enabled = true;
			selfdestruct.AutoReset = false;
		}

		public bool Need(RoleEnum role)
		{
			if (role == RoleEnum.Any)
			{
				return RolesOpen[(int)RoleEnum.Tank] > 0 || RolesOpen[(int)RoleEnum.Heal] > 0 || RolesOpen[(int)RoleEnum.Attack] > 0;
            }

			return RolesOpen[(int)role] > 0;
		}

		public bool Need(RoleEnum role, out RoleEnum asRole)
		{
			if (role == RoleEnum.Any)
			{
				int i = new Random().Next(0, 3);
				if (RolesOpen[i] > 0)
				{
					asRole = (RoleEnum)i;
					return true;
				}
				i = (i + 1) % 3;
				if (RolesOpen[i] > 0)
				{
					asRole = (RoleEnum)i;
					return true;
				}
				i = (i + 1) % 3;
				if (RolesOpen[i] > 0)
				{
					asRole = (RoleEnum)i;
					return true;
				}

				asRole = RoleEnum.Any;
				return false;
			}

			asRole = role;
			return RolesOpen[(int)role] > 0;
		}

		public void FillUp()
		{
			RolesOpen[(int)RoleEnum.Tank] = 0;
			RolesOpen[(int)RoleEnum.Heal] = 0;
			RolesOpen[(int)RoleEnum.Attack] = 0;
		}

		public void Remove()
		{
			selfdestruct.Enabled = false;
			FleepBot.Program.RAIDS.Remove(this);
		}

		public static RoleEnum ParseRole(string role)
		{
			switch (role.ToLower())
			{
				case "tanker":
				case "tank":
				case "t":
					return RoleEnum.Tank;
				case "healer":
				case "heal":
				case "h":
					return RoleEnum.Heal;
				case "attacker":
				case "attack":
				case "atk":
				case "adc":
				case "a":
					return RoleEnum.Attack;
				case "any":
				case "all":
					return RoleEnum.Any;
				default:
					return RoleEnum.Unknown;
			}
		}

		public string AddRole(string role)
		{
			RoleEnum r = ParseRole(role);

			if (r == RoleEnum.Unknown)
			{
				return String.Format("Unknown role '{0}'", role);
			}
			else if (r == RoleEnum.Tank && RolesOpen[(int)r] > 0)
			{
				RolesOpen[(int)r]--;
			}
			else if (r == RoleEnum.Heal && RolesOpen[(int)r] > 0)
			{
				RolesOpen[(int)r]--;
			}
			else if (r == RoleEnum.Attack && RolesOpen[(int)r] > 0)
			{
				RolesOpen[(int)r]--;
			}
			else
			{
				return String.Format("{0} position is already full.", r.ToString());
			}

			return null;
		}

		public string RemoveRole(string role)
		{
			RoleEnum r = ParseRole(role);

			if (r == RoleEnum.Unknown)
			{
				return String.Format("Unknown role '{0}'", role);
			}
			else if (r == RoleEnum.Tank && RolesOpen[(int)r] < 1)
			{
				RolesOpen[(int)r]++;
			}
			else if (r == RoleEnum.Heal && RolesOpen[(int)r] < 1)
			{
				RolesOpen[(int)r]++;
			}
			else if (r == RoleEnum.Attack && RolesOpen[(int)r] < 2)
			{
				RolesOpen[(int)r]++;
			}
			else
			{
				return String.Format("{0} position is already empty.", r.ToString());
			}

			return null;
		}

		private void _destroy(object source, ElapsedEventArgs e)
		{
			Remove();
		}
	}

	class RaidCreate : BaseCommand
	{
		public override string command_name { get { return "RaidCreate"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}raidcreate(?:\\s+(\\d+)\\s+(\\d+)\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string room = regex.Match(message).Groups[1].Value;
			string pass = regex.Match(message).Groups[2].Value;
			string role = regex.Match(message).Groups[3].Value;

			if (String.IsNullOrWhiteSpace(room) || String.IsNullOrWhiteSpace(pass) || String.IsNullOrWhiteSpace(role))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please enter correct format _room_ _pass_ _role_. Example: {0}raidcreate 9303 1234 tank", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			string contact_name = FleepBot.Program.GetUserName(account_id);
			RaidRoom.RoleEnum player_role = RaidRoom.ParseRole(role);

			if (player_role == RaidRoom.RoleEnum.Any)
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("{0}, cannot create raid as the Any role, please specify your role.", contact_name));
				return;
			}

			RaidRoom r = new RaidRoom(convid, contact_name, room, pass, role);

			FleepBot.Program.SendMessage(convid, String.Format("{0} has started a raid.", contact_name));
        }
	}

	class RaidJoin : BaseCommand
	{
		public override string command_name { get { return "RaidJoin"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}raidjoin(?:\\s+(?:(\\d+)\\s+)?(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string room = regex.Match(message).Groups[1].Value;
			string role = regex.Match(message).Groups[2].Value;

			if ( String.IsNullOrWhiteSpace(role))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please enter correct format [ _room_ ] _role_. Example: {0}raidjoin tank", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			string contact_name = FleepBot.Program.GetUserName(account_id);
			RaidRoom.RoleEnum player_role = RaidRoom.ParseRole(role);

			if (player_role == RaidRoom.RoleEnum.Unknown)
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("{0}, unknown role '{1}'", contact_name, role));
				return;
			}

			RaidRoom rr = null;
			if (!String.IsNullOrEmpty(room))
			{
				rr = FleepBot.Program.RAIDS.FirstOrDefault(r => r.RoomID.ToLower() == room.ToLower());
			}
			else
			{
				rr = FleepBot.Program.RAIDS.FirstOrDefault(r => r.Need(player_role));
			}

            if (rr == null)
			{
				FleepBot.Program.SendMessage(convid, String.Format("{0}, no raid found. Please create a new raid.", contact_name));
				return;
			}

			RaidRoom.RoleEnum newRole = RaidRoom.RoleEnum.Unknown;
			rr.Need(player_role, out newRole);
			string result = rr.AddRole(newRole.ToString());
			if (!String.IsNullOrEmpty(result))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("{0}, {1}", contact_name, result));
				return;
			}
			
			FleepBot.Program.SendMessage(convid, String.Format("{0} please join {1} at {2}/{3} as a {4}.", contact_name, rr.Creator, rr.RoomID, rr.Password, newRole.ToString()));
		}
	}

	class RaidKick : BaseCommand
	{
		public override string command_name { get { return "RaidKick"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}raidkick(?:\\s+(\\d+)\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string room = regex.Match(message).Groups[1].Value;
			string role = regex.Match(message).Groups[2].Value;

			if (String.IsNullOrWhiteSpace(role))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please enter correct format _room_ _role_. Example: {0}raidkick tank", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			string contact_name = FleepBot.Program.GetUserName(account_id);
			RaidRoom.RoleEnum player_role = RaidRoom.ParseRole(role);

			if (player_role == RaidRoom.RoleEnum.Unknown)
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("{0}, unknown role '{1}'", contact_name, role));
				return;
			}
			else if (player_role == RaidRoom.RoleEnum.Any)
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("{0}, cannot kick Any role, please specify the role.", contact_name));
				return;
			}

			RaidRoom rr = FleepBot.Program.RAIDS.FirstOrDefault(r => r.RoomID.ToLower() == room.ToLower());
			if (rr == null)
			{
				FleepBot.Program.SendMessage(convid, String.Format("{0}, raid '{1}' not found.", contact_name, room));
				return;
			}
			
			string result = rr.RemoveRole(role);
			if (!String.IsNullOrEmpty(result))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("{0}, {1}", contact_name, result));
				return;
			}

			FleepBot.Program.SendMessage(convid, String.Format("{0} has been removed from {1}.", player_role.ToString(), room));
		}
	}

	class RaidFull : BaseCommand
	{
		public override string command_name { get { return "RaidFull"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}raidfull(?:\\s+(\\d+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string room = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(room))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please enter correct format _room_. Example: {0}raidfull 9243", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			string contact_name = FleepBot.Program.GetUserName(account_id);

			RaidRoom rr = FleepBot.Program.RAIDS.FirstOrDefault(r => r.RoomID.ToLower() == room.ToLower());
			if (rr == null)
			{
				FleepBot.Program.SendMessage(convid, String.Format("{0}, raid '{1}' not found.", contact_name, room));
				return;
			}

			rr.FillUp();

			FleepBot.Program.SendMessage(convid, String.Format("{0} is now full.", room));
		}
	}

	class RaidPromote : BaseCommand
	{
		public override string command_name { get { return "RaidFull"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}raidpromote(?:\\s+(\\d+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string room = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(room))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please enter correct format _room_. Example: {0}raidpromote 9243", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			string contact_name = FleepBot.Program.GetUserName(account_id);

			RaidRoom rr = FleepBot.Program.RAIDS.FirstOrDefault(r => r.RoomID.ToLower() == room.ToLower());
			if (rr == null)
			{
				FleepBot.Program.SendMessage(convid, String.Format("Raid '{0}' not found.", room));
				return;
			}

			string need = "";
			for ( int i = 0; i < rr.RolesOpen[(int)RaidRoom.RoleEnum.Tank]; i++)
			{
				need += "/t";
			}
			for (int i = 0; i < rr.RolesOpen[(int)RaidRoom.RoleEnum.Heal]; i++)
			{
				need += "/h";
			}
			for (int i = 0; i < rr.RolesOpen[(int)RaidRoom.RoleEnum.Attack]; i++)
			{
				need += "/a";
			}
			need = need.Remove(0, 1);

			if (String.IsNullOrEmpty(need))
			{
				FleepBot.Program.SendMessage(convid, String.Format("{0}/{1} is full.", rr.RoomID, rr.Password));
				return;
			}

			FleepBot.Program.SendMessage(convid, String.Format("{0} in {1}/{2} is looking for {3}.", rr.Creator, rr.RoomID, rr.Password, need));
		}
	}
}
