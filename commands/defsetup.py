
import logging
import re
import webapp2
from functools import reduce

from constants import constants
from fleepclient.api import FleepApi
from utils import utils

regex = "^(?:\{0}|!)defsetup?(?:\s+(.+))?$".format(constants.PREFIX)
usage = "{0}defsetup _Member_ [, _Member2_ ]".format(constants.PREFIX)
helpmsg = usage + " - Compare guild member defense setups"

class defsetup(webapp2.RequestHandler):
	
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
		search = matches.group(1)
		
		if search is None:
			self.fleep.message_send(conv_id, "Error: Please specify a guild member. Example: {0}".format(usage))
			self.response.write("Error: Please specify a guild member. Example: {0}".format(usage))
			return
		
		members = re.split("[, ]", search.lower())
		
		query = "select {0} where lower({1}) matches '.*({2}).*'".format(utils.GoogleSheetRange("A", "CV"), "A", "|".join(members))
		stats = utils.GetGoogleSheet("1Ge91fmZEbNNLzAc4LwoeIqN8w5pdYMwbxgNuAjd25Pk", "720812518", query, 1, "A1:CZ50")
		
		msg = "No member(s) found for '{0}'. Check spelling.".format(search)
		if (len(stats.get('table').get('rows')) == 0):
			self.fleep.message_send(conv_id, msg)
			self.response.write(msg)
			return
		
		col_name = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('name')
		col_hero1a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero1a')
		col_upgrade1a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade1a')
		col_skill1a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill1a')
		col_2ndskill1a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill1a')
		col_hero2a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero2a')
		col_upgrade2a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade2a')
		col_skill2a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill2a')
		col_2ndskill2a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill2a')
		col_hero3a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero3a')
		col_upgrade3a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade3a')
		col_skill3a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill3a')
		col_2ndskill3a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill3a')
		col_hero4a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero4a')
		col_upgrade4a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade4a')
		col_skill4a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill4a')
		col_2ndskill4a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill4a')
		col_hero5a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero5a')
		col_upgrade5a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade5a')
		col_skill5a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill5a')
		col_2ndskill5a = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill5a')
		col_hero1b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero1b')
		col_upgrade1b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade1b')
		col_skill1b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill1b')
		col_2ndskill1b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill1b')
		col_hero2b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero2b')
		col_upgrade2b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade2b')
		col_skill2b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill2b')
		col_2ndskill2b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill2b')
		col_hero3b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero3b')
		col_upgrade3b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade3b')
		col_skill3b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill3b')
		col_2ndskill3b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill3b')
		col_hero4b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero4b')
		col_upgrade4b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade4b')
		col_skill4b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill4b')
		col_2ndskill4b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill4b')
		col_hero5b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('hero5b')
		col_upgrade5b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('upgrade5b')
		col_skill5b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('shortskill5b')
		col_2ndskill5b = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('short2ndskill5b')
		col_pet1 = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('pet 1')
		col_petstar1 = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('pet star 1')
		col_pet2 = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('pet 2')
		col_petstar2 = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('pet star 2')
		col_transcend1 = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('lvl extreme 1')
		col_transcend2 = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('lvl extreme 2')
		col_7skill = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('7*skill')
		col_heroquality = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('heroquality')
		col_rating = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('rating')
		col_rank = list(map((lambda x: x.get('label').lower()), stats.get('table').get('cols'))).index('rank')
		
		len_name = max(4, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_name]))), stats.get('table').get('rows'))), 0)) + 2
		len_hero1 = max(4, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero1a])) + len(self.vget(x.get('c')[col_upgrade1a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero1b])) + len(self.vget(x.get('c')[col_upgrade1b]))), stats.get('table').get('rows'))), 0)) + 3
		len_skill1 = max(5, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill1a])) + len(self.vget(x.get('c')[col_2ndskill1a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill1b])) + len(self.vget(x.get('c')[col_2ndskill1b]))), stats.get('table').get('rows'))), 0)) + 3
		len_hero2 = max(4, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero2a])) + len(self.vget(x.get('c')[col_upgrade2a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero2b])) + len(self.vget(x.get('c')[col_upgrade2b]))), stats.get('table').get('rows'))), 0)) + 3
		len_skill2 = max(5, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill2a])) + len(self.vget(x.get('c')[col_2ndskill2a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill2b])) + len(self.vget(x.get('c')[col_2ndskill2b]))), stats.get('table').get('rows'))), 0)) + 3
		len_hero3 = max(4, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero3a])) + len(self.vget(x.get('c')[col_upgrade3a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero3b])) + len(self.vget(x.get('c')[col_upgrade3b]))), stats.get('table').get('rows'))), 0)) + 3
		len_skill3 = max(5, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill3a])) + len(self.vget(x.get('c')[col_2ndskill3a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill3b])) + len(self.vget(x.get('c')[col_2ndskill3b]))), stats.get('table').get('rows'))), 0)) + 3
		len_hero4 = max(4, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero4a])) + len(self.vget(x.get('c')[col_upgrade4a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero4b])) + len(self.vget(x.get('c')[col_upgrade4b]))), stats.get('table').get('rows'))), 0)) + 3
		len_skill4 = max(5, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill4a])) + len(self.vget(x.get('c')[col_2ndskill4a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill4b])) + len(self.vget(x.get('c')[col_2ndskill4b]))), stats.get('table').get('rows'))), 0)) + 3
		len_hero5 = max(4, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero5a])) + len(self.vget(x.get('c')[col_upgrade5a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_hero5b])) + len(self.vget(x.get('c')[col_upgrade5b]))), stats.get('table').get('rows'))), 0)) + 3
		len_skill5 = max(5, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill5a])) + len(self.vget(x.get('c')[col_2ndskill5a]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_skill5b])) + len(self.vget(x.get('c')[col_2ndskill5b]))), stats.get('table').get('rows'))), 0)) + 3
		len_pet = max(3, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_petstar1])) + len(self.vget(x.get('c')[col_pet1]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_petstar2])) + len(self.vget(x.get('c')[col_pet2]))), stats.get('table').get('rows'))), 0)) + 4
		len_transcend = max(6, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_transcend1]))), stats.get('table').get('rows'))), 0), reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_transcend2]))), stats.get('table').get('rows'))), 0)) + 2
		len_skill7s = max(7, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_7skill]))), stats.get('table').get('rows'))), 0)) + 2
		len_heroquality = max(11, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_heroquality]))), stats.get('table').get('rows'))), 0)) + 2
		len_rating = max(6, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_rating]))), stats.get('table').get('rows'))), 0)) + 2
		len_rank = max(4, reduce(max, list(map((lambda x: len(self.vget(x.get('c')[col_rank]))), stats.get('table').get('rows'))), 0))
		
		msg = ":::\n{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}\n".format(
			"Name".ljust(len_name),
			"Hero1".ljust(len_hero1),
			"Skill1".ljust(len_skill1),
			"Hero2".ljust(len_hero2),
			"Skill2".ljust(len_skill2),
			"Hero3".ljust(len_hero3),
			"Skill3".ljust(len_skill3),
			"Hero4".ljust(len_hero4),
			"Skill4".ljust(len_skill4),
			"Hero5".ljust(len_hero5),
			"Skill5".ljust(len_skill5),
			"Pet".ljust(len_pet),
			"Transc".ljust(len_transcend),
			"7*Skill".ljust(len_skill7s),
			"HeroQuality".ljust(len_heroquality),
			"Rating".ljust(len_rating),
			"Rank".ljust(len_rank))
		
		msg += "\n".join("{0}\n{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}\n{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}".format(
			"-".ljust(len_name + len_hero1 + len_skill1 + len_hero2 + len_skill2 + len_hero3 + len_skill3 + len_hero4 + len_skill4 + len_hero5 + len_skill5 + len_pet + len_transcend + len_skill7s + len_heroquality + len_rating + len_rank, "-"),
			self.vget(x.get('c')[col_name]).ljust(len_name),
			(self.vget(x.get('c')[col_hero1a]) + "+" + self.vget(x.get('c')[col_upgrade1a])).ljust(len_hero1),
			(self.vget(x.get('c')[col_skill1a]) + " " + self.vget(x.get('c')[col_2ndskill1a])).ljust(len_skill1),
			(self.vget(x.get('c')[col_hero2a]) + "+" + self.vget(x.get('c')[col_upgrade2a])).ljust(len_hero2),
			(self.vget(x.get('c')[col_skill2a]) + " " + self.vget(x.get('c')[col_2ndskill2a])).ljust(len_skill2),
			(self.vget(x.get('c')[col_hero3a]) + "+" + self.vget(x.get('c')[col_upgrade3a])).ljust(len_hero3),
			(self.vget(x.get('c')[col_skill3a]) + " " + self.vget(x.get('c')[col_2ndskill3a])).ljust(len_skill3),
			(self.vget(x.get('c')[col_hero4a]) + "+" + self.vget(x.get('c')[col_upgrade4a])).ljust(len_hero4),
			(self.vget(x.get('c')[col_skill4a]) + " " + self.vget(x.get('c')[col_2ndskill4a])).ljust(len_skill4),
			(self.vget(x.get('c')[col_hero5a]) + "+" + self.vget(x.get('c')[col_upgrade5a])).ljust(len_hero5),
			(self.vget(x.get('c')[col_skill5a]) + " " + self.vget(x.get('c')[col_2ndskill5a])).ljust(len_skill5),
			(self.vget(x.get('c')[col_petstar1]) + "* " + self.vget(x.get('c')[col_pet1])).ljust(len_pet),
			self.vget(x.get('c')[col_transcend1]).ljust(len_transcend),
			self.vget(x.get('c')[col_7skill]).ljust(len_skill7s),
			self.vget(x.get('c')[col_heroquality]).ljust(len_heroquality),
			self.vget(x.get('c')[col_rating]).ljust(len_rating),
			self.vget(x.get('c')[col_rank]).ljust(len_rank),
			"".ljust(len_name),
			(self.vget(x.get('c')[col_hero1b]) + "+" + self.vget(x.get('c')[col_upgrade1b])).ljust(len_hero1),
			(self.vget(x.get('c')[col_skill1b]) + " " + self.vget(x.get('c')[col_2ndskill1b])).ljust(len_skill1),
			(self.vget(x.get('c')[col_hero2b]) + "+" + self.vget(x.get('c')[col_upgrade2b])).ljust(len_hero2),
			(self.vget(x.get('c')[col_skill2b]) + " " + self.vget(x.get('c')[col_2ndskill2b])).ljust(len_skill2),
			(self.vget(x.get('c')[col_hero3b]) + "+" + self.vget(x.get('c')[col_upgrade3b])).ljust(len_hero3),
			(self.vget(x.get('c')[col_skill3b]) + " " + self.vget(x.get('c')[col_2ndskill3b])).ljust(len_skill3),
			(self.vget(x.get('c')[col_hero4b]) + "+" + self.vget(x.get('c')[col_upgrade4b])).ljust(len_hero4),
			(self.vget(x.get('c')[col_skill4b]) + " " + self.vget(x.get('c')[col_2ndskill4b])).ljust(len_skill4),
			(self.vget(x.get('c')[col_hero5b]) + "+" + self.vget(x.get('c')[col_upgrade5b])).ljust(len_hero5),
			(self.vget(x.get('c')[col_skill5b]) + " " + self.vget(x.get('c')[col_2ndskill5b])).ljust(len_skill5),
			(self.vget(x.get('c')[col_petstar2]) + "* " + self.vget(x.get('c')[col_pet2])).ljust(len_pet),
			self.vget(x.get('c')[col_transcend2]).ljust(len_transcend)) for x in stats.get('table').get('rows'))
		
		self.fleep.message_send(conv_id, msg)
		self.response.write(msg)
	
	def vget(self, x):
		if not x:
			return ""
		elif x.get('f'):
			return x.get('f').strip()
		else:
			return x.get('v').strip()
	
	def post(self):
		self.get()
	
	def handle_exception(self, exception, mode):
		if self.fleep is not None:
			self.fleep.message_send(self.conv_id, "Error: Something unexpected happened.\nPlease excuse me while I clean up aisle 3, in the meantime you can visit http://bit.ly/defsetup")
			self.fleep.message_send(constants.CHAT_JAMES, "An error occurred when processing: {0}\n:::\n{1}".format(self.message, exception))
		
		# run the default exception handling
		webapp2.RequestHandler.handle_exception(self, exception, mode)


app = webapp2.WSGIApplication([
    ('/commands/defsetup', defsetup),
], debug=True)
