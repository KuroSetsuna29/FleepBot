using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class HeroInfo : BaseCommand
	{
		public override string command_name { get { return "HeroInfo"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}heroinfo(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a hero. Example: {0}heroinfo _Hero1_ [, _Hero2_ ][, _Hero3_ ]", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			List<string> heros = search.Split(',', ' ').Select(x => x.ToLower().Trim()).ToList();
			string query = String.Format("select A, D, I, J, K, L, M, N, O, AF, AG, AH where lower(A) matches '.*({0}).*'", String.Join("|", heros));
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "491893686", query, 1, "A4:AH");

			if (stats == null)
			{
				return;
			}

			if (!stats.Item1.Any(c => c.id.Value == "A" && c.label.Value.Trim() == "Name")
				|| !stats.Item1.Any(c => c.id.Value == "D" && c.label.Value.Trim() == "Bonus Effect")
				|| !stats.Item1.Any(c => c.id.Value == "I" && c.label.Value.Trim() == "Aspd")
				|| !stats.Item1.Any(c => c.id.Value == "J" && c.label.Value.Trim() == "Mob")
				|| !stats.Item1.Any(c => c.id.Value == "K" && c.label.Value.Trim() == "Crit")
				|| !stats.Item1.Any(c => c.id.Value == "L" && c.label.Value.Trim() == "ATK")
				|| !stats.Item1.Any(c => c.id.Value == "M" && c.label.Value.Trim() == "DMG per \nSecond")
				|| !stats.Item1.Any(c => c.id.Value == "N" && c.label.Value.Trim() == "DEF")
				|| !stats.Item1.Any(c => c.id.Value == "O" && c.label.Value.Trim() == "HP")
				|| !stats.Item1.Any(c => c.id.Value == "AF" && c.label.Value.Trim() == "Rank")
				|| !stats.Item1.Any(c => c.id.Value == "AG" && c.label.Value.Trim() == "Scenario")
				|| !stats.Item1.Any(c => c.id.Value == "AH" && c.label.Value.Trim() == "Arena"))
			{
				FleepBot.Program.SendErrorMessage(convid);
				return;
			}

			int heroLen = Math.Max((stats.Item2.Max(x => x.c[0] != null && x.c[0].v != null ? x.c[0].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 6);
			int effectLen = Math.Max((stats.Item2.Max(x => x.c[1] != null && x.c[1].v != null ? x.c[1].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 8);
			int atkLen = Math.Max((stats.Item2.Max(x => x.c[5] != null && x.c[5].v != null ? x.c[5].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 7);
			int dpsLen = Math.Max((stats.Item2.Max(x => x.c[6] != null && x.c[6].v != null ? x.c[6].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 7);
			int defLen = Math.Max((stats.Item2.Max(x => x.c[7] != null && x.c[7].v != null ? x.c[7].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 7);
			int hpLen = Math.Max((stats.Item2.Max(x => x.c[8] != null && x.c[8].v != null ? x.c[8].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 6);
			int aspdLen = Math.Max((stats.Item2.Max(x => x.c[2] != null && x.c[2].v != null ? x.c[2].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 6);
			int mobLen = Math.Max((stats.Item2.Max(x => x.c[3] != null && x.c[3].v != null ? x.c[3].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 5);
			int critLen = Math.Max((stats.Item2.Max(x => x.c[4] != null && x.c[4].v != null ? x.c[4].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 6);
			int scoreLen = Math.Max((stats.Item2.Max(x => x.c[9] != null && x.c[9].v != null ? x.c[9].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 7);
			int scenarioLen = Math.Max((stats.Item2.Max(x => x.c[10] != null && x.c[10].v != null ? x.c[10].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 10);
			int arenaLen = Math.Max((stats.Item2.Max(x => x.c[11] != null && x.c[11].v != null ? x.c[11].v.Value.ToString().Trim().Length : 0) ?? 0) + 2, 7);

			string msg = String.Format("No hero(s) found for '{0}'. Check spelling.", search);
			if (stats.Item2.Count > 0)
			{
				msg = String.Format("Hero Info for '{0}':\n:::\n{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}\n",
						search,
						"Hero".PadRight(heroLen),
						"Effect".PadRight(effectLen),
						"ATK+7".PadRight(atkLen),
						"DPS+7".PadRight(dpsLen),
						"DEF+7".PadRight(defLen),
						"HP+7".PadRight(hpLen),
						"Aspd".PadRight(aspdLen),
						"Mob".PadRight(mobLen),
						"Crit".PadRight(critLen),
						"Score".PadRight(scoreLen),
						"Scenario".PadRight(scenarioLen),
						"Arena".PadRight(arenaLen))
						+ String.Join("\n", stats.Item2.Select(x => String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}",
							(x.c[0] != null && x.c[0].v != null ? x.c[0].v.Value.ToString().Trim() : "").PadRight(heroLen),
							(x.c[1] != null && x.c[1].v != null ? x.c[1].v.Value.ToString().Trim() : "").PadRight(effectLen),
							(x.c[5] != null && x.c[5].v != null ? x.c[5].v.Value.ToString().Trim() : "").PadRight(atkLen),
							(x.c[6] != null && x.c[6].v != null ? x.c[6].v.Value.ToString().Trim() : "").PadRight(dpsLen),
							(x.c[7] != null && x.c[7].v != null ? x.c[7].v.Value.ToString().Trim() : "").PadRight(defLen),
							(x.c[8] != null && x.c[8].v != null ? x.c[8].v.Value.ToString().Trim() : "").PadRight(hpLen),
							(x.c[2] != null && x.c[2].v != null ? x.c[2].v.Value.ToString().Trim() : "").PadRight(aspdLen),
							(x.c[3] != null && x.c[3].v != null ? x.c[3].v.Value.ToString().Trim() : "").PadRight(mobLen),
							(x.c[4] != null && x.c[4].v != null ? x.c[4].v.Value.ToString().Trim() : "").PadRight(critLen),
							(x.c[9] != null && x.c[9].v != null ? x.c[9].v.Value.ToString().Trim() : "").PadRight(scoreLen),
							(x.c[10] != null && x.c[10].v != null ? x.c[10].v.Value.ToString().Trim() : "").PadRight(scenarioLen),
							(x.c[11] != null && x.c[11].v != null ? x.c[11].v.Value.ToString().Trim() : "").PadRight(arenaLen))));
			}

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
