# Database

## PostgreSQL

Create user.

```sql
create user ltbdb with password 'password';
```

Create database.

```sql
create database ltbdb with owner ltbdb encoding 'UTF8' lc_collate = 'de_DE.UTF-8' lc_ctype = 'de_DE.UTF-8' template template0;
```

Import schema.

```sh
psql -U ltbdb -h localhost -d ltbdb < contrib/Schema/Postgres/schema.sql 
```

## MariaDB

Create user.

```sql
create user 'ltbdb'@'localhost' identified by 'password';
grant all on notes.* to 'ltbdb'@'localhost';
```

Create database.

```sql
create database ltbdb;
```

Import schema.

```sh
mysql -u ltbdb -p ltbdb < contrib/Schema/MySql/schema.sql
```
