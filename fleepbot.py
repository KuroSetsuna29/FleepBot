
import json
import logging
import pprint
import re
import webapp2
from google.appengine.api import memcache
from google.appengine.api import taskqueue
from google.appengine.api import urlfetch_errors

from commands import *
from commands import help
from constants import constants

class FleepBot(webapp2.RequestHandler):
	
	def get(self):
		self.response.headers['Content-Type'] = 'text/plain'
		
		body = self.request.body
		request = {}
		
		try:
			request = json.loads(body)
		except ValueError:
			logging.debug("body:\n" + body)
			self.response.write("body:\n" + body)
			return
		
		try:
			if 'messages' not in request:
				logging.debug("could not find messages")
				self.response.write("could not find messages\n\n")
			
			for message in request['messages']:
				conversation_id = message['conversation_id']
				account_id = message['account_id']
				messageJson = {}
				
				if (account_id.lower() == constants.ACCOUNT_ID.lower()
						or message['posted_time'] == 1453836472
					):
					return
				
				try:
					messageJson = json.loads(message['message'])
				except ValueError:
					logging.debug("message:\n" + message['message'])
					self.response.write("message:\n" + message['message'])
					return
				
				if 'message' not in messageJson:
					logging.debug("could not find inner message")
					self.response.write("could not find inner message\n\n")
				
				actualMessage = messageJson['message']
				msg = (actualMessage or "").replace("<br/>", "\r\n").replace("<msg><p>", "").replace("</p></msg>", "")
				logging.debug("message: " + msg)
				self.response.write("message: " + msg + "\n\n")
				
				# help
				if re.search(help.regex, msg, flags = re.M | re.S | re.I) is not None:
					taskqueue.add(queue_name='commands', url='/commands/help', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
					logging.debug("handled as help")
					self.response.write("handled as help\n\n")
					
				# awaken
				elif re.search(awaken.regex, msg, flags = re.M | re.S | re.I) is not None:
					taskqueue.add(queue_name='commands', url='/commands/awaken', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
					logging.debug("handled as awaken")
					self.response.write("handled as awaken\n\n")
					
				# defsetup
				elif re.search(defsetup.regex, msg, flags = re.M | re.S | re.I) is not None:
					taskqueue.add(queue_name='commands', url='/commands/defsetup', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
					logging.debug("handled as defsetup")
					self.response.write("handled as defsetup\n\n")
					
				# echo
				elif re.search(echo.regex, msg, flags = re.M | re.S | re.I) is not None:
					taskqueue.add(queue_name='commands', url='/commands/echo', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
					logging.debug("handled as echo")
					self.response.write("handled as echo\n\n")
				
				# honor
				elif re.search(honor.regex, msg, flags = re.M | re.S | re.I) is not None:
					taskqueue.add(queue_name='commands', url='/commands/honor', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
					logging.debug("handled as honor")
					self.response.write("handled as honor\n\n")
					
				# whoison
				elif re.search(whoison.regex, msg, flags = re.M | re.S | re.I) is not None:
					taskqueue.add(queue_name='commands', url='/commands/whoison', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
					logging.debug("handled as whoison")
					self.response.write("handled as whoison\n\n")
					
				else:
					logging.debug("not handled as command")
					self.response.write("not handled as command\n\n")
			
		finally:
			self.response.write("request:\n")
			self.response.write(pprint.pformat(request))
		
	
	def post(self):
		self.get()


app = webapp2.WSGIApplication([
    ('/fleepbot', FleepBot),
], debug=True)
