CREATE TABLE IF NOT EXISTS `codenome_db`.`pins` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `longitude` INT NOT NULL,
  `latitude` INT NOT NULL,
  `title` VARCHAR(45) NULL,
  `street` VARCHAR(45) NULL,
  `up_vote` INT NULL DEFAULT 0,
  `down_vote` INT DEFAULT 0,
  PRIMARY KEY (`id`));
