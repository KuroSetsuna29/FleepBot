
import logging
import math
import re
import time
import webapp2

from constants import constants
from fleepclient.api import FleepApi
from fleepclient.api import FleepApiBase

regex = "^\{0}whoison(?:\s+(.+))?$".format(constants.PREFIX)
usage = "{0}whoison _[Hours]_".format(constants.PREFIX)
helpmsg = usage + " - Show who was on Fleep in the past _Hours_ hours (optional), default 1 hour"

class whoison(webapp2.RequestHandler):
	
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
		
		hours = 1.0;
		try:
			if matches.group(1) is not None:
				hours = float(matches.group(1))
		except ValueError:
			self.fleep.message_send(conv_id, "Error: Please specify time in hours. Example: {0}".format(usage))
			return
		
		conv_info = self.fleep.conversation_sync(conv_id)
		members = conv_info['header']['members']
		
		m = {}
		
		members_info = self.fleep.contact_sync_list(members)
		for member in members_info['contacts']:
			if member['account_id'] != constants.ACCOUNT_ID and member.has_key('activity_time'):
				relative_time = time.time() - float(member['activity_time'])
				
				if relative_time <= hours * 3600:
					m[member['display_name']] = { 'relative_time': relative_time, 'msg': secondsToOutput(member['display_name'], relative_time) }
		
		logging.debug("members: {0}".format(m))
		sorted_m = sorted(m.items(), key = lambda x: x[1]['relative_time'], reverse = True)
		
		msg = ":::\nLast seen on Fleep...\n{0}".format("\n".join(map(lambda x: x[1]['msg'], sorted_m)))
		
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


def secondsToOutput(name, seconds):
	if seconds >= 3600:
		return "{0} more than {1} hr{2} ago".format(name, max(int(math.floor(seconds / 3600)), 0), "s" if math.floor(seconds / 3600) != 1 else "")
	else:
		return "{0} {1} min{2} ago".format(name, max(int(math.floor(seconds / 60)), 0), "s" if math.floor(seconds / 60) != 1 else "")


app = webapp2.WSGIApplication([
    ('/commands/whoison', whoison),
], debug=True)
