using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class IGN : BaseCommand
	{
		public override string command_name { get { return "IGN"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}ign(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string ign = regex.Match(message).Groups[1].Value;
			bool isGet = String.IsNullOrEmpty(ign);

			if (isGet)
			{
				string contact_name = FleepBot.Program.GetUserName(account_id);

				FleepBot.Program.SendMessage(convid, String.Format("Your IGN is {0}", String.IsNullOrEmpty(contact_name) ? String.Format("not set. Please set using the following command, {0}ign _InGameName_", FleepBot.Program.COMMAND_PREFIX) : "*" + contact_name + "*"));
			}
			else
			{
				dynamic memberinfo = FleepBot.Program.ApiPost("api/contact/describe", new { contact_id = account_id, contact_name = ign, ticket = FleepBot.Program.TICKET });
				string contact_name = memberinfo.contact_name;

				if (!String.IsNullOrEmpty(contact_name))
				{
					FleepBot.Program.SendMessage(convid, String.Format("Successfully set your IGN to *{0}*", contact_name));
				}
				else
				{
					FleepBot.Program.SendErrorMessage(convid);
				}
			}
		}
	}
}
