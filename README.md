# LiveDirectorySyncEngine
Windows service to backup files as soon they change or added to a folder.

I'm creating this to just to get some experience with filewatchter in c# plus to have an utility to keep my son's minecraft server backup up to dated.

# NDatabase
As the computer running the minecraft server might be disconnected from the NAS used for the backup a kind of queuing mechanism is needed.
In this queu a log of changes will be kept to be processed later.
I found a library called NDatabase which seems usable
Original source: http://ndatabase.codeplex.com/documentation
Copy on https://github.com/spolnik/ndatabase
Unfortunately not really maintaned but lucky open source :-)
