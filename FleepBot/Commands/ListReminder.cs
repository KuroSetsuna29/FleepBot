using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class ListReminder
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}listreminder(?:\\s+(.+))?</p></msg>$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static void execute(string convid, string message)
		{
			if (convid != FleepBot.Program.TESTCHAT)
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}

			int idLen = Math.Max(FleepBot.Program.REMINDERS.Select(x => x.ID.Length).DefaultIfEmpty().Max() + 2, 4);
			int userLen = Math.Max(FleepBot.Program.REMINDERS.Select(x => x.User.Length).DefaultIfEmpty().Max() + 2, 6);
			int timeleftLen = Math.Max(FleepBot.Program.REMINDERS.Select(x => x.TimeLeftAsString.Length).DefaultIfEmpty().Max() + 2, 10);
			int messageLen = Math.Max(FleepBot.Program.REMINDERS.Select(x => x.Message.Length).DefaultIfEmpty().Max() + 2, 9);

			string msg = String.Format(":::\n{0}{1}{2}{3}\n",
						"ID".PadRight(idLen),
						"User".PadRight(userLen),
						"TimeLeft".PadRight(timeleftLen),
						"Message".PadRight(messageLen))
						+ String.Join("\n", FleepBot.Program.REMINDERS.Select(x => String.Format("{0}{1}{2}{3}",
						(x.ID).PadRight(idLen),
						(x.User).PadRight(userLen),
						(x.TimeLeftAsString).PadRight(timeleftLen),
						(x.Message).PadRight(messageLen))));

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
