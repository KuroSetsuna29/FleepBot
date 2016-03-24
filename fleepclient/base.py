"""Python Library for Fleep HTTP API.
"""

import urllib
import re
import json
import logging
import pprint

from google.appengine.api import memcache
from google.appengine.api import urlfetch
from google.appengine.api import urlfetch_errors

class FleepApiBase(object):
	"""Base class for HTTP API's
	"""

	def __init__(self, base_url):
		"""Initiates session
		"""
		self.base_url = base_url.rstrip('/')  #: URL of web server providing API
		self.cookies = ''
		self.code = None        #: result code of latest request
		self.expect = 200       #: if set expect this code from service and raise error if
								#: something else comes
		self.ticket = None
		self.requests = set()   #: requests processed by server

	def _webapi_call(self, function, *args, **kwargs):
		logging.debug('-' * 60)

		url = '/'.join((self.base_url, function) + args)
		hdr = {'Content-Type': 'application/json; charset=utf-8', 'Connection': 'Keep-Alive', 'Cookie': self.cookies}
		if self.ticket:
			kwargs['ticket'] = self.ticket
			kwargs['api_version'] = 2

		data = json.dumps(kwargs)

		logging.debug('HEADER: %s', hdr)
		logging.debug('REQUEST: %s', url)
		logging.debug("PARAMS: %s", data)

		r = {}
		try:
			r = urlfetch.fetch(url = url, payload = data, method = urlfetch.POST, headers = hdr, deadline=60)
		except urlfetch_errors.InternalTransientError:
			r = urlfetch.fetch(url = url, payload = data, method = urlfetch.POST, headers = hdr, deadline=60)

		if r.content and r.content[0] in ('{', '['):
			res = json.loads(r.content)
		else:
			res = {}
		logging.debug('STATUS_CODE %s', r.status_code)
		logging.debug("RESULT:\n%s", pprint.pformat(res, 4))

		if self.expect is not None and r.status_code != self.expect:
			raise Exception("%s: Expect %d, got: %d" % (url, self.expect, r.status_code))

		self.code = r.status_code
		cookies = r.headers.get('set-cookie')
		if cookies is not None:
			self.cookies = cookies
		return res

	def _file_call(self, function, **kwargs):
		raise Exception("Not Supported")

		url = self.base_url + '/' + function
		files = kwargs.get('files')
		if files:
			del kwargs['files']
		logging.debug('REQUEST: %s', url)

		r = {'text': ''}
		#r = self.ws.post(url, params = {'ticket' : self.ticket}, files = files, verify = True)
		if r.text and r.text[0] in ('{', '['):
			res = json.loads(r.text)
		else:
			res = {}
		logging.debug('STATUS_CODE %s', r.status_code)
		logging.debug("RESULT:\n%s", pprint.pformat(res, 4))

		if self.expect is not None and r.status_code != self.expect:
			raise Exception("%s: Expect %d, got: %d %s" % (url, self.expect, r.status_code, r.raw.reason))

		self.code = r.status_code
		return res

	def get_token(self):
		"""Get session token
		"""
		matches = re.search("\btoken_id=([^;]*);?", self.cookies, flags = re.M | re.S | re.I)
		if matches is None:
			return None
		return matches.group(1)

	def get_ticket(self):
		"""Get ticket
		"""
		return self.ticket

	def set_token(self, token = None, ticket = None):
		"""Set session token
		"""
		if token is not None:
			self.cookies = '; '.join(self.cookies, 'token_id=' + token)
		if ticket is not None:
			self.ticket = ticket




