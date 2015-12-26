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

namespace FleepBot
{
	class Program
	{
		public static bool DEBUG = false;
		public static DateTime START = DateTime.Now;
		public static string COMMAND_PREFIX = ConfigurationManager.AppSettings.Get("COMMAND_PREFIX") ?? "!";
		public static string ADMIN_COMMAND_PREFIX = ConfigurationManager.AppSettings.Get("ADMIN_COMMAND_PREFIX") ?? "$";

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
		public static string JAMES = "37744223-d15a-4c64-a03f-73825b6a7971";
		public static string JENNY = "71c8924b-db05-4216-bc2b-5fa975c34558";
		public static string JACK = "1e0ca824-b45b-41b3-b76b-c2c663775e5f";

		public static List<Ban.BanTimer> BANLIST = new List<Ban.BanTimer>();
		public static List<Remind.Reminder> REMINDERS = new List<Remind.Reminder>();

		static void Main(string[] args)
		{
			Login();

			if (String.IsNullOrEmpty(ACCOUNT_ID))
				throw new Exception("Failed to login!");

			//RepeatMessage costume = new RepeatMessage(SOULCHAT, "It's hammer time, don't forget to upgrade your costumes.", 200);

			while (!String.IsNullOrEmpty(ACCOUNT_ID))
			{
				try
				{
					GetMessage();
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
		}

		private static void Login()
		{
			Console.WriteLine(String.Format("Logging in as {0}...", USERNAME));

            dynamic resp = ApiPost("api/account/login", new { email = USERNAME, password = PASSWORD });

			TICKET = resp.ticket.Value;
			TOKEN_ID = COOKIEJAR.GetCookies(URI)["TOKEN_ID"].Value;
			ACCOUNT_ID = resp.account_id.Value;

			Console.WriteLine("TICKET: " + TICKET);
			Console.WriteLine("TOKEN_ID: " + TOKEN_ID);
			Console.WriteLine("ACCOUNT_ID: " + ACCOUNT_ID);
			Console.WriteLine(String.Format("Listening for '{0}'", COMMAND_PREFIX));
		}

		private static void GetMessage()
		{
            dynamic resp = ApiPost("api/account/poll", new { wait = true, event_horizon = EVENT_HORIZON, ticket = TICKET });

			EVENT_HORIZON = resp.event_horizon;

			foreach (dynamic stream in resp.stream)
			{
				if (stream.mk_rec_type == "message" && stream.posted_time != null)
				{
					string conversation_id = stream.conversation_id;
					string message = stream.message;
					string account_id = stream.account_id;

					try
					{
						DateTime time = Utils.parseUnixTimestamp(stream.posted_time.Value);
						if (time >= START && account_id != ACCOUNT_ID)
						{
							Console.WriteLine(String.Format("[{0}] {1}", time, message));

							Ban.BanTimer bt = BANLIST.FirstOrDefault(x => x.member.ToLower() == account_id.ToLower());
							if (bt != null)
							{
								SendMessage(conversation_id, String.Format("{0} has been banned for {1}", bt.name, bt.TimeLeftAsString));
								return;
							}

							if (Help.regex.IsMatch(message))
							{
								new Thread(() => Help.execute(conversation_id)).Start();
							}
							else if (Echo.regex.IsMatch(message))
							{
								new Thread(() => Echo.execute(conversation_id, message)).Start();
							}
							else if (WhoIsOn.regex.IsMatch(message))
							{
								new Thread(() => WhoIsOn.execute(conversation_id, message)).Start();
							}
							else if (IGN.regex.IsMatch(message))
							{
								new Thread(() => IGN.execute(conversation_id, message, account_id)).Start();
							}
							else if (AtkHistory.regex.IsMatch(message))
							{
								new Thread(() => AtkHistory.execute(conversation_id, message, account_id)).Start();
							}
							else if (Teams.regex.IsMatch(message))
							{
								new Thread(() => Teams.execute(conversation_id, message)).Start();
							}
							else if (HeroInfo.regex.IsMatch(message))
							{
								new Thread(() => HeroInfo.execute(conversation_id, message)).Start();
							}
							else if (HeroInfo7.regex.IsMatch(message))
							{
								new Thread(() => HeroInfo7.execute(conversation_id, message)).Start();
							}
							else if (Awaken.regex.IsMatch(message))
							{
								new Thread(() => Awaken.execute(conversation_id, message)).Start();
							}
							else if (MyMatchUp.regex.IsMatch(message))
							{
								new Thread(() => MyMatchUp.execute(conversation_id, message, account_id)).Start();
							}
							else if (GBPoints.regex.IsMatch(message))
							{
								new Thread(() => GBPoints.execute(conversation_id, message)).Start();
							}
							else if (Honor.regex.IsMatch(message))
							{
								new Thread(() => Honor.execute(conversation_id, message, account_id)).Start();
							}
							else if (ListConv.regex.IsMatch(message))
							{
								new Thread(() => ListConv.execute(conversation_id, message)).Start();
							}
							else if (ListMembers.regex.IsMatch(message))
							{
								new Thread(() => ListMembers.execute(conversation_id, message)).Start();
							}
							else if (DefSetup.regex.IsMatch(message))
							{
								new Thread(() => DefSetup.execute(conversation_id, message)).Start();
							}
							else if (Remind.regex.IsMatch(message))
							{
								new Thread(() => Remind.execute(conversation_id, message, account_id)).Start();
							}
							else if (ListReminder.regex.IsMatch(message))
							{
								new Thread(() => ListReminder.execute(conversation_id, message)).Start();
							}
							else if (RemoveReminder.regex.IsMatch(message))
							{
								new Thread(() => RemoveReminder.execute(conversation_id, message)).Start();
							}
							else if (Ban.regex.IsMatch(message))
							{
								new Thread(() => Ban.execute(conversation_id, message)).Start();
							}
							else if (ListBan.regex.IsMatch(message))
							{
								new Thread(() => ListBan.execute(conversation_id, message)).Start();
							}
							else if (Unban.regex.IsMatch(message))
							{
								new Thread(() => Unban.execute(conversation_id, message)).Start();
							}
							else if (Items.regex.IsMatch(message))
							{
								new Thread(() => Items.execute(conversation_id, message)).Start();
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

		static void ActivityIndicator(Regex r, string convid, string message)
		{
			string input = r.Match(message).Groups[1].Value;

			bool status = input == "on";

			SetActivityIndicator(convid, status);
		}

		public static void SetActivityIndicator(string convid, bool status)
		{
			Console.WriteLine("Set Activity: " + status);
			ApiPost("api/conversation/show_activity/" + convid, new { is_writing = status ? true : false, ticket = TICKET });
		}

		public static void SendMessage(string convid, string message)
		{
			if (message.StartsWith("/"))
			{
				Console.WriteLine("No slash commands allowed: " + message);
				SendErrorMessage(convid, "No slash commands allowed.");
			}

            Console.WriteLine("Sending: " + message);
			dynamic resp = ApiPost("api/message/send/" + convid, new { message = message, ticket = TICKET });
		}

		public static void SendErrorMessage(string convid, string message = "Error: Something unexpected happened.")
		{
			Console.WriteLine("Sending: " + message);
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
						Console.WriteLine(resp);

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
						Console.WriteLine(resp);

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
						Console.WriteLine(resp);

					ret = resp;
				}
				else
				{
					Console.WriteLine("Error: Response Status Code - " + response.StatusCode);
				}
			}

			return ret;
		}
	}
}
