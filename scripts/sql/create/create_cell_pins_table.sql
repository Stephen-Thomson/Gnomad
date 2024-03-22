CREATE TABLE IF NOT EXISTS `codenome_db`.`cell_pins` (
  `FRN` INT NOT NULL,
  `ProviderName` varchar(50),
  `StateAbbr` varchar(2),
  `TechCode` INT NOT NULL,
  `records` INT NOT NULL,
  `blocks` INT NOT NULL,
  PRIMARY KEY (`FRN`));
