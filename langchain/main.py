from langchain.chat_models import AzureChatOpenAI
from langchain.schema import HumanMessage
from azure.identity import ClientSecretCredential
import sys
import json
import os

config = json.load(open(sys.argv[1]))

# Set the scope to the Azure resource to which access is needed
scope = f'api://{config["client_id"]}/.default'

# Create a credential object
credential = ClientSecretCredential(
       tenant_id=config["tenant_id"],
       client_id=config["client_id"],
       client_secret=config["client_secret"],
   )

# Set the API type to `azure_ad`
os.environ["OPENAI_API_TYPE"] = "azure_ad"

# Set the API_KEY to the token from the Azure credential
os.environ["OPENAI_API_KEY"] = credential.get_token(scope).token

# Create the LLM object
chat = AzureChatOpenAI(
    openai_api_base= config["openai_endpoint"],
    openai_api_version=config["openai_api_version"],
    model_name=config["model_name"],
    deployment_name=config["deployment_name"],
    max_tokens=config["max_tokens"],
    temperature=config["temperature"]
)

print(chat([HumanMessage(content="What is the city where the James Bond movie \"No Time to Die\" starts ?")]))