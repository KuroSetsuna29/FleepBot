
import json
import logging
import math
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
	
	@staticmethod
	def RangeLetterToInt(letter):
		
		letter = letter.upper()
		length = len(letter)
		ret = 0
		
		for i in range(0, length):
			ret += (ord(letter[length - i - 1]) - (65 if i == 0 else 64)) * pow(26, i)
		
		return ret
	
	@staticmethod
	def RangeIntToLetter(x):
		
		i = x % 26
		ret = chr(i + 65)
		x = math.floor(x / 26)
		
		while x > 0:
			x -= 1
			ret = chr(int(x % 26) + 65) + ret
			x = math.floor(x / 26)
		
		return ret
	
	@staticmethod
	def GoogleSheetRange(start, end):
		
		start = start.upper()
		end = end.upper()
		
		startInt = utils.RangeLetterToInt(start)
		endInt = utils.RangeLetterToInt(end)
		
		return ', '.join(('`' + utils.RangeIntToLetter(x) + '`') for x in list(range(startInt, endInt + 1)))