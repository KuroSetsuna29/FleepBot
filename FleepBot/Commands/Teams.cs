using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class Teams
	{
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}teams(?:\\s+(.+))?</p></msg>$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase);

		public static async Task execute(string convid, string message)
		{
			string search = regex.Match(message).Groups[1].Value;

			string query = "select *";
			if (!String.IsNullOrEmpty(search))
				query = String.Format("select * where lower(A) like '%{0}%' or lower(B) like '%{0}%' or lower(C) like '%{0}%' or lower(D) like '%{0}%' or lower(E) like '%{0}%' or lower(F) like '%{0}%'", search.ToLower());

			Tuple<List<dynamic>, List<dynamic>> stats = FleepBot.Program.GetGoogleSheet(convid, "1m58UndKk59AZCp3RCg_I7clK-xam76MyWvoGtU7HPNE", "1204815824", query, 1);

			if (stats == null)
			{
				return;
			}

			if (!stats.Item1.Any(c => c.id.Value == "A" && c.label.Value == "Team")
				|| !stats.Item1.Any(c => c.id.Value == "B" && c.label.Value == "Hero1")
				|| !stats.Item1.Any(c => c.id.Value == "C" && c.label.Value == "Hero2")
				|| !stats.Item1.Any(c => c.id.Value == "D" && c.label.Value == "Hero3")
				|| !stats.Item1.Any(c => c.id.Value == "E" && c.label.Value == "Hero4")
				|| !stats.Item1.Any(c => c.id.Value == "F" && c.label.Value == "Hero5"))
			{
				await FleepBot.Program.SendErrorMessage(convid);
				return;
			}

			string msg = String.Format("No team found for '{0}'. Check spelling.", search);
			if (stats.Item2.Count > 0)
			{
				msg = String.Join("\n", stats.Item2.Select(x => String.Format("{0} - {1}{2}{3}{4}{5}",
						(x.c[0].v.Value.ToLower().Contains(search.ToLower()) && !String.IsNullOrEmpty(search) ? "*" + x.c[0].v.Value + "*" : x.c[0].v.Value),
						String.IsNullOrEmpty(x.c[1].v.Value) ? "" : (x.c[1].v.Value.ToLower().Contains(search.ToLower()) && !String.IsNullOrEmpty(search) ? "*" + x.c[1].v.Value + "*" : x.c[1].v.Value),
						String.IsNullOrEmpty(x.c[2].v.Value) ? "" : ", " + (x.c[2].v.Value.ToLower().Contains(search.ToLower()) && !String.IsNullOrEmpty(search) ? "*" + x.c[2].v.Value + "*" : x.c[2].v.Value),
						String.IsNullOrEmpty(x.c[3].v.Value) ? "" : ", " + (x.c[3].v.Value.ToLower().Contains(search.ToLower()) && !String.IsNullOrEmpty(search) ? "*" + x.c[3].v.Value + "*" : x.c[3].v.Value),
						String.IsNullOrEmpty(x.c[4].v.Value) ? "" : ", " + (x.c[4].v.Value.ToLower().Contains(search.ToLower()) && !String.IsNullOrEmpty(search) ? "*" + x.c[4].v.Value + "*" : x.c[4].v.Value),
						String.IsNullOrEmpty(x.c[5].v.Value) ? "" : ", " + (x.c[5].v.Value.ToLower().Contains(search.ToLower()) && !String.IsNullOrEmpty(search) ? "*" + x.c[5].v.Value + "*" : x.c[5].v.Value))));
			}

			await FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
