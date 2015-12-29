using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FleepBot.Commands
{
    abstract class BaseCommand
	{
		public abstract string command_name { get; }

		protected abstract void execute(string convid, string message, string account_id);
		
		public void process(string convid, string message, string account_id)
		{
			try
			{
				if (Program.AWS_ENABLED)
				{
					Amazon.DynamoDBv2.AmazonDynamoDBClient dbclient = new Amazon.DynamoDBv2.AmazonDynamoDBClient();
					Amazon.DynamoDBv2.Model.PutItemRequest item = new Amazon.DynamoDBv2.Model.PutItemRequest
					{
						TableName = "FleepBot_SoulSeeker_CommandLog",
						Item = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>
						{
							{ "Date", new Amazon.DynamoDBv2.Model.AttributeValue { S = DateTime.Now.ToString("u") } },
							{ "ID", new Amazon.DynamoDBv2.Model.AttributeValue { S = Guid.NewGuid().ToString("N") } },
							{ "Command", new Amazon.DynamoDBv2.Model.AttributeValue { S = command_name } },
							{ "ConversationID", new Amazon.DynamoDBv2.Model.AttributeValue { S = convid } },
							{ "AccountID", new Amazon.DynamoDBv2.Model.AttributeValue { S = account_id } },
							{ "Message", new Amazon.DynamoDBv2.Model.AttributeValue { S = message } }
						}
					};

					dbclient.PutItem(item);
				}

				execute(convid, message, account_id);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Program.SendErrorMessage(convid);

				if (Program.AWS_ENABLED)
				{
					Amazon.DynamoDBv2.AmazonDynamoDBClient dbclient = new Amazon.DynamoDBv2.AmazonDynamoDBClient();
					Amazon.DynamoDBv2.Model.PutItemRequest item = new Amazon.DynamoDBv2.Model.PutItemRequest
					{
						TableName = "FleepBot_SoulSeeker_ErrorLog",
						Item = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>
						{
							{ "Date", new Amazon.DynamoDBv2.Model.AttributeValue { S = DateTime.Now.ToString("u") } },
							{ "ID", new Amazon.DynamoDBv2.Model.AttributeValue { S = Guid.NewGuid().ToString("N") } },
							{ "Command", new Amazon.DynamoDBv2.Model.AttributeValue { S = command_name } },
							{ "Message", new Amazon.DynamoDBv2.Model.AttributeValue { S = e.Message } },
							{ "StackTrace", new Amazon.DynamoDBv2.Model.AttributeValue { S = e.StackTrace } },
							{ "Data", new Amazon.DynamoDBv2.Model.AttributeValue { S =  JsonConvert.SerializeObject(e.Data) } },
							{ "Object", new Amazon.DynamoDBv2.Model.AttributeValue { S =  JsonConvert.SerializeObject(e) } }
						}
					};

					dbclient.PutItem(item);
				}
			}
		}
	}
}
