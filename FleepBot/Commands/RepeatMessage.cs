using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace FleepBot.Commands
{
    class RepeatMessage
	{
		private string convid = "";
		private string message = "";
		private Timer timer = new Timer();
		private Timer starter = new Timer();

		public RepeatMessage(string convid, string message, double minutes)
		{
			this.convid = convid;
			this.message = message;

			timer = new Timer(minutes * 60 * 1000);
			timer.Elapsed += new ElapsedEventHandler(execute);
			timer.Enabled = true;
		}

		public RepeatMessage(string convid, string message, DateTime first, double minutes)
		{
			this.convid = convid;
			this.message = message;

			timer = new Timer(minutes * 60 * 1000);
			timer.Elapsed += new ElapsedEventHandler(execute);

			double timeToStart = (first - DateTime.Now).TotalMilliseconds;
			starter = new Timer(timeToStart);
			starter.AutoReset = false;
			starter.Elapsed += new ElapsedEventHandler(delaystart);
			starter.Enabled = true;
		}

		private void delaystart(object source, ElapsedEventArgs e)
		{
			timer.Enabled = true;
			starter.Enabled = false;
			execute(source, e);
		}

		private void execute(object source, ElapsedEventArgs e)
		{
			FleepBot.Program.SendMessage(convid, message).Wait(-1);
		}
	}
}
