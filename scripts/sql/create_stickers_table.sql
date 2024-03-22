CREATE TABLE IF NOT EXISTS `codenome_db`.`stickers` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NULL,
  `longitude` INT NULL,
  `latitude` INT NULL,
  `title` VARCHAR(45) NULL,
  PRIMARY KEY (`id`));
