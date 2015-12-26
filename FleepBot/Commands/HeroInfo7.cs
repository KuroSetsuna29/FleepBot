using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
	class HeroInfo7
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}heroinfo7(?:\\s+(.+))?</p></msg>$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static void execute(string convid, string message)
		{
			string search = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a hero. Example: {0}heroinfo7 _Hero1_ [, _Hero2_ ][, _Hero3_ ]", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			List<string> heros = search.Split(',', ' ').Select(x => x.ToLower().Trim()).ToList();
			string query = String.Format("select A, D, E, J, K, L, M, N, O, P where lower(A) matches '.*({0}).*'", String.Join("|", heros));
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "407202420", query, 1, "A4:P");

			if (stats == null)
			{
				return;
			}

			if (!stats.Item1.Any(c => c.id.Value == "A" && c.label.Value.Trim() == "Name")
				|| !stats.Item1.Any(c => c.id.Value == "D" && c.label.Value.Trim() == "Bonus Effect")
				|| !stats.Item1.Any(c => c.id.Value == "E" && c.label.Value.Trim() == "Skill\nCool-\ndown")
				|| !stats.Item1.Any(c => c.id.Value == "J" && c.label.Value.Trim() == "Aspd")
				|| !stats.Item1.Any(c => c.id.Value == "K" && c.label.Value.Trim() == "Mob")
				|| !stats.Item1.Any(c => c.id.Value == "L" && c.label.Value.Trim() == "Crit")
				|| !stats.Item1.Any(c => c.id.Value == "M" && c.label.Value.Trim() == "ATK")
				|| !stats.Item1.Any(c => c.id.Value == "N" && c.label.Value.Trim() == "DMG per \nSecond")
				|| !stats.Item1.Any(c => c.id.Value == "O" && c.label.Value.Trim() == "DEF")
				|| !stats.Item1.Any(c => c.id.Value == "P" && c.label.Value.Trim() == "HP"))
			{
				FleepBot.Program.SendErrorMessage(convid);
				return;
			}

			int heroLen = Math.Max((stats.Item2.Max(x => x.c[0].v.Value.ToString().Trim().Length) ?? 0) + 2, 6);
			int effectLen = Math.Max((stats.Item2.Max(x => x.c[1].v.Value.ToString().Trim().Length) ?? 0) + 2, 8);
			int cooldownLen = Math.Max((stats.Item2.Max(x => x.c[2].v.Value.ToString().Trim().Length) ?? 0) + 2, 4);
			int atkLen = Math.Max((stats.Item2.Max(x => x.c[6].v.Value.ToString().Trim().Length) ?? 0) + 2, 7);
			int dpsLen = Math.Max((stats.Item2.Max(x => x.c[7].v.Value.ToString().Trim().Length) ?? 0) + 2, 7);
			int defLen = Math.Max((stats.Item2.Max(x => x.c[8].v.Value.ToString().Trim().Length) ?? 0) + 2, 7);
			int hpLen = Math.Max((stats.Item2.Max(x => x.c[9].v.Value.ToString().Trim().Length) ?? 0) + 2, 6);
			int aspdLen = Math.Max((stats.Item2.Max(x => x.c[3].v.Value.ToString().Trim().Length) ?? 0) + 2, 6);
			int mobLen = Math.Max((stats.Item2.Max(x => x.c[4].v.Value.ToString().Trim().Length) ?? 0) + 2, 5);
			int critLen = Math.Max((stats.Item2.Max(x => x.c[5].v.Value.ToString().Trim().Length) ?? 0) + 2, 6); ;

			string msg = String.Format("No hero(s) found for '{0}'. Check spelling.", search);
			if (stats.Item2.Count > 0)
			{
				msg = String.Format(":::\n{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}\n",
						"Hero".PadRight(heroLen),
						"Effect".PadRight(effectLen),
						"CD".PadRight(cooldownLen),
						"ATK+7".PadRight(atkLen),
						"DPS+7".PadRight(dpsLen),
						"DEF+7".PadRight(defLen),
						"HP+7".PadRight(hpLen),
						"Aspd".PadRight(aspdLen),
						"Mob".PadRight(mobLen),
						"Crit".PadRight(critLen))
						+ String.Join("\n", stats.Item2.Select(x => String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
						x.c[0].v.Value.ToString().Trim().PadRight(heroLen),
						x.c[1].v.Value.ToString().Trim().PadRight(effectLen),
						x.c[2].v.Value.ToString().Trim().PadRight(cooldownLen),
						x.c[6].v.Value.ToString().Trim().PadRight(atkLen),
						x.c[7].v.Value.ToString().Trim().PadRight(dpsLen),
						x.c[8].v.Value.ToString().Trim().PadRight(defLen),
						x.c[9].v.Value.ToString().Trim().PadRight(hpLen),
						x.c[3].v.Value.ToString().Trim().PadRight(aspdLen),
						x.c[4].v.Value.ToString().Trim().PadRight(mobLen),
						x.c[5].v.Value.ToString().Trim().PadRight(critLen))));
			}

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
