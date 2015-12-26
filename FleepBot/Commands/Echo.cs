using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class Echo
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}echo(?:\\s+(.+))?</p></msg>$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static void execute(string convid, string message)
		{
			string input = regex.Match(message).Groups[1].Value;
			FleepBot.Program.SendMessage(convid, input);
		}
	}
}
