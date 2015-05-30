CREATE TABLE IF NOT EXISTS `players_log` (
  `uid` int(11) NOT NULL,
  `updatetime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `logtype` enum('update','insert') NOT NULL,
  `cash` int(100) NOT NULL,
  `bank` int(100) NOT NULL,
  `totalcash` int(100) NOT NULL,
  PRIMARY KEY (`uid`,`updatetime`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;