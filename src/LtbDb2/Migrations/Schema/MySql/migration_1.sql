-- book table
CREATE TABLE `book` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `number` INT(11) NOT NULL,
  `title` VARCHAR(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `category` VARCHAR(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `created` DATETIME NOT NULL,
  `modified` DATETIME NOT NULL,
  `filename` VARCHAR(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_category` (`category`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- story table
CREATE TABLE `story` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `item_order` INT(11) NOT NULL DEFAULT 0,
  `bookid` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_name` (`name`),
  KEY `ix_bookid` (`bookid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- tag table
CREATE TABLE `tag` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `bookid` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ix_name` (`name`),
  KEY `ix_bookid` (`bookid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;