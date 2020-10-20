# LiveDirectorySyncEngine
Windows service to backup files as soon they change or added to a folder.

I started this just as a project to get some experience with filewatchter in c# plus as I thought it could be a helpful utility to keep my son's minecraft server backup up to dated. 
It however also ended in some learning experience.

## .net core worker service (October 2020)
To get more familiar with the .net core worker service I decided to create a .net core worker service instead of my original .net framework 4.61 worker service.
I started by watching a youtube video of Tim Corey (https://www.youtube.com/watch?v=PzrTiz_NRKA)

It appeared to be handy to watch a video on some core concepts as well to learn how to use the appsettings file and how the logging works. Also from Tim Corey. (https://www.youtube.com/watch?v=GAOCe-2nXqc)

Settings is needed as I wanted a setting to point to the settings file which is configured via the LiveDirectorySyncEngineConsoleApp. 
I still have the idea that I somehow want a UI for changing settings and then trigger refresh of the settings of the worker. Not sure how yet. 
So for now I read settings from same place.

Logging is something I wanted to know about more to know how to fit it with my current basic logging system.
After reading some more articles:
- Scopes: https://www.codeproject.com/Articles/1556475/How-to-write-a-custom-logging-provider-in-Asp-Net)
- Logging in my business logic: https://stackify.com/net-core-loggerfactory-use-correctly/#:~:text=Basics%20of%20the%20.NET%20Core%20Logging%20With%20LoggerFactory.,be%20able%20to%20send%20your%20application%20logging%20anywhere. 
- Microsoft Documentation: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1 
My conclusion is that I don't want to pass logging objects all around my code. But it might be interesting to route some logs from the core libraries to my own logging classes as well.
This means I need some "proxy" in for of logger or factory following the .net core logging factory pattern.
Also the concept of splitting the text from parameters in the logs could be an interesting approach. In my day to day job we don't do this which makes it harder to find the number of times a certain log entry exists in the log.