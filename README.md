# Photon Turnbased Webhooks Sample #
----------

## Summary ##

This is the **Photon Turnbased Webhooks** sample using **Azure Websites** and **Redis**.

## Requirements ##

- [Photon Developer account](https://dev-cloud.exitgames.com/)
- Windows with Internet Information Service feature enabled
- Visual Studio 2013
- [Redis for Windows](https://github.com/MSOpenTech/redis)
- [ngrok](https://ngrok.com/) to forward requests to your PC

## How to run locally ##

- Open the sample running Visual Studio as administrator and build the project (admin privileges are required because a virtual directory is used).
- Install redis and start it if it didn't start automatically. 
- Start ngrok in a command shell: "ngrok 80" and copy the url which forwards to 127.0.0.1:80.
- go to the [Photon Dashboard](https://dev-cloud.exitgames.com/en/Turnbased/Dashboard), create an application and set in the Webhooks tab the BaseUrl value: [url from ngrok]/turnbased/[your app id]/.
- run the client demo
- check the requests and responses in your browser at [127.0.0.1:4040](http://127.0.0.1:4040)

## Next Steps ##

### Publish to Azure Websites ###

- Create an [Azure Websites account](http://www.windowsazure.com/en-us/services/web-sites/), create a website, load down a publisher profile and publish the side to Azure Websites. Don't forget to change the Redis settings to point to your Redis server (you can use your local server by forwarding the corresponding port with ngrok to your PC)
- adjust the base url in the Photon Dashboard

### Modify the webhooks logic ###