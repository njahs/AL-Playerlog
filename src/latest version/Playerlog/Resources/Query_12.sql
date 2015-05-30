DROP TRIGGER IF EXISTS players_log_update;
DROP TRIGGER IF EXISTS playerlog_update;
DROP TRIGGER IF EXISTS plog_update;
CREATE TRIGGER playerlog_update AFTER UPDATE ON players
FOR EACH ROW
	BEGIN
		INSERT INTO players_log SET uid = old.uid, logtype = 'update', 
		cash = new.cash, bank = new.bankacc,
		totalcash = new.cash + new.bankacc;
	END;