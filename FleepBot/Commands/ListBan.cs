using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class ListBan
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}listban(?:\\s+(.+))?</p></msg>$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static void execute(string convid, string message)
		{
			if (convid != FleepBot.Program.TESTCHAT)
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}

			int nameLen = Math.Max(FleepBot.Program.BANLIST.Select(x => x.name.Length).DefaultIfEmpty().Max() + 2, 6);
			int memberLen = Math.Max(FleepBot.Program.BANLIST.Select(x => x.member.Length).DefaultIfEmpty().Max() + 2, 10);
			int timeleftLen = Math.Max(FleepBot.Program.BANLIST.Select(x => x.TimeLeftAsString.Length).DefaultIfEmpty().Max() + 2, 10);

			string msg = String.Format(":::\n{0}{1}{2}\n",
						"Name".PadRight(nameLen),
						"MemberID".PadRight(memberLen),
						"TimeLeft".PadRight(timeleftLen))
						+ String.Join("\n", FleepBot.Program.BANLIST.Select(x => String.Format("{0}{1}{2}",
						(x.name).PadRight(nameLen),
						(x.member).PadRight(memberLen),
						(x.TimeLeftAsString).PadRight(timeleftLen))));

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
