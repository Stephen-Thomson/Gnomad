CREATE TABLE IF NOT EXISTS `codenome_db`.`users` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(320) NULL,
  `profile_photo_url` VARCHAR(512) NULL,
  `first_name` VARCHAR(45) NULL,
  `last_name` VARCHAR(45) NULL,
  PRIMARY KEY (`id`));