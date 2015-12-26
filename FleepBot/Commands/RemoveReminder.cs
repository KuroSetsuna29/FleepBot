using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace FleepBot.Commands
{
    class RemoveReminder
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}removereminder(?:\\s+(.+))?</p></msg>$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static void execute(string convid, string message)
		{
			string id = regex.Match(message).Groups[1].Value;

			if (convid != FleepBot.Program.TESTCHAT)
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}
			
			FleepBot.Program.REMINDERS.RemoveAll(x => x.ID.ToLower() == id.ToLower());

			FleepBot.Program.SendMessage(convid, String.Format("Successfully removed {0}.", id));
		}
	}
}
