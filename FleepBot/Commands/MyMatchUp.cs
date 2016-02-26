using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class MyMatchUp : BaseCommand
	{
		public override string command_name { get { return "MyMatchUp"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}mymatchup(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			//bool isIndividual = r.Match(message).Groups[1].Length > 0;
			string opponent = regex.Match(message).Groups[1].Value;

			/*if (String.IsNullOrWhiteSpace(guild))
            {
                FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify opponent name. Example: {0}mymatchup _Opponent_", FleepBot.Program.COMMAND_PREFIX));
                return;
            }*/
			
			//if (isIndividual)
			//{
			string contact_name = FleepBot.Program.GetUserName(account_id);

			if (String.IsNullOrEmpty(contact_name))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please set your IGN using the following command, {0}ign _InGameName_", FleepBot.Program.COMMAND_PREFIX));
				return;
			}
			//}

			string url = String.Format("https://script.google.com/macros/s/AKfycbytsEiFRiPgbxifgQiuNzxi6wzH5QdBNXxwPP3FVm807Qxqjig/exec?query=matchup&Member={0}", contact_name);
			dynamic stats = FleepBot.Program.GetAsJson(url);

			if (stats.result != "success")
			{
				FleepBot.Program.SendErrorMessage(convid);
				return;
			}

			List<string> opponents = opponent.Split(',', ' ').Select(x => x.Trim()).ToList();
			Regex rgx = new Regex(String.Format(".*({0}).*", String.Join("|", opponents)), RegexOptions.IgnoreCase);
			Dictionary<string, dynamic> matchups = ObjectToDictionaryHelper.ToDictionary(stats.@return);
			var matchupsFiltered = matchups.Where(x => rgx.IsMatch(x.Key) || rgx.IsMatch((x.Value.Guild ?? "").Value))
											.OrderBy(x => x.Value.Guild ?? "").ThenBy(x => x.Key);

			int guildLen = Math.Max(matchupsFiltered.Max(x => (x.Value.Guild ?? "").Value.Length) ?? 0, 5) + 2;
			int opponentLen = Math.Max(matchupsFiltered.Count() == 0 ? 0 : matchupsFiltered.Max(x => x.Key.Length), 8) + 2;
			int scoreLen = 7;
			int atkLen = 5;
			int defLen = 5;

			string msg = String.Format(":::\n{0}{1}{2}{3}{4}\n", "Guild".PadRight(guildLen), "Opponent".PadRight(opponentLen), "Score".PadRight(scoreLen), "ATK".PadRight(atkLen), "DEF".PadRight(defLen))
				+ String.Join("\n", matchupsFiltered.Select(x => String.Format("{0}{1}{2}{3}{4}",
					(x.Value.Guild ?? "").Value.PadRight(guildLen),
					x.Key.PadRight(opponentLen),
					(x.Value.score != null ? x.Value.score.Value.ToString().PadRight(scoreLen) : "".PadRight(scoreLen)),
					(x.Value.atk != null ? x.Value.atk.Value.ToString().PadRight(atkLen) : "".PadRight(atkLen)),
					(x.Value.def != null ? x.Value.def.Value.ToString().PadRight(defLen) : "".PadRight(defLen)))));

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
