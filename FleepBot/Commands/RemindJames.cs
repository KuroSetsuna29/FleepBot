using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace FleepBot.Commands
{
    class RemindJames : BaseCommand
	{
		public override string command_name { get { return "RemindJames"; } }
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}remindjames(?:\\s+((?:(\\d+)d)?(?:(\\d+)h)?(?:(\\d+)m)?(?:(\\d+)s)?))?(?:\\s+(.+))?</p></msg>$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase);

		protected override void execute(string convid, string message, string account_id)
		{
			if (!FleepBot.Program.ADMIN_CHATS.Contains(convid.ToLower()))
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}

			string delay = regex.Match(message).Groups[1].Value;
			string days = regex.Match(message).Groups[2].Value ?? "0";
			string hours = regex.Match(message).Groups[3].Value ?? "0";
			string minutes = regex.Match(message).Groups[4].Value ?? "0";
			string seconds = regex.Match(message).Groups[5].Value ?? "0";
			string msg = regex.Match(message).Groups[6].Value;

			days = String.IsNullOrEmpty(days) ? "0" : days;
			hours = String.IsNullOrEmpty(hours) ? "0" : hours;
			minutes = String.IsNullOrEmpty(minutes) ? "0" : minutes;
			seconds = String.IsNullOrEmpty(seconds) ? "0" : seconds;

			if (String.IsNullOrWhiteSpace(delay) || String.IsNullOrWhiteSpace(msg))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a valid delay. Example: {0}remind 1h30m This message will show after 1 hour 30 minutes", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			TimeSpan timespan = new TimeSpan(int.Parse(days), int.Parse(hours), int.Parse(minutes), int.Parse(seconds));
			string contact_name = FleepBot.Program.GetUserName(account_id);

			Remind.Reminder r = new Remind.Reminder(contact_name, timespan, convid, msg, FleepBot.Program.HANGOUTS_JAMES);

			FleepBot.Program.SendMessage(convid, String.Format("Set reminder for {0} with hangouts", timespan.ToString("dd\\.hh\\:mm\\:ss")));
        }
	}
}
