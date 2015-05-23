**Altis Life Playerlog** is a Project that helps server administrators catching money hackers (Altis Life).

There are several things you must do in order to start:

## SQL

1. **Download** the [SQL file](https://github.com/njahs/AL-Playerlog/blob/master/sql/).
2. **Execute** it on your Database (**Always create a backup of your database**).

## Log Viewer

Choose one of the following steps:

* **Download** the [source code](https://github.com/njahs/AL-Playerlog/tree/master/viewer/src).
  2. Compile the source code.
  3. Download [external librarys](https://github.com/njahs/AL-Playerlog/tree/master/viewer/ext).
* **Download** the [binary files](https://github.com/njahs/AL-Playerlog/tree/master/viewer/bin).
  2. Download [external librarys](https://github.com/njahs/AL-Playerlog/tree/master/viewer/ext).

##Requirements

* You need **[.NET Framework 4.5](https://www.microsoft.com/de-de/download/details.aspx?id=30653)** for the Log Viewer.
* You need a working *Altis Life* server.
* You need a **MySQL** database.
* You need **database permissisons** (to execute the SQL file).
* You need a database that allows you to **create triggers**.
* To compile: Visual Studio (recommend: 2013).

## Information

The SQL file you executed on your server inserts a new table into your database called '*players_log*'.
This table is getting filled by two triggers that handle update/insertion of any player.
**These 2 triggers cause traffic on your database, you may test it before using it.**

After logging players a while, you can simply open the Log Viewer, enter your *database credentials* and watch every player's money history / log (as of the date you've started logging).

**This can't be undone yet.**

## To-Do

* Implement a way to remove everything from the database (within the Log Viewer?).
* Implement a way to execute the SQL file automatically within the Log Viewer instead of doing this manually.

## Credits

  Credits for the Log Viewer & SQL Script: [Njahs](https://github.com/njahs)
  
