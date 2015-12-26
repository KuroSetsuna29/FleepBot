using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace FleepBot.Commands
{
    class Ban
	{
		public class BanTimer
		{
			public string name = "";
			public string member = "";
			public TimeSpan Delay = new TimeSpan();
			private Timer timer = new Timer();
			private DateTime _ends = new DateTime();

			public BanTimer(string contact_name, string account_id, TimeSpan delay)
			{
				this.name = contact_name;
				this.member = account_id;
				this.Delay = delay;

				timer = new Timer(delay.TotalMilliseconds);
				timer.Elapsed += delegate { unban(member); };
				timer.Enabled = true;
				timer.AutoReset = false;
				_ends = DateTime.Now.Add(delay);

				FleepBot.Program.BANLIST.Add(this);
			}

			public TimeSpan TimeLeft
			{
				get { return _ends - DateTime.Now; }
			}

			public string TimeLeftAsString
			{
				get { return TimeLeft.ToString("dd\\.hh\\:mm\\:ss"); }
			}

			private static void unban(string member)
			{
				FleepBot.Program.BANLIST.RemoveAll(x => x.member.ToLower() == member.ToLower());
			}
		}

		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}ban(?:\\s+((?:(\\d+)d)?(?:(\\d+)h)?(?:(\\d+)m)?(?:(\\d+)s)?))?(?:\\s+(.+))?</p></msg>$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static async Task execute(string convid, string message)
		{
			string delay = regex.Match(message).Groups[1].Value;
			string days = regex.Match(message).Groups[2].Value;
			string hours = regex.Match(message).Groups[3].Value;
			string minutes = regex.Match(message).Groups[4].Value;
			string seconds = regex.Match(message).Groups[5].Value;
			string member = regex.Match(message).Groups[6].Value;

			days = String.IsNullOrEmpty(days) ? "0" : days;
			hours = String.IsNullOrEmpty(hours) ? "0" : hours;
			minutes = String.IsNullOrEmpty(minutes) ? "0" : minutes;
			seconds = String.IsNullOrEmpty(seconds) ? "0" : seconds;

			if (convid != FleepBot.Program.TESTCHAT)
			{
				await FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}

			TimeSpan timespan = new TimeSpan(int.Parse(days), int.Parse(hours), int.Parse(minutes), int.Parse(seconds));
			dynamic memberinfo = await FleepBot.Program.ApiPost("api/contact/sync", new { contact_id = member, ticket = FleepBot.Program.TICKET });
			string contact_name = memberinfo.contact_name;

			if (String.IsNullOrEmpty(contact_name))
			{
				await FleepBot.Program.SendErrorMessage(convid, "Error: Could not find user.");
				return;
			}

			BanTimer ban = new BanTimer(contact_name, member, timespan);

			await FleepBot.Program.SendMessage(convid, String.Format("Successfully banned {0} for {1}.", contact_name, timespan.ToString("dd\\.hh\\:mm\\:ss")));
		}
	}
}
