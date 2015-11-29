using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class WhoIsOn
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}whoison(?:\\s+(.+))?</p></msg>$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static async Task execute(string convid, string message)
		{
			double hours = 1;
			string input = regex.Match(message).Groups[1].Value;
			if (!String.IsNullOrWhiteSpace(input))
			{
				if (!double.TryParse(input, out hours))
				{
					await FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify time in hours. Example: {0}whoison _Hours_", FleepBot.Program.COMMAND_PREFIX));
					return;
				}
			}

			int sync_horizon = 0;
			bool found = false;
			dynamic conversations = new { };
			do
			{
				dynamic list = await FleepBot.Program.ApiPost("api/conversation/list", new { sync_horizon = sync_horizon, ticket = FleepBot.Program.TICKET });
				conversations = list.conversations;
				sync_horizon = list.sync_horizon;

				foreach (dynamic conversation in conversations)
				{
					if (conversation.conversation_id == convid)
					{
						dynamic membersinfo = await FleepBot.Program.ApiPost("api/contact/sync/list", new { contacts = conversation.members, ticket = FleepBot.Program.TICKET });
						List<dynamic> members = new List<dynamic>();
						foreach (dynamic contact in membersinfo.contacts)
						{
							if (contact.account_id.Value != FleepBot.Program.ACCOUNT_ID && contact.activity_time != null)
							{
								DateTime lastactivity = Utils.parseUnixTimestamp(contact.activity_time.Value);
								if (lastactivity >= DateTime.Now.AddHours(-hours))
								{
									int relativeminutes = (int)(DateTime.Now - lastactivity).TotalMinutes;
									string relativetime = "";
									if (relativeminutes >= 60)
									{
										relativetime = String.Format("more than {0} hr{1} ago", relativeminutes / 60, relativeminutes >= 120 ? "s" : "");
									}
									else
									{
										relativetime = String.Format("{0} min{1} ago", relativeminutes, relativeminutes > 1 ? "s" : "");
									}

									members.Add(new { DisplayName = contact.display_name, LastActivity = lastactivity, msg = String.Format("{0} {1}", contact.display_name, relativetime) });
								}
							}
						}

						members.Sort((x, y) => DateTime.Compare(x.LastActivity, y.LastActivity));
						string msg = ":::\nLast seen on Fleep...\n" + String.Join("\n", members.Select(x => x.msg));
						await FleepBot.Program.SendMessage(convid, msg);

						found = true;
						break;
					}
				}

			} while (!found && !string.IsNullOrWhiteSpace((string)conversations));
		}
	}
}
