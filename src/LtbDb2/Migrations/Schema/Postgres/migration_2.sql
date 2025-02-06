-- add unique constraint over multiple columns
alter table book
add constraint book_number_title_category_key
unique (number, title, category);