using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace FleepBot.Commands
{
    class Unban : BaseCommand
	{
		public override string command_name { get { return "Unban"; } }
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}unban(?:\\s+(.+))?</p></msg>$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase);

		protected override void execute(string convid, string message, string account_id)
		{
			string member = regex.Match(message).Groups[1].Value;

			if (convid != FleepBot.Program.TESTCHAT)
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}
			
			dynamic memberinfo = FleepBot.Program.ApiPost("api/contact/sync", new { contact_id = member, ticket = FleepBot.Program.TICKET });
			string contact_name = memberinfo.contact_name;

			if (String.IsNullOrEmpty(contact_name))
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Could not find user.");
				return;
			}

			FleepBot.Program.BANLIST.RemoveAll(x => x.member.ToLower() == member.ToLower());

			FleepBot.Program.SendMessage(convid, String.Format("Successfully unbanned {0}.", contact_name));
		}
	}
}
