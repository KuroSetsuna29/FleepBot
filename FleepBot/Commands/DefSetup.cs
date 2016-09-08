using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class DefSetup : BaseCommand
	{
		public override string command_name { get { return "DefSetup"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}defsetup(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a member. Example: {0}defsetup _Member1_ [, _Member2_ ][, _Member3_ ]", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			List<string> members = search.Split(',', ' ').Select(x => x.ToLower().Trim()).ToList();
			string query = String.Format("select A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, AA, AB, AC, AD, AE, AF, AG, AH, AI, AJ, AK, AL, AM, AN, AO, AP, AQ, AR, AS, AT, AU, AV, AW, AX, AY, AZ, BA, BB, BC, BD, BE, BF, BG, BH, BI, BJ, BK, BL, BM, BN, BO, BP, BQ where lower(A) matches '.*({0}).*'", String.Join("|", members));
			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1Ge91fmZEbNNLzAc4LwoeIqN8w5pdYMwbxgNuAjd25Pk", "720812518", query, 1, "A:BQ");

			if (stats == null)
			{
				return;
			}

            int colName = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "name").id.Value);
            int colHero1A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero1a").id.Value);
            int colUpgrade1A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade1a").id.Value);
            int colSkill1A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill1a").id.Value);
            int colHero2A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero2a").id.Value);
            int colUpgrade2A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade2a").id.Value);
            int colSkill2A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill2a").id.Value);
            int colHero3A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero3a").id.Value);
            int colUpgrade3A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade3a").id.Value);
            int colSkill3A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill3a").id.Value);
            int colHero4A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero4a").id.Value);
            int colUpgrade4A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade4a").id.Value);
            int colSkill4A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill4a").id.Value);
            int colHero5A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero5a").id.Value);
            int colUpgrade5A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade5a").id.Value);
            int colSkill5A = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill5a").id.Value);
            int colHero1B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero1b").id.Value);
            int colUpgrade1B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade1b").id.Value);
            int colSkill1B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill1b").id.Value);
            int colHero2B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero2b").id.Value);
            int colUpgrade2B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade2b").id.Value);
            int colSkill2B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill2b").id.Value);
            int colHero3B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero3b").id.Value);
            int colUpgrade3B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade3b").id.Value);
            int colSkill3B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill3b").id.Value);
            int colHero4B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero4b").id.Value);
            int colUpgrade4B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade4b").id.Value);
            int colSkill4B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill4b").id.Value);
            int colHero5B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "hero5b").id.Value);
            int colUpgrade5B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "upgrade5b").id.Value);
            int colSkill5B = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "shortskill5b").id.Value);
            int colPet1 = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "pet 1").id.Value);
            int colPetStar1 = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "pet star 1").id.Value);
            int colPet2 = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "pet 2").id.Value);
            int colPetStar2 = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "pet star 2").id.Value);
            int colTranscend1 = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "lvl extreme 1").id.Value);
            int colTranscend2 = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "lvl extreme 2").id.Value);
            int colSetItem = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "setitemset").id.Value);
            int colCostume = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "costume").id.Value);
            int col7Skill = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "7*skill").id.Value);
            int colHeroQuality = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "heroquality").id.Value);
            int colRating = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "rating").id.Value);
            int colRank = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "rank").id.Value);
            int col7Count = Utils.ExcelColumnToIndex(stats.Item1.SingleOrDefault(c => c.label.Value.Trim().ToLower() == "7*count").id.Value);
            

			int nameLen = Math.Max((stats.Item2.Max(x => x.c[colName].v.Value.ToString().Trim().Length) ?? 0) + 2, 6);
			int hero1Len = Math.Max(7, Math.Max(
								(stats.Item2.Max(x => (x.c[colHero1A] != null ? x.c[colHero1A].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade1A] != null ? x.c[colUpgrade1B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
								(stats.Item2.Max(x => (x.c[colHero1B] != null ? x.c[colHero1B].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade1B] != null ? x.c[colUpgrade1B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int skill1Len = Math.Max(8, Math.Max(
                                (stats.Item2.Max(x => (x.c[colSkill1A] != null ? x.c[colSkill1A].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colSkill1B] != null ? x.c[colSkill1B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int hero2Len = Math.Max(7, Math.Max(
                                (stats.Item2.Max(x => (x.c[colHero2A] != null ? x.c[colHero2A].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade2A] != null ? x.c[colUpgrade2B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colHero2B] != null ? x.c[colHero2B].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade2B] != null ? x.c[colUpgrade2B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int skill2Len = Math.Max(8, Math.Max(
                                (stats.Item2.Max(x => (x.c[colSkill2A] != null ? x.c[colSkill2A].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colSkill2B] != null ? x.c[colSkill2B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int hero3Len = Math.Max(7, Math.Max(
                                (stats.Item2.Max(x => (x.c[colHero3A] != null ? x.c[colHero3A].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade3A] != null ? x.c[colUpgrade3B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colHero3B] != null ? x.c[colHero3B].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade3B] != null ? x.c[colUpgrade3B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int skill3Len = Math.Max(8, Math.Max(
                                (stats.Item2.Max(x => (x.c[colSkill3A] != null ? x.c[colSkill3A].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colSkill3B] != null ? x.c[colSkill3B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int hero4Len = Math.Max(7, Math.Max(
                                (stats.Item2.Max(x => (x.c[colHero4A] != null ? x.c[colHero4A].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade4A] != null ? x.c[colUpgrade4B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colHero4B] != null ? x.c[colHero4B].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade4B] != null ? x.c[colUpgrade4B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int skill4Len = Math.Max(8, Math.Max(
                                (stats.Item2.Max(x => (x.c[colSkill4A] != null ? x.c[colSkill4A].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colSkill4B] != null ? x.c[colSkill4B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int hero5Len = Math.Max(7, Math.Max(
                                (stats.Item2.Max(x => (x.c[colHero1A] != null ? x.c[colHero5A].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade5A] != null ? x.c[colUpgrade5B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colHero1B] != null ? x.c[colHero5B].v.Value.ToString().Trim().Length : 0) + (x.c[colUpgrade5B] != null ? x.c[colUpgrade5B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int skill5Len = Math.Max(8, Math.Max(
                                (stats.Item2.Max(x => (x.c[colSkill5A] != null ? x.c[colSkill5A].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3,
                                (stats.Item2.Max(x => (x.c[colSkill5B] != null ? x.c[colSkill5B].v.Value.ToString().Trim().Length : 0)) ?? 0) + 3));
            int petLen = Math.Max(5, Math.Max(
								(stats.Item2.Max(x => (x.c[colPet1] != null ? x.c[colPet1].v.Value.ToString().Trim().Length : 0) + (x.c[colPetStar1] != null ? x.c[colPetStar1].v.Value.ToString().Trim().Length : 0)) ?? 0) + 4,
								(stats.Item2.Max(x => (x.c[colPet2] != null ? x.c[colPet2].v.Value.ToString().Trim().Length : 0) + (x.c[colPetStar2] != null ? x.c[colPetStar2].v.Value.ToString().Trim().Length : 0)) ?? 0) + 4));
			int transcendLen = Math.Max(8, Math.Max(
								(stats.Item2.Max(x => (x.c[colTranscend1] != null ? x.c[colTranscend1].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2,
								(stats.Item2.Max(x => (x.c[colTranscend2] != null ? x.c[colTranscend2].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2));
			int setItemLen = Math.Max((stats.Item2.Max(x => (x.c[colSetItem] != null ? x.c[colSetItem].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 9);
			int costumeLen = Math.Max((stats.Item2.Max(x => (x.c[colCostume] != null ? x.c[colCostume].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 9);
			int skill7sLen = Math.Max((stats.Item2.Max(x => (x.c[col7Skill] != null ? x.c[col7Skill].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 9);
            int heroQualityLen = Math.Max((stats.Item2.Max(x => (x.c[colHeroQuality] != null ? x.c[colHeroQuality].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 13);
            int ratingLen = Math.Max((stats.Item2.Max(x => (x.c[colRating] != null ? x.c[colRating].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 8);
			int rankLen = Math.Max((stats.Item2.Max(x => (x.c[colRank] != null ? x.c[colRank].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 6);
			int count7sLen = Math.Max((stats.Item2.Max(x => (x.c[col7Count] != null ? x.c[col7Count].v.Value.ToString().Trim().Length : 0)) ?? 0) + 2, 9);

			string msg = String.Format("No member(s) found for '{0}'. Check spelling.", search);
			if (stats.Item2.Count > 0)
			{
				msg = String.Format(":::\n{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}\n",
						"Name".PadRight(nameLen),
						"Hero1".PadRight(hero1Len),
                        "Skill1".PadRight(skill1Len),
                        "Hero2".PadRight(hero2Len),
                        "Skill2".PadRight(skill2Len),
                        "Hero3".PadRight(hero3Len),
                        "Skill3".PadRight(skill3Len),
                        "Hero4".PadRight(hero4Len),
                        "Skill4".PadRight(skill4Len),
                        "Hero5".PadRight(hero5Len),
                        "Skill5".PadRight(skill5Len),
                        "Pet".PadRight(petLen),
						"Transc".PadRight(transcendLen),
						"SetItem".PadRight(setItemLen),
						"Costume".PadRight(costumeLen),
						"7*Skill".PadRight(skill7sLen),
                        "HeroQuality".PadRight(heroQualityLen),
                        "Rating".PadRight(ratingLen),
						"Rank".PadRight(rankLen),
						"7*Count".PadRight(count7sLen))
						+ String.Join("\n", stats.Item2.Select(x => String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}\n{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}",
						x.c[colName].v.Value.ToString().Trim().PadRight(nameLen),
						((x.c[colHero1A] != null ? x.c[colHero1A].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade1A] != null ? x.c[colUpgrade1A].v.Value.ToString().Trim() : "")).PadRight(hero1Len),
                        (x.c[colSkill1A] != null ? x.c[colSkill1A].v.Value.ToString().Trim() : "").PadRight(skill1Len),
                        ((x.c[colHero2A] != null ? x.c[colHero2A].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade2A] != null ? x.c[colUpgrade2A].v.Value.ToString().Trim() : "")).PadRight(hero2Len),
                        (x.c[colSkill2A] != null ? x.c[colSkill2A].v.Value.ToString().Trim() : "").PadRight(skill2Len),
                        ((x.c[colHero3A] != null ? x.c[colHero3A].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade3A] != null ? x.c[colUpgrade3A].v.Value.ToString().Trim() : "")).PadRight(hero3Len),
                        (x.c[colSkill3A] != null ? x.c[colSkill3A].v.Value.ToString().Trim() : "").PadRight(skill3Len),
                        ((x.c[colHero4A] != null ? x.c[colHero4A].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade4A] != null ? x.c[colUpgrade4A].v.Value.ToString().Trim() : "")).PadRight(hero4Len),
                        (x.c[colSkill4A] != null ? x.c[colSkill4A].v.Value.ToString().Trim() : "").PadRight(skill4Len),
                        ((x.c[colHero5A] != null ? x.c[colHero5A].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade5A] != null ? x.c[colUpgrade5A].v.Value.ToString().Trim() : "")).PadRight(hero5Len),
                        (x.c[colSkill5A] != null ? x.c[colSkill5A].v.Value.ToString().Trim() : "").PadRight(skill5Len),
                        ((x.c[colPetStar1] != null ? x.c[colPetStar1].v.Value.ToString().Trim() : "") + "* " + (x.c[colPet1] != null ? x.c[colPet1].v.Value.ToString().Trim() : "")).PadRight(petLen),
						(x.c[colTranscend1] != null ? x.c[colTranscend1].v.Value.ToString().Trim() : "").PadRight(transcendLen),
						(x.c[colSetItem] != null ? x.c[colSetItem].v.Value.ToString().Trim() : "").PadRight(setItemLen),
						(x.c[colCostume] != null ? x.c[colCostume].v.Value.ToString().Trim() : "").PadRight(costumeLen),
						(x.c[col7Skill] != null ? x.c[col7Skill].v.Value.ToString().Trim() : "").PadRight(skill7sLen),
                        (x.c[colHeroQuality] != null ? x.c[colHeroQuality].v.Value.ToString().Trim() : "").PadRight(heroQualityLen),
                        (x.c[colRating] != null ? x.c[colRating].v.Value.ToString().Trim() : "").PadRight(ratingLen),
						(x.c[colRank] != null ? x.c[colRank].v.Value.ToString().Trim() : "").PadRight(rankLen),
						(x.c[col7Count] != null ? x.c[col7Count].v.Value.ToString().Trim() : "").PadRight(count7sLen),
						"".PadRight(nameLen),
						((x.c[colHero1B] != null ? x.c[colHero1B].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade1A] != null ? x.c[colUpgrade1A].v.Value.ToString().Trim() : "")).PadRight(hero1Len),
                        (x.c[colSkill1B] != null ? x.c[colSkill1B].v.Value.ToString().Trim() : "").PadRight(skill1Len),
                        ((x.c[colHero2B] != null ? x.c[colHero2B].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade2A] != null ? x.c[colUpgrade2A].v.Value.ToString().Trim() : "")).PadRight(hero2Len),
                        (x.c[colSkill2B] != null ? x.c[colSkill2B].v.Value.ToString().Trim() : "").PadRight(skill2Len),
                        ((x.c[colHero3B] != null ? x.c[colHero3B].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade3A] != null ? x.c[colUpgrade3A].v.Value.ToString().Trim() : "")).PadRight(hero3Len),
                        (x.c[colSkill3B] != null ? x.c[colSkill3B].v.Value.ToString().Trim() : "").PadRight(skill3Len),
                        ((x.c[colHero4B] != null ? x.c[colHero4B].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade4A] != null ? x.c[colUpgrade4A].v.Value.ToString().Trim() : "")).PadRight(hero4Len),
                        (x.c[colSkill4B] != null ? x.c[colSkill4B].v.Value.ToString().Trim() : "").PadRight(skill4Len),
                        ((x.c[colHero5B] != null ? x.c[colHero5B].v.Value.ToString().Trim() : "") + "+" + (x.c[colUpgrade5A] != null ? x.c[colUpgrade5A].v.Value.ToString().Trim() : "")).PadRight(hero5Len),
                        (x.c[colSkill5B] != null ? x.c[colSkill5B].v.Value.ToString().Trim() : "").PadRight(skill5Len),
                        ((x.c[colPetStar2] != null ? x.c[colPetStar2].v.Value.ToString().Trim() : "") + "* " + (x.c[colPet2] != null ? x.c[colPet2].v.Value.ToString().Trim() : "")).PadRight(petLen),
						(x.c[colTranscend2] != null ? x.c[colTranscend2].v.Value.ToString().Trim() : "").PadRight(transcendLen))));
			}

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
