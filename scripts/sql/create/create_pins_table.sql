CREATE TABLE IF NOT EXISTS `codenome_db`.`pins` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `longitude` INT NOT NULL,
  `latitude` INT NOT NULL,
  `title` VARCHAR(45) NULL,
  `street` VARCHAR(45) NULL,
  PRIMARY KEY (`id`));
