# Stats-Database

Connects to a local or remote(still untested) MS-SQL database to display NBA
stats. The stats are provided in a .csv file, which are programatically 
inserted into the database. This requires the user to already have an MS-SQL
server running. The stats are then displayed in a table after searching for
a player's name. Names can be partial matches.

This is for learning puposes and has little value beyond looking at
publicly available NBA statistics. They are from the 2018-2019 season.
Some players are listed twice due to them playing on two teams in a season
(traded mid-season). This might be something that I change in the future,
but the point of the project is to learn T-SQL and Windows Forms, so for
now, they will stay.


This has only been tested on Ubuntu 18.0 using Mono.

compile: csc -out:NBA_DB.exe *.cs
or try:	 mcs -pkg:dotnet -out:NBA_DB.exe *.cs

run:     mono NBA_DB.exe
