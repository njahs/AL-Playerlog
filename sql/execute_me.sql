SET FOREIGN_KEY_CHECKS=0;

-- Create log table if not exists
CREATE TABLE IF NOT EXISTS `players_log` (
  `uid` int(11) NOT NULL,
  `updatetime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `logtype` enum('update','insert') NOT NULL,
  `cashdiff` int(100) NOT NULL,
  `bankdiff` int(100) NOT NULL,
  `totalcash` int(100) NOT NULL,
  PRIMARY KEY (`uid`,`updatetime`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Drop trigger if exists
DROP TRIGGER IF EXISTS players_log_insert;

-- Create insert trigger
CREATE TRIGGER players_log_insert BEFORE INSERT ON players
FOR EACH ROW
	BEGIN
			INSERT INTO players_log SET uid = new.uid, logtype = 'insert', 
			cashdiff = new.cash, bankdiff = new.bankacc, totalcash = new.cash + new.bankacc;
	END;

-- Drop trigger if exists
DROP TRIGGER IF EXISTS players_log_update;

-- Create update trigger
CREATE TRIGGER players_log_update AFTER UPDATE ON players
FOR EACH ROW
	BEGIN
		INSERT INTO players_log SET uid = old.uid, logtype = 'update', 
		cashdiff = new.cash - old.cash, bankdiff = new.bankacc - old.bankacc,
		totalcash = new.cash + new.bankacc;
	END;