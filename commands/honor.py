
import datetime
import logging
import random
import re
import time
import webapp2

from constants import constants
from fleepclient.api import FleepApi
from fleepclient.api import FleepApiBase

regex = "^\{0}honou?r(?:\s+(\+)?(.*))??$".format(constants.PREFIX)
usage = "{0}honor [+][reroll]".format(constants.PREFIX)
helpmsg = usage + " - Who is your honor of the day? You get 1 free reroll"

class honor(webapp2.RequestHandler):
	
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
		
		both = False
		reroll = False
		
		if matches.group(1) == "+":
			both = True
		if matches.group(2) in [ 'r', 'reroll' ]:
			reroll = True
		
		conv_info = self.fleep.conversation_sync(constants.CHAT_SOUL)
		members = conv_info['header']['members']
		
		m = []
		name = "Your"
		
		members_info = self.fleep.contact_sync_list(members)
		for member in members_info['contacts']:
			if member['account_id'] == account_id:
				name = member['display_name']
			elif member['account_id'] not in [
				constants.ACCOUNT_ID,
				constants.JAMES,
				constants.JENNY,
				constants.GTJ,
				constants.ANDERAN
			]:
				m.append(member['display_name'])
		
		logging.debug("members: {0}".format(m))
		
		seed = (datetime.datetime.utcnow() - datetime.timedelta(hours = 15)).date().isoformat() + account_id
		random.seed(seed)
		first = m.pop(random.randint(0, len(m) - 1))
		second = m.pop(random.randint(0, len(m) - 1))
		
		msg = ""
		if both == False:
			msg = "{0} honor of the day is *{1}*".format(name, first if reroll == False else second)
		else:
			msg = "{0} honor of the day is *{1}* and *{2}*".format(name, first, second)
		
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
    ('/commands/honor', honor),
], debug=True)
