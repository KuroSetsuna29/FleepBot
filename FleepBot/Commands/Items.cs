using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
	class Items : BaseCommand
	{
		public override string command_name { get { return "Items"; } }
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}items(?:\\s+(.+))?</p></msg>$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a hero. Example: {0}items _Item1_ [, _Item2_ ]", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			List<string> items = search.Split(',', ' ').Select(x => x.ToLower().Trim()).ToList();
			string query = String.Format("select * where lower(A) matches '.*({0}).*' or lower(C) matches '.*({0}).*'", String.Join("|", items));
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "1108603960", query, 1, "A:V");

			if (stats == null)
			{
				return;
			}

			if (!stats.Item1.Any(c => c.id.Value == "A" && c.label.Value.Trim() == "ItemName")
				|| !stats.Item1.Any(c => c.id.Value == "B" && c.label.Value.Trim() == "ItemClass")
				|| !stats.Item1.Any(c => c.id.Value == "C" && c.label.Value.Trim() == "Set")
				|| !stats.Item1.Any(c => c.id.Value == "D" && c.label.Value.Trim() == "ATK")
				|| !stats.Item1.Any(c => c.id.Value == "E" && c.label.Value.Trim() == "DEF")
				|| !stats.Item1.Any(c => c.id.Value == "F" && c.label.Value.Trim() == "HP")
				|| !stats.Item1.Any(c => c.id.Value == "G" && c.label.Value.Trim() == "Skill ATK")
				|| !stats.Item1.Any(c => c.id.Value == "H" && c.label.Value.Trim() == "ATK Speed")
				|| !stats.Item1.Any(c => c.id.Value == "I" && c.label.Value.Trim() == "Mob")
				|| !stats.Item1.Any(c => c.id.Value == "J" && c.label.Value.Trim() == "CRIT")
				|| !stats.Item1.Any(c => c.id.Value == "K" && c.label.Value.Trim() == "Dodge")
				|| !stats.Item1.Any(c => c.id.Value == "L" && c.label.Value.Trim() == "CC")
				|| !stats.Item1.Any(c => c.id.Value == "M" && c.label.Value.Trim() == "Ignore Def")
				|| !stats.Item1.Any(c => c.id.Value == "N" && c.label.Value.Trim() == "Decrease Melee")
				|| !stats.Item1.Any(c => c.id.Value == "O" && c.label.Value.Trim() == "Decrease Range")
				|| !stats.Item1.Any(c => c.id.Value == "P" && c.label.Value.Trim() == "Fire Res")
				|| !stats.Item1.Any(c => c.id.Value == "Q" && c.label.Value.Trim() == "Ice Res")
				|| !stats.Item1.Any(c => c.id.Value == "R" && c.label.Value.Trim() == "Electr. Res")
				|| !stats.Item1.Any(c => c.id.Value == "S" && c.label.Value.Trim() == "Poison Res")
				|| !stats.Item1.Any(c => c.id.Value == "T" && c.label.Value.Trim() == "Support Time")
				|| !stats.Item1.Any(c => c.id.Value == "U" && c.label.Value.Trim() == "Decrease\nCooltime")
				|| !stats.Item1.Any(c => c.id.Value == "V" && c.label.Value.Trim() == "Bonus\nEffect Time"))
			{
				FleepBot.Program.SendErrorMessage(convid);
				return;
			}
            
			string msg = String.Format("No item(s) found for '{0}'. Check spelling.", search);
			if (stats.Item2.Count > 0)
			{
				msg = String.Format("Item Search Result for '{0}':\n:::\n[Class] ItemSet: ItemName ATK/DEF/HP - attribute:value\n", search)
						+ String.Join("\n", stats.Item2.Where(x => x.c[1] == null || x.c[1].v == null || x.c[1].v.Value.ToString().ToUpper() != "SET").Select(x => String.Format("[{1}] {2}: {0} {3}/{4}/{5} -{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}",
						(x.c[0] != null && x.c[0].v != null ? x.c[0].v.Value.ToString().Trim() : ""),
						(x.c[1] != null && x.c[1].v != null ? x.c[1].v.Value.ToString().Trim().Substring(0, 3).ToUpper().Replace("WEA", "WEP") : ""),
						(x.c[2] != null && x.c[2].v != null ? x.c[2].v.Value.ToString().Trim() : ""),
						(x.c[3] != null && x.c[3].v != null && !String.IsNullOrWhiteSpace(x.c[3].v.Value.ToString()) ? (x.c[3].v.Value.ToString().Trim()) : "0"),
						(x.c[4] != null && x.c[4].v != null && !String.IsNullOrWhiteSpace(x.c[4].v.Value.ToString()) ? (x.c[4].v.Value.ToString().Trim()) : "0"),
						(x.c[5] != null && x.c[5].v != null && !String.IsNullOrWhiteSpace(x.c[5].v.Value.ToString()) ? (x.c[5].v.Value.ToString().Trim()) : "0"),
						(x.c[6] != null && x.c[6].v != null && !String.IsNullOrWhiteSpace(x.c[6].v.Value.ToString()) ? (" SATK:" + x.c[6].v.Value.ToString().Trim()) : ""),
						(x.c[7] != null && x.c[7].v != null && !String.IsNullOrWhiteSpace(x.c[7].v.Value.ToString()) ? (" AtkSpd:" + x.c[7].v.Value.ToString().Trim()) : ""),
						(x.c[8] != null && x.c[8].v != null && !String.IsNullOrWhiteSpace(x.c[8].v.Value.ToString()) ? (" Mob:" + x.c[8].v.Value.ToString().Trim()) : ""),
						(x.c[9] != null && x.c[9].v != null && !String.IsNullOrWhiteSpace(x.c[9].v.Value.ToString()) ? (" Crit:" + x.c[9].v.Value.ToString().Trim()) : ""),
						(x.c[10] != null && x.c[10].v != null && !String.IsNullOrWhiteSpace(x.c[10].v.Value.ToString()) ? (" Dodge:" + x.c[10].v.Value.ToString().Trim()) : ""),
						(x.c[11] != null && x.c[11].v != null && !String.IsNullOrWhiteSpace(x.c[11].v.Value.ToString()) ? (" CC:" + x.c[11].v.Value.ToString().Trim()) : ""),
						(x.c[12] != null && x.c[12].v != null && !String.IsNullOrWhiteSpace(x.c[12].v.Value.ToString()) ? (" IgnDef:" + x.c[12].v.Value.ToString().Trim()) : ""),
						(x.c[13] != null && x.c[13].v != null && !String.IsNullOrWhiteSpace(x.c[13].v.Value.ToString()) ? (" DecMelee:" + x.c[13].v.Value.ToString().Trim()) : ""),
						(x.c[14] != null && x.c[14].v != null && !String.IsNullOrWhiteSpace(x.c[14].v.Value.ToString()) ? (" DecRange:" + x.c[14].v.Value.ToString().Trim()) : ""),
						(x.c[15] != null && x.c[15].v != null && !String.IsNullOrWhiteSpace(x.c[15].v.Value.ToString()) ? (" FireRes:" + x.c[15].v.Value.ToString().Trim()) : ""),
						(x.c[16] != null && x.c[16].v != null && !String.IsNullOrWhiteSpace(x.c[16].v.Value.ToString()) ? (" IceRes:" + x.c[16].v.Value.ToString().Trim()) : ""),
						(x.c[17] != null && x.c[17].v != null && !String.IsNullOrWhiteSpace(x.c[17].v.Value.ToString()) ? (" ElecRes:" + x.c[17].v.Value.ToString().Trim()) : ""),
						(x.c[18] != null && x.c[18].v != null && !String.IsNullOrWhiteSpace(x.c[18].v.Value.ToString()) ? (" PoisRes:" + x.c[18].v.Value.ToString().Trim()) : ""),
						(x.c[19] != null && x.c[19].v != null && !String.IsNullOrWhiteSpace(x.c[19].v.Value.ToString()) ? (" SupTime:" + x.c[19].v.Value.ToString().Trim()) : ""),
						(x.c[20] != null && x.c[20].v != null && !String.IsNullOrWhiteSpace(x.c[20].v.Value.ToString()) ? (" CD:" + x.c[20].v.Value.ToString().Trim()) : ""),
						(x.c[21] != null && x.c[21].v != null && !String.IsNullOrWhiteSpace(x.c[21].v.Value.ToString()) ? (" BonusTime:" + x.c[21].v.Value.ToString().Trim()) : ""))))
						+ "\n\n" + String.Join("\n", stats.Item2.Where(x => x.c[1] != null && x.c[1].v != null && x.c[1].v.Value.ToString().ToUpper() == "SET").Select(x => String.Format("[{1}] {2}: {0} {3}/{4}/{5} -{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}",
						(x.c[0] != null && x.c[0].v != null ? x.c[0].v.Value.ToString().Trim() : ""),
						(x.c[1] != null && x.c[1].v != null ? x.c[1].v.Value.ToString().Trim().Substring(0, 3).ToUpper().Replace("WEA", "WEP") : ""),
						(x.c[2] != null && x.c[2].v != null ? x.c[2].v.Value.ToString().Trim() : ""),
						(x.c[3] != null && x.c[3].v != null && !String.IsNullOrWhiteSpace(x.c[3].v.Value.ToString()) ? (x.c[3].v.Value.ToString().Trim()) : "0"),
						(x.c[4] != null && x.c[4].v != null && !String.IsNullOrWhiteSpace(x.c[4].v.Value.ToString()) ? (x.c[4].v.Value.ToString().Trim()) : "0"),
						(x.c[5] != null && x.c[5].v != null && !String.IsNullOrWhiteSpace(x.c[5].v.Value.ToString()) ? (x.c[5].v.Value.ToString().Trim()) : "0"),
						(x.c[6] != null && x.c[6].v != null && !String.IsNullOrWhiteSpace(x.c[6].v.Value.ToString()) ? (" SATK:" + x.c[6].v.Value.ToString().Trim()) : ""),
						(x.c[7] != null && x.c[7].v != null && !String.IsNullOrWhiteSpace(x.c[7].v.Value.ToString()) ? (" AtkSpd:" + x.c[7].v.Value.ToString().Trim()) : ""),
						(x.c[8] != null && x.c[8].v != null && !String.IsNullOrWhiteSpace(x.c[8].v.Value.ToString()) ? (" Mob:" + x.c[8].v.Value.ToString().Trim()) : ""),
						(x.c[9] != null && x.c[9].v != null && !String.IsNullOrWhiteSpace(x.c[9].v.Value.ToString()) ? (" Crit:" + x.c[9].v.Value.ToString().Trim()) : ""),
						(x.c[10] != null && x.c[10].v != null && !String.IsNullOrWhiteSpace(x.c[10].v.Value.ToString()) ? (" Dodge:" + x.c[10].v.Value.ToString().Trim()) : ""),
						(x.c[11] != null && x.c[11].v != null && !String.IsNullOrWhiteSpace(x.c[11].v.Value.ToString()) ? (" CC:" + x.c[11].v.Value.ToString().Trim()) : ""),
						(x.c[12] != null && x.c[12].v != null && !String.IsNullOrWhiteSpace(x.c[12].v.Value.ToString()) ? (" IgnDef:" + x.c[12].v.Value.ToString().Trim()) : ""),
						(x.c[13] != null && x.c[13].v != null && !String.IsNullOrWhiteSpace(x.c[13].v.Value.ToString()) ? (" DecMelee:" + x.c[13].v.Value.ToString().Trim()) : ""),
						(x.c[14] != null && x.c[14].v != null && !String.IsNullOrWhiteSpace(x.c[14].v.Value.ToString()) ? (" DecRange:" + x.c[14].v.Value.ToString().Trim()) : ""),
						(x.c[15] != null && x.c[15].v != null && !String.IsNullOrWhiteSpace(x.c[15].v.Value.ToString()) ? (" FireRes:" + x.c[15].v.Value.ToString().Trim()) : ""),
						(x.c[16] != null && x.c[16].v != null && !String.IsNullOrWhiteSpace(x.c[16].v.Value.ToString()) ? (" IceRes:" + x.c[16].v.Value.ToString().Trim()) : ""),
						(x.c[17] != null && x.c[17].v != null && !String.IsNullOrWhiteSpace(x.c[17].v.Value.ToString()) ? (" ElecRes:" + x.c[17].v.Value.ToString().Trim()) : ""),
						(x.c[18] != null && x.c[18].v != null && !String.IsNullOrWhiteSpace(x.c[18].v.Value.ToString()) ? (" PoisRes:" + x.c[18].v.Value.ToString().Trim()) : ""),
						(x.c[19] != null && x.c[19].v != null && !String.IsNullOrWhiteSpace(x.c[19].v.Value.ToString()) ? (" SupTime:" + x.c[19].v.Value.ToString().Trim()) : ""),
						(x.c[20] != null && x.c[20].v != null && !String.IsNullOrWhiteSpace(x.c[20].v.Value.ToString()) ? (" CD:" + x.c[20].v.Value.ToString().Trim()) : ""),
						(x.c[21] != null && x.c[21].v != null && !String.IsNullOrWhiteSpace(x.c[21].v.Value.ToString()) ? (" BonusTime:" + x.c[21].v.Value.ToString().Trim()) : ""))));
			}

			Regex r = new Regex(" -(\\n|$)");
			FleepBot.Program.SendMessage(convid, r.Replace(msg, "$1"));
		}
	}
}
