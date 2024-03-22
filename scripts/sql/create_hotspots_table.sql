CREATE TABLE IF NOT EXISTS `codenome_db`.`hotspots` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `longitude` INT NULL,
  `latitude` INT NULL,
  `place` VARCHAR(45) NULL,
  `carrier` VARCHAR(45) NULL,
  PRIMARY KEY (`id`));
