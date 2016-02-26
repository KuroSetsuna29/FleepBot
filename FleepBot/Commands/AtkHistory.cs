using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class AtkHistory : BaseCommand
	{
		public override string command_name { get { return "AtkHistory"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}(my)?atkhistory(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			bool isIndividual = regex.Match(message).Groups[1].Length > 0;
			string guild = regex.Match(message).Groups[2].Value;

			if (isIndividual)
			{
				Program.SendErrorMessage(convid, String.Format("Please use {0}mymatchup [ _GuildName_ | _Opponent_ ] instead.", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			if (String.IsNullOrWhiteSpace(guild))
			{
				Program.SendErrorMessage(convid, String.Format("Error: Please specify guild name. Example: {0}atkhistory _GuildName_", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			string contact_name = "";
			if (isIndividual)
			{
				contact_name = FleepBot.Program.GetUserName(account_id);

				if (String.IsNullOrEmpty(contact_name))
				{
					FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please set your IGN using the following command, {0}ign _InGameName_", FleepBot.Program.COMMAND_PREFIX));
					return;
				}
			}

			string query = String.Format("select * where lower(A) = 'atk' {0} and lower(D) = '{1}' order by B, E desc, C", (isIndividual ? "and lower(B) = '" + contact_name.ToLower() + "'" : ""), guild.ToLower());
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1ZIP1vK0DJJjYdsU-CY1wF9It2rLaIjjT4bMWyT4ZvsY", null, query, 1);

			if (stats == null)
			{
				return;
			}

			if (!stats.Item1.Any(c => c.id.Value == "A" && c.label.Value == "Type")
				|| !stats.Item1.Any(c => c.id.Value == "B" && c.label.Value == "Member")
				|| !stats.Item1.Any(c => c.id.Value == "C" && c.label.Value == "Opponent")
				|| !stats.Item1.Any(c => c.id.Value == "D" && c.label.Value == "Guild")
				|| !stats.Item1.Any(c => c.id.Value == "E" && c.label.Value == "Result"))
			{
				FleepBot.Program.SendErrorMessage(convid);
				return;
			}

			int memberLen = Math.Max((stats.Item2.Max(x => x.c[1].v.Value.Length) ?? 0) + 2, 8);
			int opponentLen = Math.Max((stats.Item2.Max(x => x.c[2].v.Value.Length) ?? 0) + 2, 10);

			string msg = String.Format(":::\n{0}{1}{2}\n", "Member".PadRight(memberLen), "Opponent".PadRight(opponentLen), "Result")
				+ String.Join("\n", stats.Item2.Select(x => String.Format("{0}{1}{2}", x.c[1].v.Value.PadRight(memberLen), x.c[2].v.Value.PadRight(opponentLen), x.c[4].v.Value)));

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
