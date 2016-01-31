using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    class Help : BaseCommand
	{
		public override string command_name { get { return "Help"; } }
		public static Regex regex = new Regex(String.Format("^<msg><p>\\{0}help(?:\\s+(.*))?</p></msg>$", FleepBot.Program.COMMAND_PREFIX), RegexOptions.IgnoreCase);

		protected override void execute(string convid, string message, string account_id)
		{
			string search = regex.Match(message).Groups[1].Value;

			List<string> commands = new List<string>()
			{
				FleepBot.Program.COMMAND_PREFIX + "help - Show this message",
				FleepBot.Program.COMMAND_PREFIX + "atkhistory _GuildName_ - List previous attack results against _GuildName_",
				FleepBot.Program.COMMAND_PREFIX + "awaken _Hero_ - Show awaken mats required for hero",
				FleepBot.Program.COMMAND_PREFIX + "defsetup _Member1_ [, _Member2_ ][, _Member3_ ] - Compare defense setups",
				FleepBot.Program.COMMAND_PREFIX + "echo _Message_ - Repeat your _Message_",
				FleepBot.Program.COMMAND_PREFIX + "gbpoints _GuildName_ _Points_ - Submit _Points_ for _GuildName_ during Guild Battle. Hint: Use new line for multiple guilds",
				FleepBot.Program.COMMAND_PREFIX + "heroinfo _Hero1_ [, _Hero2_ ] - Show stats for each hero",
				FleepBot.Program.COMMAND_PREFIX + "heroinfo7 _Hero1_ [, _Hero2_ ] - Show stats for each 7* hero",
				FleepBot.Program.COMMAND_PREFIX + "honor [reroll] - Who is your honor of the day? You get 1 free reroll",
				FleepBot.Program.COMMAND_PREFIX + "ign [ _InGameName_ ] - Set your IGN, passing empty _InGameName_ will display your currently set name",
				FleepBot.Program.COMMAND_PREFIX + "items _Item1_ [, _Item2_ ] - Show stats for each item",
				FleepBot.Program.COMMAND_PREFIX + "myatkhistory - DEPRECATED. Please use " + FleepBot.Program.COMMAND_PREFIX + "mymatchup instead",
				FleepBot.Program.COMMAND_PREFIX + "mymatchup [ _GuildName1_ | _Opponent1_ ] [, _GuildName2_ | _Opponent2_ ] - Show matchup score, optionally search for a list of opponents or guilds",
				FleepBot.Program.COMMAND_PREFIX + "raidcreate _Room_ _Pass_ _Role_ - Create a new raid room.",
				FleepBot.Program.COMMAND_PREFIX + "raidfull _Room_ - Indicate room is full",
				FleepBot.Program.COMMAND_PREFIX + "raidjoin [ _Room_ ] _Role_ - Find a room to join as _Role_. Enter _Room_ to join specific room",
				FleepBot.Program.COMMAND_PREFIX + "raidkick _Room_ _Role_ - Clear _Role_ as open spot",
				FleepBot.Program.COMMAND_PREFIX + "remind [Xd][Xh][Xm][Xs] _Message_ - Repeat _Message_ after delay specified",
				FleepBot.Program.COMMAND_PREFIX + "teams [ _Search_ ] - Search all teams for _Search_, leave blank to list all teams",
				FleepBot.Program.COMMAND_PREFIX + "whoison [ _X_ ] - Show who was on Fleep in the past X hours (optional), default 1 hour",
			};
			
			if (!String.IsNullOrWhiteSpace(search))
			{
				Regex r = new Regex(search, RegexOptions.IgnoreCase);
                commands = commands.Where(c => r.IsMatch(c)).ToList();
			}

			string msg = String.Join("\n", commands);
			FleepBot.Program.SendMessage(convid, msg);
		}
	}
}
