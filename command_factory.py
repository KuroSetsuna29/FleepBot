
import datetime
import logging
import re
from google.appengine.api import taskqueue

from commands import *
from commands import help
from constants import constants

class command_factory:
	
	@staticmethod
	def process(conversation_id, account_id, message):
		
		msg = (message or "").replace("<br/>", "\r\n").replace("<msg><p>", "").replace("</p></msg>", "")
		logging.debug("message: " + msg)
		
		# help
		if re.search(help.regex, msg, flags = re.M | re.S | re.I) is not None:
			taskqueue.add(queue_name='commands', url='/commands/help', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
			logging.debug("handled as help")
			
		# echo
		elif re.search(echo.regex, msg, flags = re.M | re.S | re.I) is not None:
			taskqueue.add(queue_name='commands', url='/commands/echo', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
			logging.debug("handled as echo")
		
		# honor
		if re.search(honor.regex, msg, flags = re.M | re.S | re.I) is not None:
			taskqueue.add(queue_name='commands', url='/commands/honor', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
			logging.debug("handled as honor")
			
		# whoison
		elif re.search(whoison.regex, msg, flags = re.M | re.S | re.I) is not None:
			taskqueue.add(queue_name='commands', url='/commands/whoison', params={'conv_id': conversation_id, 'account_id': account_id, 'message': msg})
			logging.debug("handled as whoison")
			
		else:
			logging.debug("not handled as command")
