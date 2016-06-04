
import logging
import re
import webapp2


from constants import constants
from fleepclient.api import FleepApi
from utils import utils

regex = "^\{0}awaken?(?:\s+(.+))?$".format(constants.PREFIX)
usage = "{0}awaken _Hero_ [, _Hero2_ ]".format(constants.PREFIX)
helpmsg = usage + " - Show awaken mats required for heroes"

class awaken(webapp2.RequestHandler):
	
	fleep = None
	conv_id = ""
	message = ""
	
	def get(self):
		self.fleep = FleepApi(constants.FLEEP_HOST)
		self.fleep.account_login(constants.FLEEP_USERNAME, constants.FLEEP_PASSWORD)
		
		self.response.headers['Content-Type'] = 'text/plain'
		
		conv_id = self.request.get('conv_id')
		account_id = self.request.get('account_id')
		message = self.request.get('message')
		
		self.conv_id = conv_id
		self.message = message
		
		matches = re.search(regex, message, flags=re.M | re.S | re.I)
		
		if matches is None:
			self.fleep.message_send(conv_id, "Error: Please specify a hero. Example: {0}".format(usage))
			return
		
		heroes = re.split("[, ]", matches.group(1).lower())
		
		sets = utils.GetGoogleSheet("1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", "select B, C, D, E", 0, "B18:E50")
		mats = utils.GetGoogleSheet("1w-w0GPyvXt-Aouqkhh5CkGUSsI0m3kevuhq9vZ83lQE", "763917879", "select B, C, D, E, F, G, H, I, J", 0, "B3:J6")
		
		heroes1 = []
		heroes2 = []
		heroes3 = []
		heroes4 = []
		
		for row in sets.get('table').get('rows'):
			if row.get('c')[0] is not None and row.get('c')[0].get('v') is not None and any(s in row.get('c')[0].get('v').lower() for s in heroes):
				heroes1.append(row.get('c')[0].get('v'))
			if row.get('c')[1] is not None and row.get('c')[1].get('v') is not None and any(s in row.get('c')[1].get('v').lower() for s in heroes):
				heroes2.append(row.get('c')[1].get('v'))
			if row.get('c')[2] is not None and row.get('c')[2].get('v') is not None and any(s in row.get('c')[2].get('v').lower() for s in heroes):
				heroes3.append(row.get('c')[2].get('v'))
			if row.get('c')[3] is not None and row.get('c')[3].get('v') is not None and any(s in row.get('c')[3].get('v').lower() for s in heroes):
				heroes4.append(row.get('c')[3].get('v'))
		
		msg = "";
		
		if len(heroes1) > 0:
			msg += ":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}\n:::\n".format(
				", ".join(heroes1),
				mats.get('table').get('rows')[0].get('c')[0].get('v'),
				mats.get('table').get('rows')[0].get('c')[1].get('v'),
				mats.get('table').get('rows')[0].get('c')[2].get('v'),
				mats.get('table').get('rows')[0].get('c')[3].get('v'),
				mats.get('table').get('rows')[0].get('c')[4].get('v'),
				mats.get('table').get('rows')[0].get('c')[5].get('v'),
				mats.get('table').get('rows')[0].get('c')[6].get('v'),
				mats.get('table').get('rows')[0].get('c')[7].get('v'),
				mats.get('table').get('rows')[0].get('c')[8].get('v')
			)
		if len(heroes2) > 0:
			msg += ":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}\n:::\n".format(
				", ".join(heroes2),
				mats.get('table').get('rows')[1].get('c')[0].get('v'),
				mats.get('table').get('rows')[1].get('c')[1].get('v'),
				mats.get('table').get('rows')[1].get('c')[2].get('v'),
				mats.get('table').get('rows')[1].get('c')[3].get('v'),
				mats.get('table').get('rows')[1].get('c')[4].get('v'),
				mats.get('table').get('rows')[1].get('c')[5].get('v'),
				mats.get('table').get('rows')[1].get('c')[6].get('v'),
				mats.get('table').get('rows')[1].get('c')[7].get('v'),
				mats.get('table').get('rows')[1].get('c')[8].get('v')
			)
		if len(heroes3) > 0:
			msg += ":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}\n:::\n".format(
				", ".join(heroes3),
				mats.get('table').get('rows')[2].get('c')[0].get('v'),
				mats.get('table').get('rows')[2].get('c')[1].get('v'),
				mats.get('table').get('rows')[2].get('c')[2].get('v'),
				mats.get('table').get('rows')[2].get('c')[3].get('v'),
				mats.get('table').get('rows')[2].get('c')[4].get('v'),
				mats.get('table').get('rows')[2].get('c')[5].get('v'),
				mats.get('table').get('rows')[2].get('c')[6].get('v'),
				mats.get('table').get('rows')[2].get('c')[7].get('v'),
				mats.get('table').get('rows')[2].get('c')[8].get('v')
			)
		if len(heroes4) > 0:
			msg += ":::\nMats required to awaken '{0}':\n- {1}\n- {2}\n- {3}\n- {4}\n- {5}\n- {6}\nDungeons:\n- {7}\n- {8}\n- {9}\n:::\n".format(
				", ".join(heroes4),
				mats.get('table').get('rows')[3].get('c')[0].get('v'),
				mats.get('table').get('rows')[3].get('c')[1].get('v'),
				mats.get('table').get('rows')[3].get('c')[2].get('v'),
				mats.get('table').get('rows')[3].get('c')[3].get('v'),
				mats.get('table').get('rows')[3].get('c')[4].get('v'),
				mats.get('table').get('rows')[3].get('c')[5].get('v'),
				mats.get('table').get('rows')[3].get('c')[6].get('v'),
				mats.get('table').get('rows')[3].get('c')[7].get('v'),
				mats.get('table').get('rows')[3].get('c')[8].get('v')
			)
		
		if len(heroes1) + len(heroes2) + len(heroes3) + len(heroes4) == 0:
				msg = "No awaken info found for '{0}'".format(matches.group(1))
		
		self.fleep.message_send(conv_id, msg)
		self.response.write(msg)
	
	def post(self):
		self.get()
	
	def handle_exception(self, exception, mode):
		if self.fleep is not None:
			self.fleep.message_send(self.conv_id, "Error: Something unexpected happened.")
			self.fleep.message_send(constants.CHAT_JAMES, "An error occurred when processing: {0}\n:::\n{1}".format(self.message, exception))
		
		# run the default exception handling
		webapp2.RequestHandler.handle_exception(self, exception, mode)


app = webapp2.WSGIApplication([
    ('/commands/awaken', awaken),
], debug=True)
