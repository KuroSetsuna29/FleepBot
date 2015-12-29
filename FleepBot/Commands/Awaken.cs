using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class Awaken : BaseCommand
	{
		public override string command_name { get { return "Awaken"; } }
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}awaken(?:\\s+(.+))?</p></msg>$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;
			string hero = "";

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a hero. Example: {0}heroinfo _Hero_", FleepBot.Program.COMMAND_PREFIX));
				return;
			}
			
			string query = String.Format("select B where lower(B) matches '.*({0}).*'", search.ToLower().Trim());
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", query, 0, "B16:B50");

			if (stats == null)
			{
				return;
			}

			string msg = String.Format("No awaken info found for '{0}'.", search);

			if (stats.Item2.Count > 0)
			{
				hero = stats.Item2.First().c[0].v.Value;
				query = String.Format("select B, C, D, E, F, G, H, I, J", search.ToLower().Trim());
				Tuple<List<dynamic>, List<dynamic>> mats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", query, 0, "B3:J3");
				msg = String.Format(":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}",
					hero,
					mats.Item2.First().c[0].v.Value,
					mats.Item2.First().c[1].v.Value,
					mats.Item2.First().c[2].v.Value,
					mats.Item2.First().c[3].v.Value,
					mats.Item2.First().c[4].v.Value,
					mats.Item2.First().c[5].v.Value,
					mats.Item2.First().c[6].v.Value,
					mats.Item2.First().c[7].v.Value,
					mats.Item2.First().c[8].v.Value);
			}
			else
			{
				query = String.Format("select C where lower(C) matches '.*({0}).*'", search.ToLower().Trim());
				stats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", query, 0, "C16:C50");

				if (stats == null)
				{
					return;
				}

				if (stats.Item2.Count > 0)
				{
					hero = stats.Item2.First().c[0].v.Value;
					query = String.Format("select B, C, D, E, F, G, H, I, J", search.ToLower().Trim());
					Tuple<List<dynamic>, List<dynamic>> mats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", query, 0, "B4:J4");
					msg = String.Format(":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}",
						hero,
						mats.Item2.First().c[0].v.Value,
						mats.Item2.First().c[1].v.Value,
						mats.Item2.First().c[2].v.Value,
						mats.Item2.First().c[3].v.Value,
						mats.Item2.First().c[4].v.Value,
						mats.Item2.First().c[5].v.Value,
						mats.Item2.First().c[6].v.Value,
						mats.Item2.First().c[7].v.Value,
						mats.Item2.First().c[8].v.Value);
				}
				else
				{
					query = String.Format("select D where lower(D) matches '.*({0}).*'", search.ToLower().Trim());
					stats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", query, 0, "D16:D50");

					if (stats == null)
					{
						return;
					}

					if (stats.Item2.Count > 0)
					{
						hero = stats.Item2.First().c[0].v.Value;
						query = String.Format("select B, C, D, E, F, G, H, I, J", search.ToLower().Trim());
						Tuple<List<dynamic>, List<dynamic>> mats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", query, 0, "B5:J5");
						msg = String.Format(":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}",
							hero,
							mats.Item2.First().c[0].v.Value,
							mats.Item2.First().c[1].v.Value,
							mats.Item2.First().c[2].v.Value,
							mats.Item2.First().c[3].v.Value,
							mats.Item2.First().c[4].v.Value,
							mats.Item2.First().c[5].v.Value,
							mats.Item2.First().c[6].v.Value,
							mats.Item2.First().c[7].v.Value,
							mats.Item2.First().c[8].v.Value);
					}
				}
			}

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
