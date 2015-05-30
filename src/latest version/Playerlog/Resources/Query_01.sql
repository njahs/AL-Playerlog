DROP TRIGGER IF EXISTS players_log_insert;
DROP TRIGGER IF EXISTS playerlog_insert;
DROP TRIGGER IF EXISTS plog_insert;
CREATE TRIGGER playerlog_insert BEFORE INSERT ON players
FOR EACH ROW
	BEGIN
			INSERT INTO players_log SET uid = new.uid, logtype = 'insert', 
			cash = new.cash, bank = new.bankacc, totalcash = new.cash + new.bankacc;
	END;