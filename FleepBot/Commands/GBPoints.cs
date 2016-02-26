using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class GBPoints : BaseCommand
	{
		public override string command_name { get { return "GBPoints"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}gbpoints(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string input = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrEmpty(input))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify _GuildName_ and _Points_.", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			List<string> lines = input.Split(new string[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries).ToList();


			List<dynamic> output = new List<dynamic>();
			foreach (string line in lines)
			{
				List<string> param = line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).ToList();

				try
				{

					if (param.Count != 2)
					{
						output.Add(new { guild = param[0], points = "", delta = "", atk_wins = "", atk_losses = "", def_wins = "", def_losses = "", error = "Unexpected number of parameters." });
						continue;
					}

					string url = String.Format("https://script.google.com/macros/s/AKfycbwOBfTs6ZFxr5MCYoULN9JqxCOT1cpjrO0V4l4enPYbjWfotag/exec?Guild={0}&Points={1}", param[0], param[1]);
					dynamic resp = FleepBot.Program.GetAsJson(url);

					if (resp.result == "success")
					{
						output.Add(new
						{
							guild = resp.entry[1].Value.ToString(),
							points = resp.entry[2].Value.ToString(),
							delta = resp.entry[3].Value.ToString(),
							atk_wins = resp.entry[4].Value.ToString(),
							atk_losses = resp.entry[5].Value.ToString(),
							def_wins = resp.entry[6].Value.ToString(),
							def_losses = resp.entry[7].Value.ToString(),
							error = ""
						});
					}
					else if (resp.result == "error")
					{
						output.Add(new { guild = param[0], points = "", delta = "", atk_wins = "", atk_losses = "", def_wins = "", def_losses = "", error = resp.error.Value.ToString() });
					}
					else
					{
						output.Add(new { guild = param[0], points = "", delta = "", atk_wins = "", atk_losses = "", def_wins = "", def_losses = "", error = "Failed" });
					}
				}
				catch (Exception)
				{
					output.Add(new { guild = param[0], points = "", delta = "", atk_wins = "", atk_losses = "", def_wins = "", def_losses = "", error = "Failed" });
				}

			}

			int guildLen = Math.Max(output.Max(x => (x.guild ?? "").Length) ?? 0, 5) + 2;
			int pointsLen = Math.Max(output.Count() == 0 ? 0 : output.Max(x => (x.points + x.delta).Length + 3), 9) + 2;
			int atkTotalLen = 3;
			int atkLen = Math.Max(output.Count() == 0 ? 0 : output.Max(x => (x.atk_wins + x.atk_losses).Length + 3), 5) + 2;
			int defTotalLen = 3;
			int defLen = Math.Max(output.Count() == 0 ? 0 : output.Max(x => (x.def_wins + x.def_losses).Length + 3), 5) + 2;

			string msg = String.Format(":::\n{0}{1}{2}{3}\n", "Guild".PadRight(guildLen), "Points(Δ)".PadRight(pointsLen), "ATK(W-L)".PadRight(atkTotalLen + atkLen), "DEF(W-L)".PadRight(defTotalLen + defLen))
				+ String.Join("\n", output.Select(x => String.Format("{0}{1}{2}{3}",
					(String.IsNullOrEmpty(x.error) ? x.guild.PadRight(guildLen) : x.guild + " - " + x.error),
					(String.IsNullOrEmpty(x.points) ? "".PadRight(pointsLen) : String.Format("{0} ({1})", x.points, x.delta).PadRight(pointsLen)),
					(String.IsNullOrEmpty(x.atk_wins) ? "".PadRight(atkLen) : String.Format("{0}({1}-{2})", (int.Parse(x.atk_wins ?? "0") + int.Parse(x.atk_losses ?? "0")).ToString().PadRight(atkTotalLen), x.atk_wins, x.atk_losses).PadRight(atkTotalLen + atkLen)),
					(String.IsNullOrEmpty(x.def_wins) ? "".PadRight(defLen) : String.Format("{0}({1}-{2})", (int.Parse(x.def_wins ?? "0") + int.Parse(x.def_losses ?? "0")).ToString().PadRight(defTotalLen), x.def_wins, x.def_losses).PadRight(defTotalLen + defLen)))));

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
