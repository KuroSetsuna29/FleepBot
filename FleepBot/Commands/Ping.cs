using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class Ping : BaseCommand
	{
		public override string command_name { get { return "Ping"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}ping(?:\\s+(.+))?$", FleepBot.Program.ADMIN_COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			if (!FleepBot.Program.ADMIN_CHATS.Contains(convid.ToLower()))
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Admin commands not permitted");
				return;
			}
			
			FleepBot.Program.SendMessage(convid, "pong");
			FleepBot.Program.SendHangouts(FleepBot.Program.HANGOUTS_JAMES, "pong");
		}
	}
}
