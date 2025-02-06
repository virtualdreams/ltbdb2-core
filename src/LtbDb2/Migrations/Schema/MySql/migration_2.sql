-- add unique constraint over multiple columns
ALTER TABLE `book`
ADD UNIQUE `uq_number_title_category` 
(`number`, `title`, `category`);