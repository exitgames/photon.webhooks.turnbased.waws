# Photon Turnbased Webhooks Sample

## Summary

This is the **Photon Turnbased Webhooks** sample using [Azure Websites](http://www.windowsazure.com/en-us/services/storage/) and [Blob Storage]() and [Table Storage](http://www.windowsazure.com/en-us/services/storage/) for persitence.


## Requirements

- [Photon Account](https://www.photonengine.com)
- Windows with IIS (Internet Information Service) feature enabled
- Visual Studio 2013 or up
- [ngrok](https://ngrok.com/) to forward requests to your PC


### Option 1: Azure Storage

- Azure Account: It is free (but you need to enter a Credit Card)
- Azure Storage: Create a "STORAGE" > Quick Create (enter Name and Region)


### Option 2: Redis
- local [Redis](http://redis.io/download)


## Set this project up for free now! Check out all the possibilities of Microsoft Azure!

Sign up for your free trial month of Microsoft Azure now and get USD 200 / EURO 150 to spend on all(!) Azure services you like to try out – without any further obligation!  
You’ll get the full power from the Cloud and you can choose yourself, how to spend your balance!

**[http://aka.ms/exitgames_azure](http://aka.ms/exitgames_azure)**
 
For authentification purposes you’ll need a credit card to sign up.
Without a credit card request a free Microsoft Azure Pass simply by sending a short note to [azurenow@microsoft.com](azurenow@microsoft.com).
Use the Microsoft Azure Pass to sign up at [www.windowsazurepass.com](www.windowsazurepass.com) and you’ll also discover the cloud power of Microsoft Azure for free!


## Run it locally

- Open the sample running Visual Studio as administrator and build the project (admin privileges are required because a virtual directory is used).
- *Option 1: Azure Storage*, web.config `<add key="DataAccess" value="Azure"/>`
  - Select the Azure Storage > Manage Access Keys (copy paste into the config)
  - set `<add key="AzureAccountName" value="" />`
  - set `<add key="AzureAccountKey" value="" />`
- *Option 2: local Redis*, web.config `<add key="DataAccess" value="Redis"/>`
  - set `<add key="RedisPassword" value=""/>`
  - set `<add key="RedisUrl" value="127.0.0.1"/>`
  - Start local redis server
- Start ngrok in a command shell: `ngrok http 80` and copy the url which forwards to 127.0.0.1:80.
- go to the [Photon Dashboard](https://www.photonengine.com/Dashboard), create an application and set in the Webhooks tab the BaseUrl value: `[url from ngrok]/turnbased/[your app id]/`.
- run the client demo
- check the requests and responses in your browser at [127.0.0.1:4040](http://127.0.0.1:4040)


## Next Steps ##

### Publish to Azure Websites ###

- Create an [Microsoft Azure Websites account](http://aka.ms/exitgames_azure), create a website, load down a publisher profile and publish the side to Azure Websites. Don't forget to change the Redis settings to point to your Redis server (you can use your local server by forwarding the corresponding port with ngrok to your PC)
- adjust the base url in the Photon Dashboard


### Modify the webhooks logic ###
