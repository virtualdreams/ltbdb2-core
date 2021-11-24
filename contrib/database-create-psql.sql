-- book table
create table book (
  id serial primary key,
  number int not null, 
  title varchar(200) not null,
  category varchar(100) not null,
  created timestamptz not null,
  modified timestamptz not null,
  filename varchar(100) default null
);

-- story table
create table story (
  id serial primary key,
  name varchar(200) not null,
  bookid int not null
);

-- tag table
create table tag (
  id serial primary key,
  name varchar(50) not null,
  bookid int not null
);

-- create indexes
create index ix_book_category on book(category);
create index ix_story_name on story(name);
create index ix_story_bookid on story(bookid);
create index ix_tag_name on tag(name);
create index ix_tag_bookid on tag(bookid);