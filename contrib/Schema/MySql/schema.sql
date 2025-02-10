-- schema table
CREATE TABLE `schema` (
  `version` BIGINT(20) NOT NULL,
  `applied_on` DATETIME NOT NULL,
  `description` VARCHAR(1024) COLLATE utf8mb4_unicode_ci NOT NULL,
  KEY `ux_schema_version` (`version`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

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
  KEY `ix_category` (`category`),
  UNIQUE `uq_number_title_category` (`number`, `title`, `category`)
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

-- insert schema version
INSERT INTO `schema` 
  (`version`, `applied_on`, `description`) VALUE
  (1, NOW(), 'Create schema.'),
  (2, NOW(), 'Add unique key for number, title and category.');
