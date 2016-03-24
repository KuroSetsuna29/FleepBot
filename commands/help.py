
import logging
import re
import webapp2

from constants import constants
from fleepclient.api import FleepApi
from fleepclient.api import FleepApiBase

regex = "^\{0}help(?:\s+(.+))?$".format(constants.PREFIX)
usage = "{0}help _[filter]_".format(constants.PREFIX)
helpmsg = usage + " - Show this help message"

class help(webapp2.RequestHandler):
	
	fleep = None
	conv_id = ""
	message = ""
	
	def get(self):
		self.fleep = FleepApi(constants.FLEEP_HOST)
		self.fleep.account_login(constants.FLEEP_USERNAME, constants.FLEEP_PASSWORD)
		
		from commands import *
		self.response.headers['Content-Type'] = 'text/plain'
		
		conv_id = self.request.get('conv_id')
		account_id = self.request.get('account_id')
		message = self.request.get('message')
		
		self.conv_id = conv_id
		self.message = message
		
		matches = re.search(regex, message, flags=re.M | re.S | re.I)
		
		filter = "";
		if matches.group(1) is not None:
			filter = matches.group(1)
		
		cmds = [
			help.helpmsg,
			echo.helpmsg,
			honor.helpmsg,
			whoison.helpmsg
		]
		
		msg = "\n".join(cmds)
		
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
    ('/commands/help', help),
], debug=True)
