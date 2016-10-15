using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using FleepBot.Commands;
using System.Net.Mail;

namespace FleepBot
{
	class Program
	{
		public static bool DEBUG = false;
		public static DateTime START = DateTime.Now;
		public static string COMMAND_PREFIX = ConfigurationManager.AppSettings.Get("COMMAND_PREFIX") ?? "!";
		public static string ADMIN_COMMAND_PREFIX = ConfigurationManager.AppSettings.Get("ADMIN_COMMAND_PREFIX") ?? "$";
		public static bool AWS_ENABLED = bool.Parse(ConfigurationManager.AppSettings.Get("AWS_ENABLED"));

		public static CookieContainer COOKIEJAR = new CookieContainer();
		public static Uri URI = new Uri("https://fleep.io/");
		public static string USERNAME = ConfigurationManager.AppSettings.Get("FLEEP_USERNAME");
		public static string PASSWORD = ConfigurationManager.AppSettings.Get("FLEEP_PASSWORD");
		public static string TICKET = "";
		public static string TOKEN_ID = "";
		public static string ACCOUNT_ID = "";
		public static int EVENT_HORIZON = 0;

		public static string TESTCHAT = "60366328-1f6e-4674-a520-036bf92adb1e";
		public static string SOULCHAT = "d43d3128-3db9-4147-b46c-f8efd11573d4";
		public static string SKCHAT = "c84f256a-166d-4d0b-8cd7-7c52c05a1e94";
		public static string JAMESCHAT = "fe9f439a-fba4-4169-9a9c-05473be56192";
		public static string TEDDYCHAT = "95afa794-75e1-4637-8c70-98bd276a277e";
		public static string JAMES = "37744223-d15a-4c64-a03f-73825b6a7971";
		public static string JENNY = "71c8924b-db05-4216-bc2b-5fa975c34558";
		public static string JACK = "1e0ca824-b45b-41b3-b76b-c2c663775e5f";
		public static string JON = "9a0acc6b-081a-4e44-83be-e08b7e5ed338";
		public static string ALEXA = "ff370d1c-49c8-4374-b18f-4b26dc7a7c56";
		public static string GTJ = "c7526b79-fc8c-4a45-904c-6f145ebba8e9";
		public static string ANDERAN = "d5d208e4-b3a0-4de0-95cd-c1cd812fbb9c";
		public static string JOEYBANANAS = "0fc1a0fb-9367-4f1c-b64f-20eb38f3880a";
		public static string SPOONY = "7d932b7b-1272-469c-879a-0cfee18dfb4a";
		public static string HYZNDUS = "874a2cca-3c36-41e6-bdc3-328ad361db2f";
		public static string PHILIP = "01a8b354-cc9d-4d50-a601-c7d151f90a8e";
		public static string TEDDY = "27addfdc-4555-4d32-98aa-5b996a1e7c08";
		public static List<string> ADMIN_CHATS = new List<string>() { TESTCHAT, JAMESCHAT };

		public static string HANGOUTS_JAMES = "Ugz_UcDODOqHt5O-s9p4AaABAagBrI-CBQ";
		public static string HANGOUTS_SOULCHAT = "UgyyFKOFE69CUfgfuIJ4AaABAQ";
		public static string HANGOUTS_HYZNDUS = "UgyWn-UeTToDWysUZip4AaABAQ";
		public static string HANGOUTS_PHILIP = "UgyLgH3eniaQxI-GeOF4AaABAQ";

		public static string EMAIL_TEDDY = "";

		public static List<Ban.BanTimer> BANLIST = new List<Ban.BanTimer>();
		public static List<Remind.Reminder> REMINDERS = new List<Remind.Reminder>();
		public static List<RaidRoom> RAIDS = new List<RaidRoom>();

		static void Main(string[] args)
		{
			Login();

			if (String.IsNullOrEmpty(ACCOUNT_ID))
				throw new Exception("Failed to login!");

			//RepeatMessage costume = new RepeatMessage(SOULCHAT, "It's hammer time, don't forget to upgrade your costumes.", 200);

			/*
			DateTime Base = DateTime.Now.Date + new TimeSpan(4, 0, 0);
			DateTime NextSundayStart = Base.AddDays((7 - (int)DateTime.Now.DayOfWeek) % 7);
			DateTime NextSundayEnd = NextSundayStart.AddHours(19);
			DateTime NextTuesdayStart = Base.AddDays((9 - (int)DateTime.Now.DayOfWeek) % 7);
			DateTime NextTuesdayEnd = NextTuesdayStart.AddHours(19);
			DateTime NextFridayStart = Base.AddDays((12 - (int)DateTime.Now.DayOfWeek) % 7);
			DateTime NextFridayEnd = NextFridayStart.AddHours(19);
			if (NextSundayStart < DateTime.Now) NextSundayStart = NextSundayStart.AddDays(7);
			if (NextSundayEnd < DateTime.Now) NextSundayEnd = NextSundayEnd.AddDays(7);
			if (NextTuesdayStart < DateTime.Now) NextTuesdayStart = NextTuesdayStart.AddDays(7);
			if (NextTuesdayEnd < DateTime.Now) NextTuesdayEnd = NextTuesdayEnd.AddDays(7);
			if (NextFridayStart < DateTime.Now) NextFridayStart = NextFridayStart.AddDays(7);
			if (NextFridayEnd < DateTime.Now) NextFridayEnd = NextFridayEnd.AddDays(7);
			RepeatMessage sk_gw_sunday_start = new RepeatMessage(SKCHAT, "New Guild War Started, please attack.", NextSundayStart, 10080);
			RepeatMessage sk_gw_sunday_end = new RepeatMessage(SKCHAT, "Guild War ending in 5 hours, don't forget to attack.", NextSundayEnd, 10080);
			RepeatMessage sk_gw_tuesday_start = new RepeatMessage(SKCHAT, "New Guild War Started, please attack.", NextTuesdayStart, 10080);
			RepeatMessage sk_gw_tuesday_end = new RepeatMessage(SKCHAT, "Guild War ending in 5 hours, don't forget to attack.", NextTuesdayEnd, 10080);
			RepeatMessage sk_gw_friday_start = new RepeatMessage(SKCHAT, "New Guild War Started, please attack.", NextFridayStart, 10080);
			RepeatMessage sk_gw_friday_end = new RepeatMessage(SKCHAT, "Guild War ending in 5 hours, don't forget to attack.", NextFridayEnd, 10080);
			*/

			while (!String.IsNullOrEmpty(ACCOUNT_ID))
			{
				try
				{
					GetMessage();
				}
				catch (Exception e)
				{
					Log(e);

					if (AWS_ENABLED)
					{
						Amazon.DynamoDBv2.AmazonDynamoDBClient dbclient = new Amazon.DynamoDBv2.AmazonDynamoDBClient();
						Amazon.DynamoDBv2.Model.PutItemRequest item = new Amazon.DynamoDBv2.Model.PutItemRequest
						{
							TableName = "FleepBot_SoulSeeker_ErrorLog",
							Item = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>
							{
								{ "Date", new Amazon.DynamoDBv2.Model.AttributeValue { S = DateTime.Now.ToString("u") } },
								{ "ID", new Amazon.DynamoDBv2.Model.AttributeValue { S = Guid.NewGuid().ToString("N") } },
								{ "Message", new Amazon.DynamoDBv2.Model.AttributeValue { S = e.Message } },
								{ "StackTrace", new Amazon.DynamoDBv2.Model.AttributeValue { S = e.StackTrace } },
								{ "Data", new Amazon.DynamoDBv2.Model.AttributeValue { S =  JsonConvert.SerializeObject(e.Data) } },
								{ "Object", new Amazon.DynamoDBv2.Model.AttributeValue { S =  JsonConvert.SerializeObject(e) } }
							}
						};

						dbclient.PutItem(item);
					}

					if (e.Message.Contains("Response Status Code - Unauthorized"))
					{
						Login();
					}
				}
			}
		}

		private static void Log(object obj)
		{
			string msg = String.Format("[{0}] {1}", DateTime.Now, obj);
			Console.WriteLine(msg);

			string logdir = ConfigurationManager.AppSettings.Get("LOGS");
			if (!String.IsNullOrWhiteSpace(logdir))
			{
				string logname = String.Format("FleepBot_{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));
				string logfile = Path.Combine(logdir, logname);

				File.AppendAllText(logfile, msg + "\r\n");
			}
		}

		private static void Login()
		{
			Log(String.Format("Logging in as {0}...", USERNAME));

            dynamic resp = ApiPost("api/account/login", new { email = USERNAME, password = PASSWORD });

			TICKET = resp.ticket.Value;
			TOKEN_ID = COOKIEJAR.GetCookies(URI)["TOKEN_ID"].Value;
			ACCOUNT_ID = resp.account_id.Value;

			resp = ApiPost("api/account/sync", new { ticket = TICKET });

			EVENT_HORIZON = resp.event_horizon;

			Log("TICKET: " + TICKET);
			Log("TOKEN_ID: " + TOKEN_ID);
			Log("ACCOUNT_ID: " + ACCOUNT_ID);
			Log("EVENT_HORIZON: " + EVENT_HORIZON);
			Log(String.Format("Listening for '{0}'", COMMAND_PREFIX));
		}

		private static void GetMessage()
		{
            dynamic resp = ApiPost("api/account/poll", new { wait = true, event_horizon = EVENT_HORIZON, ticket = TICKET });

			if (resp == null || resp.event_horizon == null)
				throw new Exception("Failed to poll Fleep!");

			EVENT_HORIZON = resp.event_horizon;

			foreach (dynamic stream in resp.stream)
			{
				if (stream.mk_rec_type == "message" && stream.posted_time != null)
				{
					string conversation_id = stream.conversation_id;
					string message = stream.message;
					string messageNewLines = (message ?? "").Replace("<br/>", "\r\n").Replace("<msg><p>", "").Replace("</p></msg>", "");
					string account_id = stream.account_id;

					try
					{
						DateTime time = Utils.parseUnixTimestamp(stream.edited_time != null ? stream.edited_time.Value : stream.posted_time.Value);
						if (account_id != ACCOUNT_ID
								&& stream.posted_time != 1453836472 // ignore message
							)
						{
							Log(String.Format("[{0}] {1}", time, message));

							Ban.BanTimer bt = BANLIST.FirstOrDefault(x => x.member.ToLower() == account_id.ToLower());
							if (bt != null)
							{
								SendMessage(conversation_id, String.Format("{0} has been banned for {1}", bt.name, bt.TimeLeftAsString));
								return;
							}

							if (Help.regex.IsMatch(messageNewLines))
							{
								Help command = new Help();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Echo.regex.IsMatch(messageNewLines))
							{
								Echo command = new Echo();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (WhoIsOn.regex.IsMatch(messageNewLines))
							{
								WhoIsOn command = new WhoIsOn();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (IGN.regex.IsMatch(messageNewLines))
							{
								IGN command = new IGN();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (AtkHistory.regex.IsMatch(messageNewLines))
							{
								AtkHistory command = new AtkHistory();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Teams.regex.IsMatch(messageNewLines))
							{
								Teams command = new Teams();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (HeroInfo.regex.IsMatch(messageNewLines))
							{
								HeroInfo command = new HeroInfo();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (HeroInfo7.regex.IsMatch(messageNewLines))
							{
								HeroInfo7 command = new HeroInfo7();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Awaken.regex.IsMatch(messageNewLines))
							{
								Awaken command = new Awaken();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (MyMatchUp.regex.IsMatch(messageNewLines))
							{
								MyMatchUp command = new MyMatchUp();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (GBPoints.regex.IsMatch(messageNewLines))
							{
								GBPoints command = new GBPoints();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Honor.regex.IsMatch(messageNewLines))
							{
								Honor command = new Honor();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (ListConv.regex.IsMatch(messageNewLines))
							{
								ListConv command = new ListConv();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (ListMembers.regex.IsMatch(messageNewLines))
							{
								ListMembers command = new ListMembers();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (DefSetup.regex.IsMatch(messageNewLines))
							{
								DefSetup command = new DefSetup();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Remind.regex.IsMatch(messageNewLines))
							{
								Remind command = new Remind();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (ListReminder.regex.IsMatch(messageNewLines))
							{
								ListReminder command = new ListReminder();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (RemoveReminder.regex.IsMatch(messageNewLines))
							{
								RemoveReminder command = new RemoveReminder();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Ban.regex.IsMatch(messageNewLines))
							{
								Ban command = new Ban();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (ListBan.regex.IsMatch(messageNewLines))
							{
								ListBan command = new ListBan();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Unban.regex.IsMatch(messageNewLines))
							{
								Unban command = new Unban();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Items.regex.IsMatch(messageNewLines))
							{
								Items command = new Items();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Ping.regex.IsMatch(messageNewLines))
							{
								Ping command = new Ping();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (RemindJames.regex.IsMatch(messageNewLines))
							{
								RemindJames command = new RemindJames();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Enemy7s.regex.IsMatch(messageNewLines))
							{
								Enemy7s command = new Enemy7s();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Arena7s.regex.IsMatch(messageNewLines))
							{
								Arena7s command = new Arena7s();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (RaidCreate.regex.IsMatch(messageNewLines))
							{
								RaidCreate command = new RaidCreate();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (RaidJoin.regex.IsMatch(messageNewLines))
							{
								RaidJoin command = new RaidJoin();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (RaidKick.regex.IsMatch(messageNewLines))
							{
								RaidKick command = new RaidKick();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (RaidFull.regex.IsMatch(messageNewLines))
							{
								RaidFull command = new RaidFull();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (RaidPromote.regex.IsMatch(messageNewLines))
							{
								RaidPromote command = new RaidPromote();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}
							else if (Arena7s.regex.IsMatch(messageNewLines))
							{
								Arena7s command = new Arena7s();
								new Thread(() => command.process(conversation_id, messageNewLines, account_id)).Start();
							}

							if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("PYTHON3"))
								&& !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("HANGOUTS_CMD"))
								&& account_id.ToLower() != JAMES
								&& (new Regex(String.Format("^<msg><p>.*<mention [^<>]*account_id=\"({0})\".*</p></msg>$", String.Join("|", JAMES)), RegexOptions.IgnoreCase).IsMatch(message)
									|| new Regex("kuro", RegexOptions.IgnoreCase).IsMatch(message)
									|| conversation_id.ToLower() == TEDDYCHAT))
							{
								new Thread(() =>
								{
									Regex mention = new Regex("<mention[^<>]*>(.*?)</mention>", RegexOptions.IgnoreCase);
									Regex remove = new Regex("(</?msg>|</?p>)", RegexOptions.IgnoreCase);
									string conv_name = FleepBot.Program.GetConvName(conversation_id);
									string contact_name = FleepBot.Program.GetUserName(account_id);

									string msg = String.Format("<b>{0}<br>At {1}<br>{2}</b>:<br>{3}", conv_name, time, contact_name, remove.Replace(mention.Replace(message, "$1"), ""));
									SendHangouts(HANGOUTS_JAMES, msg);
								}).Start();
							}

							if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("PYTHON3"))
								&& !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("HANGOUTS_CMD"))
								&& new Regex(String.Format("^<msg><p>.*<mention [^<>]*account_id=\"({0})\".*</p></msg>$", String.Join("|", JACK, JON, ALEXA)), RegexOptions.IgnoreCase).IsMatch(message))
							{
								new Thread(() =>
								{
									Regex mention = new Regex("<mention[^<>]*>(.*?)</mention>", RegexOptions.IgnoreCase);
									Regex remove = new Regex("(</?msg>|</?p>)", RegexOptions.IgnoreCase);
									string conv_name = FleepBot.Program.GetConvName(conversation_id);
									string contact_name = FleepBot.Program.GetUserName(account_id);

									string msg = String.Format("<b>{0}<br>At {1}<br>{2}</b>:<br>{3}", conv_name, time, contact_name, remove.Replace(mention.Replace(message, "$1"), ""));
									SendHangouts(HANGOUTS_SOULCHAT, msg);
								}).Start();
							}

							if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("PYTHON3"))
								&& !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("HANGOUTS_CMD"))
								&& new Regex(String.Format("^<msg><p>.*<mention [^<>]*account_id=\"({0})\".*</p></msg>$", String.Join("|", HYZNDUS)), RegexOptions.IgnoreCase).IsMatch(message))
							{
								new Thread(() =>
								{
									Regex mention = new Regex("<mention[^<>]*>(.*?)</mention>", RegexOptions.IgnoreCase);
									Regex remove = new Regex("(</?msg>|</?p>)", RegexOptions.IgnoreCase);
									string conv_name = FleepBot.Program.GetConvName(conversation_id);
									string contact_name = FleepBot.Program.GetUserName(account_id);

									string msg = String.Format("<b>{0}<br>At {1}<br>{2}</b>:<br>{3}", conv_name, time, contact_name, remove.Replace(mention.Replace(message, "$1"), ""));
									SendHangouts(HANGOUTS_HYZNDUS, msg);
								}).Start();
							}

							if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("PYTHON3"))
								&& !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("HANGOUTS_CMD"))
								&& new Regex(String.Format("^<msg><p>.*<mention [^<>]*account_id=\"({0})\".*</p></msg>$", String.Join("|", PHILIP)), RegexOptions.IgnoreCase).IsMatch(message))
							{
								new Thread(() =>
								{
									Regex mention = new Regex("<mention[^<>]*>(.*?)</mention>", RegexOptions.IgnoreCase);
									Regex remove = new Regex("(</?msg>|</?p>)", RegexOptions.IgnoreCase);
									string conv_name = FleepBot.Program.GetConvName(conversation_id);
									string contact_name = FleepBot.Program.GetUserName(account_id);

									string msg = String.Format("<b>{0}<br>At {1}<br>{2}</b>:<br>{3}", conv_name, time, contact_name, remove.Replace(mention.Replace(message, "$1"), ""));
									SendHangouts(HANGOUTS_PHILIP, msg);
								}).Start();
							}

							if (!String.IsNullOrEmpty(EMAIL_TEDDY)
								&& new Regex(String.Format("^<msg><p>.*<mention [^<>]*account_id=\"({0})\".*</p></msg>$", String.Join("|", TEDDY)), RegexOptions.IgnoreCase).IsMatch(message))
							{
								new Thread(() =>
								{
									Regex mention = new Regex("<mention[^<>]*>(.*?)</mention>", RegexOptions.IgnoreCase);
									Regex remove = new Regex("(</?msg>|</?p>)", RegexOptions.IgnoreCase);
									string conv_url = null;
									string conv_name = FleepBot.Program.GetConvName(conversation_id, out conv_url);
									string contact_name = null;
									List<string> msgs = new List<string>();

									dynamic sync_back = ApiPost("api/conversation/sync_backward/" + conversation_id, new { from_message_nr = stream.message_nr, ticket = TICKET });
									foreach (dynamic sync in sync_back.stream)
									{
										DateTime sync_time = Utils.parseUnixTimestamp(sync.posted_time.Value);
										if (sync_time >= time.AddMinutes(-10) && sync.message_nr.Value != stream.message_nr.Value)
										{
											contact_name = FleepBot.Program.GetUserName(sync.account_id.Value);
											msgs.Add(String.Format("[{0}] <b>{1}</b>: {2}", TimeZoneInfo.ConvertTimeBySystemTimeZoneId(sync_time, "Central European Time"), contact_name, remove.Replace(mention.Replace(message, "$1"), "")));
										}

									}

									msgs = msgs.Skip(msgs.Count > 10 ? msgs.Count - 10 : 0).ToList();

									msgs.Insert(0, String.Format("Conversation: {0}", !String.IsNullOrEmpty(conv_url) ? ("<a href='" + conv_url + "'>" + conv_name + "</a>") : (conv_name ?? "No Name")));

									contact_name = FleepBot.Program.GetUserName(account_id);
									msgs.Add(String.Format("[{0}] <br>{1}</b>: {2}", TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, "Central European Time"), contact_name, remove.Replace(mention.Replace(message, "$1"), "")));

									SendEmail(EMAIL_TEDDY, "You have been mentioned in Fleep", String.Join("<br>", msgs), true);
								}).Start();
							}
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						SendErrorMessage(conversation_id);
					}
				}
			}
		}

		public static void SendHangouts(string chatid, string message)
		{
			Log("Sending Hangout (" + chatid + "): " + message);
			System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
			start.FileName = ConfigurationManager.AppSettings.Get("PYTHON3");
			start.Arguments = string.Format(ConfigurationManager.AppSettings.Get("HANGOUTS_CMD"), chatid, message);
			start.UseShellExecute = false;
			start.RedirectStandardOutput = true;
			using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
			{
				using (StreamReader reader = process.StandardOutput)
				{
					string result = reader.ReadToEnd();
					Console.Write(result);
				}
			}
		}

		public static void SendEmail(string recipient, string subject, string body, bool isHtml)
		{
			Log("Sending Email (" + recipient + "): " + subject + "\n" + body);
			string smtp_host = ConfigurationManager.AppSettings.Get("SMTP_HOST");
			string smtp_port = ConfigurationManager.AppSettings.Get("SMTP_PORT");
			string smtp_username = ConfigurationManager.AppSettings.Get("SMTP_USERNAME");
			string smtp_password = ConfigurationManager.AppSettings.Get("SMTP_PASSWORD");

			if (!String.IsNullOrEmpty(smtp_username))
			{
				MailMessage message = new MailMessage(smtp_username, recipient, subject, body);
				message.IsBodyHtml = isHtml;

				using (var smtp = new SmtpClient())
				{
					smtp.Credentials = new NetworkCredential
					{
						UserName = smtp_username,
						Password = smtp_password
					};
					smtp.Host = smtp_host;
					smtp.Port = int.Parse(smtp_port);
					smtp.EnableSsl = true;
					smtp.Send(message);
				}
			}
		}



		public static string GetConvName(string conv_id)
		{
			string url;
			return GetConvName(conv_id, out url);
		}

		public static string GetConvName(string conv_id, out string url)
		{
			long? sync_horizon = 0;
			bool found = false;
			string name = null;
			url = null;
			dynamic conversations = new { };
			do
			{
				dynamic list = FleepBot.Program.ApiPost("api/conversation/list", new { sync_horizon = sync_horizon, ticket = FleepBot.Program.TICKET });
				conversations = list.conversations;
				sync_horizon = list.sync_horizon;

				foreach (dynamic conversation in conversations)
				{
					if (conversation.conversation_id != null && conversation.conversation_id.Value.ToLower() == conv_id.ToLower())
					{
						name = (conversation.topic != null && !String.IsNullOrEmpty(conversation.topic.Value) ? conversation.topic : conversation.default_topic) ?? "No Name Set";
						url = conversation.autojoin_url;

						found = true;
						break;
					}
				}

			} while (!found && conversations != null && conversations.HasValues);

			return name;
		}

		public static string GetUserName(string account_id)
		{
			dynamic memberinfo = FleepBot.Program.ApiPost("api/contact/sync", new { contact_id = account_id, ticket = TICKET });
			return memberinfo.contact_name ?? memberinfo.display_name;
		}

		static void ActivityIndicator(Regex r, string convid, string message)
		{
			string input = r.Match(message).Groups[1].Value;

			bool status = input == "on";

			SetActivityIndicator(convid, status);
		}

		public static void SetActivityIndicator(string convid, bool status)
		{
			Log("Set Activity: " + status);
			ApiPost("api/conversation/show_activity/" + convid, new { is_writing = status ? true : false, ticket = TICKET });
		}

		public static void SendMessage(string convid, string message)
		{
			if (message.StartsWith("/"))
			{
				Log("No slash commands allowed: " + message);
				SendErrorMessage(convid, "No slash commands allowed.");
			}

			Log("Sending: " + message);
			dynamic resp = ApiPost("api/message/send/" + convid, new { message = message, ticket = TICKET });
		}

		public static void SendErrorMessage(string convid, string message = "Error: Something unexpected happened.")
		{
			Log("Sending: " + message);
			ApiPost("api/message/send/" + convid, new { message = message, ticket = TICKET });
		}

		public static Tuple<List<dynamic>, List<dynamic>> GetGoogleSheet(string convid, string docid, string sheetid, string query, int? headers = null, string range = null)
		{
			string resp = Get(String.Format("https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tq={2}{1}{3}{4}",
					docid,
					String.IsNullOrEmpty(sheetid) ? "" : "&gid=" + sheetid,
					Uri.EscapeUriString(query),
					headers != null ? "&headers=" + headers : "",
					String.IsNullOrEmpty(range) ? "" : "&range=" + range));
			Regex rgx = new Regex("^.+?(\\{.*\\}).*$", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
			if (!rgx.IsMatch(resp))
			{
				SendErrorMessage(convid);
				return null;
			}

			dynamic obj = JsonConvert.DeserializeObject(rgx.Match(resp).Groups[1].Value);
			if (obj.status == "error")
			{
				SendErrorMessage(convid);
				return null;
			}

			return Tuple.Create(obj.table.cols.ToObject<List<dynamic>>(), obj.table.rows.ToObject<List<dynamic>>());
		}

		public static string Get(string url)
		{
			WebRequest request = WebRequest.Create(url);

			using (WebResponse response = request.GetResponse())
			{
				Stream dataStream = response.GetResponseStream();
				using (StreamReader reader = new StreamReader(dataStream))
				{
					string resp = reader.ReadToEnd();
					if (DEBUG)
						Log(resp);

					return resp;
				}
			}
		}

		public static dynamic GetAsJson(string url)
		{
			WebRequest request = WebRequest.Create(url);

			using (WebResponse response = request.GetResponse())
			{
				Stream dataStream = response.GetResponseStream();
				using (StreamReader reader = new StreamReader(dataStream))
				{
					string resp = reader.ReadToEnd();
					if (DEBUG)
						Log(resp);

					return JsonConvert.DeserializeObject(resp);
				}
			}
		}

		public static dynamic ApiPost(string api, object data)
		{
			dynamic ret = new { };

			HttpClientHandler handler = new HttpClientHandler()
			{
				CookieContainer = COOKIEJAR,
				UseCookies = true
			};
			using (var client = new HttpClient(handler))
			{
				client.BaseAddress = URI;
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Task<HttpResponseMessage> t = client.PostAsJsonAsync(api, data);
                t.Wait(-1);

                HttpResponseMessage response = t.Result;
				if (response.IsSuccessStatusCode)
                {
                    Task<dynamic> t2 = response.Content.ReadAsAsync<dynamic>();
                    t2.Wait(-1);

                    dynamic resp = t2.Result;
					if (DEBUG)
						Log(resp);

					ret = resp;
				}
				else
				{
					Log("Error: Response Status Code - " + response.StatusCode);
				}
			}

			return ret;
		}
	}
}
