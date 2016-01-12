using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace FleepBot.Commands
{
    class RemoveReminder : BaseCommand
	{
		public override string command_name { get { return "RemoveReminder"; } }
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}removereminder(?:\\s+(.+))?</p></msg>$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase);

		protected override void execute(string convid, string message, string account_id)
		{
			string id = regex.Match(message).Groups[1].Value;

			if (!FleepBot.Program.ADMIN_CHATS.Contains(convid.ToLower()))
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}
			
			IEnumerable<Remind.Reminder> reminders = FleepBot.Program.REMINDERS.Where(x => x.ID.ToLower() == id.ToLower());
			foreach (Remind.Reminder reminder in reminders)
			{
				reminder.Remove();
			}

			FleepBot.Program.SendMessage(convid, String.Format("Successfully removed {0}.", id));
		}
	}
}
