using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class Awaken : BaseCommand
	{
		public override string command_name { get { return "Awaken"; } }
		public static Regex regex = new Regex(String.Format("^\\{0}awaken?(?:\\s+(.+))?$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;

			if (String.IsNullOrWhiteSpace(search))
			{
				FleepBot.Program.SendErrorMessage(convid, String.Format("Error: Please specify a hero. Example: {0}heroinfo _Hero1_ [, _Hero2_ ]", FleepBot.Program.COMMAND_PREFIX));
				return;
			}

			List<string> heroes = search.Split(',', ' ').Select(x => x.ToLower().Trim()).ToList();
			string query = "select B, C, D, E";
			Tuple<List<dynamic>, List<dynamic>> sets = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", query, 0, "B18:E50");

			query = "select B, C, D, E, F, G, H, I, J";
			Tuple<List<dynamic>, List<dynamic>> mats = FleepBot.Program.GetGoogleSheet(convid, "1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", query, 0, "B3:J6");


			if (sets == null || query == null)
			{
				FleepBot.Program.SendErrorMessage(convid);
				return;
            }

			string msg = "";
			List<string> heroes1 = sets.Item2.Select<dynamic, string>(y => y.c[0] != null && y.c[0].v != null ? y.c[0].v.Value : "").Where(x => heroes.Any(y => x.ToLower().Contains(y))).ToList();
			List<string> heroes2 = sets.Item2.Select<dynamic, string>(y => y.c[1] != null && y.c[1].v != null ? y.c[1].v.Value : "").Where(x => heroes.Any(y => x.ToLower().Contains(y))).ToList();
			List<string> heroes3 = sets.Item2.Select<dynamic, string>(y => y.c[2] != null && y.c[2].v != null ? y.c[2].v.Value : "").Where(x => heroes.Any(y => x.ToLower().Contains(y))).ToList();
			List<string> heroes4 = sets.Item2.Select<dynamic, string>(y => y.c[3] != null && y.c[3].v != null ? y.c[3].v.Value : "").Where(x => heroes.Any(y => x.ToLower().Contains(y))).ToList();

			if (heroes1.Count > 0)
			{
				msg += String.Format(":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}\n:::\n",
					String.Join(", ", heroes1),
					mats.Item2[0].c[0].v.Value,
					mats.Item2[0].c[1].v.Value,
					mats.Item2[0].c[2].v.Value,
					mats.Item2[0].c[3].v.Value,
					mats.Item2[0].c[4].v.Value,
					mats.Item2[0].c[5].v.Value,
					mats.Item2[0].c[6].v.Value,
					mats.Item2[0].c[7].v.Value,
					mats.Item2[0].c[8].v.Value);
			}
			if (heroes2.Count > 0)
			{
				msg += String.Format(":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}\n:::\n",
                    String.Join(", ", heroes2),
					mats.Item2[1].c[0].v.Value,
					mats.Item2[1].c[1].v.Value,
					mats.Item2[1].c[2].v.Value,
					mats.Item2[1].c[3].v.Value,
					mats.Item2[1].c[4].v.Value,
					mats.Item2[1].c[5].v.Value,
					mats.Item2[1].c[6].v.Value,
					mats.Item2[1].c[7].v.Value,
					mats.Item2[1].c[8].v.Value);
			}
			if (heroes3.Count > 0)
			{
				msg += String.Format(":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}\n:::\n",
					String.Join(", ", heroes3),
					mats.Item2[2].c[0].v.Value,
					mats.Item2[2].c[1].v.Value,
					mats.Item2[2].c[2].v.Value,
					mats.Item2[2].c[3].v.Value,
					mats.Item2[2].c[4].v.Value,
					mats.Item2[2].c[5].v.Value,
					mats.Item2[2].c[6].v.Value,
					mats.Item2[2].c[7].v.Value,
					mats.Item2[2].c[8].v.Value);
			}
			if (heroes4.Count > 0)
			{
				msg += String.Format(":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}\n:::\n",
					String.Join(", ", heroes4),
					mats.Item2[3].c[0].v.Value,
					mats.Item2[3].c[1].v.Value,
					mats.Item2[3].c[2].v.Value,
					mats.Item2[3].c[3].v.Value,
					mats.Item2[3].c[4].v.Value,
					mats.Item2[3].c[5].v.Value,
					mats.Item2[3].c[6].v.Value,
					mats.Item2[3].c[7].v.Value,
					mats.Item2[3].c[8].v.Value);
			}

			if (heroes1.Count + heroes2.Count + heroes3.Count + heroes4.Count == 0)
			{
				msg = String.Format("No awaken info found for '{0}'.", search);
			}

			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
