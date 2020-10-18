# LiveDirectorySyncEngine
Windows service to backup files as soon they change or added to a folder.

I started this just as a project to get some experience with filewatchter in c# plus as I thought it could be a helpfull utility to keep my son's minecraft server backup up to dated. 
It however also ended in some learning experience.

## .net core worker service (October 2020)
To get more familiar with the .net core worker service I decided to create a .net core worker service instead of my original .net framework 4.61 worker service.
I started by watching a youtube video of Tim Corey (https://www.youtube.com/watch?v=PzrTiz_NRKA)

It appeared to be handy to watch a video on some core concepts as well to learn how to use the appsettings file and how the logging works. Also from Tim Corey. (https://www.youtube.com/watch?v=GAOCe-2nXqc)

Settings is needed as I wanted a setting to point to the settings file which is configured via the LiveDirectorySyncEngineConsoleApp.
Logging is something I wanted to know about to know how to use my own "logging" classes. After some experiments and also investigating about scopes (https://www.codeproject.com/Articles/1556475/How-to-write-a-custom-logging-provider-in-Asp-Net) I have to revisit how I do logging. 
The static library does not fully fit in the concept of how logging in .net core is done.
Also wondering how scopes work in combination with async. But first refactor logging.

