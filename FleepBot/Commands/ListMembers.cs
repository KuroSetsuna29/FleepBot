using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class ListMembers
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}listmembers(?:\\s+(.+))?</p></msg>$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static void execute(string convid, string message)
		{
			if (convid != FleepBot.Program.TESTCHAT)
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}

			string filter = regex.Match(message).Groups[1].Value;
			string filter_conv = "";
			string filter_member = "";

			if (String.IsNullOrEmpty(filter))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify conversation name.", FleepBot.Program.COMMAND_PREFIX));
				return;
			}
			else
			{
				string[] filters = filter.Split(new char[] { ' ' }, 2);

				if (filters.Count() > 0)
					filter_conv = filter.Split(new char[] { ' ' }, 2)[0];
				if (filters.Count() > 1)
					filter_member = filter.Split(new char[] { ' ' }, 2)[1];
			}

			int sync_horizon = 0;
			bool found = false;
			dynamic conversations = new { };
			do
			{
				dynamic list = FleepBot.Program.ApiPost("api/conversation/list", new { sync_horizon = sync_horizon, ticket = FleepBot.Program.TICKET });
				conversations = list.conversations;
				sync_horizon = list.sync_horizon;

				foreach (dynamic conversation in conversations)
				{
					if (conversation.conversation_id != null && ((string)conversation.topic).ToLower().Contains((filter_conv ?? "").ToLower()))
					{
						dynamic membersinfo = FleepBot.Program.ApiPost("api/contact/sync/list", new { contacts = conversation.members, ticket = FleepBot.Program.TICKET });

						if (!String.IsNullOrEmpty(filter_member))
						{
							foreach (dynamic contact in membersinfo.contacts)
							{
								if (((string)contact.display_name.Value).ToLower().Contains(filter_member.ToLower()))
									FleepBot.Program.SendMessage(convid, String.Format(":::\n{0}", contact));
							}
						}
						else
						{
							FleepBot.Program.SendMessage(convid, String.Format(":::\n{0}", membersinfo));
						}

						found = true;
						break;
					}
				}

			} while (!found && !string.IsNullOrWhiteSpace((string)conversations));

			if (!found)
				FleepBot.Program.SendErrorMessage(convid, "Error: could not find conversation.");
		}
	}
}
