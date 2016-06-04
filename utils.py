
import json
import logging
import re
import urllib2

class utils:
	@staticmethod
	def GetGoogleSheet(docid, sheetid, query, headers = None, range = None):
		
		url = "https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tq={2}{1}{3}{4}".format(
			docid,
			"&gid=" + str(sheetid) if sheetid is not None and sheetid != "" else "",
		    urllib2.quote(query),
		    "&headers=" + str(headers) if headers is not None and headers != "" else "",
			"&range=" + str(range) if range is not None and range != "" else ""
		)
		logging.debug("URL: {0}".format(url))
		response = urllib2.urlopen(url)
		
		match = re.search("^.*?(\{.*\}).*$", response.read(), flags = re.MULTILINE | re.DOTALL | re.IGNORECASE)
		
		if match is None:
			logging.debug(response)
			raise Exception('unable to find json from google sheet response')
		
		return json.loads(match.group(1))
