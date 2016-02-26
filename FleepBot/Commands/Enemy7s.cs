using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
	class Enemy7s : BaseCommand
	{
		public override string command_name { get { return "Enemy7s"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}enemy7s(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify an enemy guild. Example: {0}enemy7s _Guild_", FleepBot.Program.COMMAND_PREFIX));
				return;
			}
			
			string query = String.Format("select A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X where lower(A) matches lower('.*({0}).*')", String.Join("|", search));
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1Ge91fmZEbNNLzAc4LwoeIqN8w5pdYMwbxgNuAjd25Pk", "1310592142", query, 0, "A2:X");

			if (stats == null)
			{
				FleepBot.Program.SendErrorMessage(convid, "Error: Something unexpected happen. See https://docs.google.com/spreadsheets/d/1Ge91fmZEbNNLzAc4LwoeIqN8w5pdYMwbxgNuAjd25Pk/edit#gid=1310592142");
                return;
			}

			if (stats.Item2.Count == 0)
			{
				FleepBot.Program.SendMessage(convid, String.Format("No info found for '{0}'. Check spelling.", search));
				return;
			}

			List<string> guilds = stats.Item2.Select<dynamic, string>(x => x.c[0].v.Value).Distinct().ToList();
			Regex skill1 = new Regex("(Fire Zone|Storm Zone)", RegexOptions.IgnoreCase);
			Regex skill2 = new Regex("(Goddess Protection|Survival Instinct)", RegexOptions.IgnoreCase);
			Regex skill3 = new Regex("(Oath of Protection|Overcome Death)", RegexOptions.IgnoreCase);

			foreach (string guild in guilds)
			{
				int nameLen = Math.Max((stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[1].v.Value.ToString().Trim().Length) ?? 0) + 2, 6);
				int heroLen = Math.Max(6, Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max( Math.Max(
									(stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[2] != null ? x.c[2].v.Value.ToString().Trim().Length : 0) ?? 0) + 2,
									(stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[4] != null ? x.c[4].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
									(stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[6] != null ? x.c[6].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
									(stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[8] != null ? x.c[8].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
									(stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[10] != null ? x.c[10].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
                                    (stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[12] != null ? x.c[12].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
									(stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[14] != null ? x.c[14].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
									(stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[16] != null ? x.c[16].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
									(stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[18] != null ? x.c[18].v.Value.ToString().Trim().Length : 0) ?? 0) + 2),
                                    (stats.Item2.Where(x => x.c[0].v.Value == guild).Max(x => x.c[20] != null ? x.c[20].v.Value.ToString().Trim().Length : 0) ?? 0) + 2));

				string msg = String.Format(":::\n{0}\n{1}{2}{3}\n",
						guild,
						"Name".PadRight(nameLen),
						"Hero".PadRight(heroLen),
						"    Skill (Z) >> (H) >> (S) >> (Z)")
						+ String.Join("\n", stats.Item2.Where(x => x.c[0].v.Value == guild).Select(x => String.Format("{0}{1}",
							(x.c[1] != null ? x.c[1].v.Value.ToString().Trim() : "").PadRight(nameLen),
							(x.c[2] != null ? (x.c[2].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[3].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[3].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[3].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[3].v.Value.ToString().Trim()) : "")
								+ (x.c[4] != null && !String.IsNullOrWhiteSpace(x.c[4].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[4].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[5].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[5].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[5].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[5].v.Value.ToString().Trim()) : "")
								+ (x.c[6] != null && !String.IsNullOrWhiteSpace(x.c[6].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[6].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[7].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[7].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[7].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[7].v.Value.ToString().Trim()) : "")
								+ (x.c[8] != null && !String.IsNullOrWhiteSpace(x.c[8].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[8].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[9].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[9].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[9].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[9].v.Value.ToString().Trim()) : "")
								+ (x.c[10] != null && !String.IsNullOrWhiteSpace(x.c[10].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[10].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[11].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[11].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[11].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[11].v.Value.ToString().Trim()) : "")
								+ (x.c[12] != null && !String.IsNullOrWhiteSpace(x.c[12].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[12].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[13].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[13].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[13].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[13].v.Value.ToString().Trim()) : "")
								+ (x.c[14] != null && !String.IsNullOrWhiteSpace(x.c[14].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[14].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[15].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[15].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[15].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[15].v.Value.ToString().Trim()) : "")
								+ (x.c[16] != null && !String.IsNullOrWhiteSpace(x.c[16].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[16].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[17].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[17].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[17].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[17].v.Value.ToString().Trim()) : "")
								+ (x.c[18] != null && !String.IsNullOrWhiteSpace(x.c[18].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[18].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[19].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[19].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[19].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[19].v.Value.ToString().Trim()) : "")
								+ (x.c[20] != null && !String.IsNullOrWhiteSpace(x.c[20].v.Value.ToString()) ? ("\n" + "".PadRight(nameLen) + x.c[20].v.Value.ToString().Trim().PadRight(heroLen) + (skill1.IsMatch(x.c[21].v.Value.ToString().Trim()) ? "(Z)" : (skill2.IsMatch(x.c[21].v.Value.ToString().Trim()) ? "(H)" : (skill3.IsMatch(x.c[21].v.Value.ToString().Trim()) ? "(S)" : ""))).PadRight(4) + x.c[21].v.Value.ToString().Trim()) : ""))))
						+ "\n:::\nhttps://docs.google.com/spreadsheets/d/1Ge91fmZEbNNLzAc4LwoeIqN8w5pdYMwbxgNuAjd25Pk/edit#gid=1310592142";

				FleepBot.Program.SendMessage(convid, msg);
			}
		}
	}
}
