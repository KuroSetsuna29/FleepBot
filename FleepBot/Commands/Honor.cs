using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class Honor : BaseCommand
	{
		public override string command_name { get { return "Honor"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}honou?r(?:\\s+(.*))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string options = regex.Match(message).Groups[1].Value;
			bool reroll = false;

			if (options.ToLower() == "reroll" || options.ToLower() == "r")
				reroll = true;

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
					if (conversation.conversation_id == FleepBot.Program.SOULCHAT)
					{
						dynamic membersinfo = FleepBot.Program.ApiPost("api/contact/sync/list", new { contacts = conversation.members, ticket = FleepBot.Program.TICKET });
						List<string> members = new List<string>();
						string name = "Your";
						foreach (dynamic contact in membersinfo.contacts)
						{
							if (contact.account_id.Value == account_id)
							{
								name = contact.display_name.Value;
							} else if (contact.account_id.Value != account_id
								&& contact.account_id.Value != FleepBot.Program.ACCOUNT_ID
								&& contact.account_id.Value != FleepBot.Program.JAMES
								&& contact.account_id.Value != FleepBot.Program.JENNY
								&& contact.account_id.Value != FleepBot.Program.GTJ
								&& contact.account_id.Value != FleepBot.Program.ANDERAN
								&& contact.account_id.Value != FleepBot.Program.JOEYBANANAS
								&& contact.account_id.Value != FleepBot.Program.SPOONY
								/*&& contact.account_id.Value != FleepBot.Program.JACK*/)
							{
								members.Add(contact.display_name.Value);
							}
						}

						int dst = DateTime.Now.IsDaylightSavingTime() ? -11 : -10;
						int seed = (int)(DateTime.Now.AddHours(dst) - new DateTime(1970, 1, 1)).Days + account_id.Sum(c => System.Convert.ToInt32(c));
						Random rand = new Random(seed);
						int i = rand.Next(0, members.Count - 1);

						if (reroll)
						{
							members.RemoveAt(i);
							i = rand.Next(0, members.Count - 1);
						}

						if (options.ToLower() == "+reroll" || options.ToLower() == "+r")
						{
							string member1 = members[i];
							members.RemoveAt(i);
							i = rand.Next(0, members.Count - 1);
							string member2 = members[i];

							FleepBot.Program.SendMessage(convid, String.Format("{0} honor of the day is *{1}* and *{2}*", name ?? "Your", member1, member2));
						} else {
							FleepBot.Program.SendMessage(convid, String.Format("{0} honor of the day is *{1}*", name ?? "Your", members[i]));
						}

						found = true;
						break;
					}
				}

			} while (!found && !string.IsNullOrWhiteSpace((string)conversations));
		}
	}
}
