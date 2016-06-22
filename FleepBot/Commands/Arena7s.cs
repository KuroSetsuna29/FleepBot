using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
	class Arena7s : BaseCommand
	{
		public override string command_name { get { return "Arena7s"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}arena7s(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a list of players. Example: {0}arena7s _PlayerName1_ [, _PlayerName2_ ]", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			List<string> players = search.Split(',', ' ').Select(x => x.ToLower().Trim()).ToList();
			string query = String.Format("select A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, AA, AB, AC, AD, AE, AF, AG, AH, AI where lower(B) matches lower('.*({0}).*')", String.Join("|", players));
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1Ge91fmZEbNNLzAc4LwoeIqN8w5pdYMwbxgNuAjd25Pk", "1310592142", query, 0, "A2:AI");

			if (stats == null)
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Something unexpected happen. See https://docs.google.com/spreadsheets/d/1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE/edit#gid=763917879");
                return;
			}

			if (stats.Item2.Count == 0)
			{
				FleepBot.Program.SendMessage(convid, String.Format("No info found for '{0}'. Check spelling.", search));
				return;
			}
			
			Regex skill1 = new Regex("(Fire Zone|Storm Zone)", RegexOptions.IgnoreCase);
			Regex skill2 = new Regex("(Goddess Protection|Survival Instinct)", RegexOptions.IgnoreCase);
			Regex skill3 = new Regex("(Oath of Protection|Overcome Death)", RegexOptions.IgnoreCase);
			
			int nameLen = Math.Max((stats.Item2.Max(x => x.c[1].v.Value.ToString().Trim().Length) ?? 0) + 2, 6);
			int updatedLen = Math.Max((stats.Item2.Max(x => x.c[2].f.Value.ToString().Trim().Length) ?? 0) + 2, 14);
			int heroLen = Math.Max(6, Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max(
								(stats.Item2.Max(x => x.c[3] != null ? x.c[3].v.Value.ToString().Trim().Length : 0) ?? 0) + 2,
								(stats.Item2.Max(x => x.c[5] != null ? x.c[5].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[7] != null ? x.c[7].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[9] != null ? x.c[9].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[11] != null ? x.c[11].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
                                (stats.Item2.Max(x => x.c[13] != null ? x.c[13].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[15] != null ? x.c[15].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[17] != null ? x.c[17].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[19] != null ? x.c[19].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
                                (stats.Item2.Max(x => x.c[21] != null ? x.c[21].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[23] != null ? x.c[23].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[25] != null ? x.c[25].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[27] != null ? x.c[27].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[29] != null ? x.c[29].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[31] != null ? x.c[31].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
								(stats.Item2.Max(x => x.c[33] != null ? x.c[33].v.Value.ToString().Trim().Length : 0) ?? 0) + 2));

			string msg = String.Format(":::\n{0}{1}{2}{3}\n",
					"Name".PadRight(nameLen),
					"Last Updated".PadRight(updatedLen),
					"Hero".PadRight(heroLen),
					"    Skill (Z) >> (H) >> (S) >> (Z)")
					+ String.Join("\n", stats.Item2.Select(x => String.Format("{0}{1}{2}",
						(x.c[1] != null ? x.c[1].v.Value.ToString().Trim() : "").PadRight(nameLen),
						(x.c[2] != null ? x.c[2].f.Value.ToString().Trim() : "").PadRight(updatedLen),
						(x.c[3] != null ? (x.c[3].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[4].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[4].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[4].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[4].v.Value.ToString().Trim()) : "")
							+ (x.c[5] != null && !String.IsNullOrWhiteSpace(x.c[5].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[5].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[6].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[6].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[6].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[6].v.Value.ToString().Trim()) : "")
							+ (x.c[7] != null && !String.IsNullOrWhiteSpace(x.c[7].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[7].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[8].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[8].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[8].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[8].v.Value.ToString().Trim()) : "")
							+ (x.c[9] != null && !String.IsNullOrWhiteSpace(x.c[9].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[9].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[10].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[10].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[10].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[10].v.Value.ToString().Trim()) : "")
							+ (x.c[11] != null && !String.IsNullOrWhiteSpace(x.c[11].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[11].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[12].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[12].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[12].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[12].v.Value.ToString().Trim()) : "")
							+ (x.c[13] != null && !String.IsNullOrWhiteSpace(x.c[13].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[13].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[14].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[14].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[14].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[14].v.Value.ToString().Trim()) : "")
							+ (x.c[15] != null && !String.IsNullOrWhiteSpace(x.c[15].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[15].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[16].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[16].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[16].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[16].v.Value.ToString().Trim()) : "")
							+ (x.c[17] != null && !String.IsNullOrWhiteSpace(x.c[17].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[17].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[18].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[18].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[18].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[18].v.Value.ToString().Trim()) : "")
							+ (x.c[19] != null && !String.IsNullOrWhiteSpace(x.c[19].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[19].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[20].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[20].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[20].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[20].v.Value.ToString().Trim()) : "")
							+ (x.c[21] != null && !String.IsNullOrWhiteSpace(x.c[21].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[21].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[22].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[22].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[22].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[22].v.Value.ToString().Trim()) : "")
							+ (x.c[23] != null && !String.IsNullOrWhiteSpace(x.c[23].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[23].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[24].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[24].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[24].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[24].v.Value.ToString().Trim()) : "")
							+ (x.c[25] != null && !String.IsNullOrWhiteSpace(x.c[25].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[25].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[26].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[26].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[26].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[26].v.Value.ToString().Trim()) : "")
							+ (x.c[27] != null && !String.IsNullOrWhiteSpace(x.c[27].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[27].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[28].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[28].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[28].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[28].v.Value.ToString().Trim()) : "")
							+ (x.c[29] != null && !String.IsNullOrWhiteSpace(x.c[29].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[29].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[30].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[30].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[30].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[30].v.Value.ToString().Trim()) : "")
							+ (x.c[31] != null && !String.IsNullOrWhiteSpace(x.c[31].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[31].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[32].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[32].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[32].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[32].v.Value.ToString().Trim()) : "")
							+ (x.c[33] != null && !String.IsNullOrWhiteSpace(x.c[33].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen+updatedLen) + x.c[33].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[34].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[34].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[34].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[34].v.Value.ToString().Trim()) : ""))));

				FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
