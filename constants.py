from google.appengine.api.app_identity import get_application_id

class constants:
	
	# Configuration
	DEBUG = True if get_application_id() == "fleepbotstaging" else False
	PREFIX = "%" if DEBUG else "/"
	PREFIX_ADMIN = "%" if DEBUG else "/"
	
	# Fleep Config
	FLEEP_HOST = "https://fleep.io"
	FLEEP_USERNAME = ""
	FLEEP_PASSWORD = ""
	
	# Fleep Conversation IDs
	CHAT_TEST = "60366328-1f6e-4674-a520-036bf92adb1e"
	CHAT_SOUL = "d43d3128-3db9-4147-b46c-f8efd11573d4"
	CHAT_SK = "c84f256a-166d-4d0b-8cd7-7c52c05a1e94"
	CHAT_JAMES = "fe9f439a-fba4-4169-9a9c-05473be56192"
	CHAT_TEDDY = "95afa794-75e1-4637-8c70-98bd276a277e"
	
	ADMIN_CHATS = [ CHAT_TEST, CHAT_JAMES ]
	
	# Fleep Member IDs
	ACCOUNT_ID = "deb263ef-34a7-4637-a87b-906320bef924"
	
	JAMES = "37744223-d15a-4c64-a03f-73825b6a7971"
	JENNY = "71c8924b-db05-4216-bc2b-5fa975c34558"
	JACK = "1e0ca824-b45b-41b3-b76b-c2c663775e5f"
	JON = "9a0acc6b-081a-4e44-83be-e08b7e5ed338"
	ALEXA = "ff370d1c-49c8-4374-b18f-4b26dc7a7c56"
	GTJ = "c7526b79-fc8c-4a45-904c-6f145ebba8e9"
	ANDERAN = "d5d208e4-b3a0-4de0-95cd-c1cd812fbb9c"
	JOEYBANANAS = "0fc1a0fb-9367-4f1c-b64f-20eb38f3880a"
	SPOONY = "7d932b7b-1272-469c-879a-0cfee18dfb4a"
