**Altis Life Playerlog** is a Project that helps server administrators catching money hackers (Altis Life).

****Project is inactive****

##Requirements

* You need **[.NET Framework 4.5](https://www.microsoft.com/de-de/download/details.aspx?id=30653)** for the Playerlog.
* You need a working *Altis Life* server.
* You need a **MySQL** database.
* You need a database that allows you to create / delete **triggers**.
* You need the following **Server Privileges** to use this tool:
  * Select
  * Delete
  * Create
  * Drop
  * Trigger
* To compile: Visual Studio (recommend: 2013).

## Installation

Choose one of the following steps:

* **Download** the [source code](https://github.com/njahs/AL-Playerlog/tree/master/src/latest%20version/).
  2. Compile the source code.
  3. Download [external librarys](https://github.com/njahs/AL-Playerlog/tree/master/ext).
* **Download** the [binary files](https://github.com/njahs/AL-Playerlog/tree/master/bin/latest%20version/).
  2. Download [external librarys](https://github.com/njahs/AL-Playerlog/tree/master/ext).

When you are finished with that:

* **Start** the Playerlog.
* **Fill in** your database details.
* **Connect** to your databse.
* **Choose** your logging mode and click continue.
* **Open** the options and click on:
  1. Create Table
  2. Create Triggers
* Now you are good to go! Your players are now being logged.

## Information / Tips

Coming soon...

## Changelog

**1.0.1**
* **Renamed the project to 'Playerlog'**
* Added: Metro Theme
* Added: Options window
  * Implemented: Add & Remove SQL Stuff (triggers, table)
  * Implemented: Trigger / Table status check
  * Implemented: Two different logging modes (actual/difference)
* Added: About window
* Added: Welcome window
  * Added: Mode-selection
* Changed: Login window
  * Added: Control labels
  * Added: Some more error messages
  * Changed: Control positions
* Changed: Control window
  * Added: About button
  * Added: Options button
* Added: Exceptions with database connection
* Added: Divison factor in the chart to prevent crashing
* Removed: Manual SQL file execution
* Removed: Many bugs

**1.0.0**
* Initial release

## Credits

  Credits for the Log Viewer & SQL Script: [Njahs](https://github.com/njahs)
  
