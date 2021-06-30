#!/usr/bin/env python3

import mysql.connector as mysql
import psycopg2 as pgsql
from configparser import ConfigParser
import argparse


def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument("-i", "--ini", required=False,
                        default="dbmigrate.ini", dest="ini", help="Configuration file.")
    parser.add_argument("-m", "--mysql", required=False,
                        default="mysql", dest="mysql_section", help="MySql section.")
    parser.add_argument("-p", "--pgsql", required=False,
                        default="pgsql", dest="pgsql_section", help="PgSql section.")

    subparsers = parser.add_subparsers(dest="command", title="Converter")

    mysql_to_pgsql_parser = subparsers.add_parser("m2p", help="MySql to PgSql")

    pgsql_to_mysql_parser = subparsers.add_parser("p2m", help="PgSql to MySql")

    return parser.parse_args()


def read_config(filename, section):
    parser = ConfigParser()
    parser.read(filename)

    db = {}
    if parser.has_section(section):
        params = parser.items(section)
        for param in params:
            db[param[0]] = param[1]

    return db


def copy_book(src_connection, dst_connection):
    # delete src
    dst_cursor = dst_connection.cursor()
    dst_cursor.execute("delete from \"book\";")
    dst_connection.commit()
    dst_cursor.close()

    # copy from src
    src_cursor = src_connection.cursor()
    src_cursor.execute(
        "select id, number, title, category, created, modified, filename from \"book\";")
    for (id, number, title, category, created, modified, filename) in src_cursor:
        print("Insert into \"book\"", id)
        # insert to dst
        dst_cursor = dst_connection.cursor()
        dst_cursor.execute("insert into \"book\" (id, number, title, category, created, modified, filename) values (%s, %s, %s, %s, %s, %s, %s);",
                           (id, number, title, category, created, modified, filename))
        dst_connection.commit()
        dst_cursor.close()
    src_cursor.close()


def copy_story(src_connection, dst_connection):
    # delete src
    dst_cursor = dst_connection.cursor()
    dst_cursor.execute("delete from \"story\";")
    dst_connection.commit()
    dst_cursor.close()

    # copy from src
    src_cursor = src_connection.cursor()
    src_cursor.execute("select id, name, bookid from \"story\";")
    for (id, name, bookid) in src_cursor:
        print("Insert into \"story\"", id)
        # insert to dst
        dst_cursor = dst_connection.cursor()
        dst_cursor.execute("insert into \"story\" (id, name, bookid) values (%s, %s, %s);",
                           (id, name, bookid))
        dst_connection.commit()
        dst_cursor.close()
    src_cursor.close()


def copy_tag(src_connection, dst_connection):
    # delete src
    dst_cursor = dst_connection.cursor()
    dst_cursor.execute("delete from \"tag\";")
    dst_connection.commit()
    dst_cursor.close()

    # copy from src
    src_cursor = src_connection.cursor()
    src_cursor.execute("select id, name, bookid from \"tag\";")
    for (id, name, bookid) in src_cursor:
        print("Insert into \"tag\"", id)
        # insert to dst
        dst_cursor = dst_connection.cursor()
        dst_cursor.execute("insert into \"tag\" (id, name, bookid) values (%s, %s, %s);",
                           (id, name, bookid))
        dst_connection.commit()
        dst_cursor.close()
    src_cursor.close()


def psql_set_sequence(dst_connection):
    # book
    print("Set sequence \"book\"")
    dst_cursor = dst_connection.cursor()
    dst_cursor.execute(
        "SELECT setval('book_id_seq', (SELECT MAX(id) from \"book\"));")
    dst_connection.commit()
    dst_cursor.close()

    # story
    print("Set sequence \"story\"")
    dst_cursor = dst_connection.cursor()
    dst_cursor.execute(
        "SELECT setval('story_id_seq', (SELECT MAX(id) from \"story\"));")
    dst_connection.commit()
    dst_cursor.close()

    # tag
    print("Set sequence \"tag\"")
    dst_cursor = dst_connection.cursor()
    dst_cursor.execute(
        "SELECT setval('tag_id_seq', (SELECT MAX(id) from \"tag\"));")
    dst_connection.commit()
    dst_cursor.close()


def mysql_set_mode(dst_connection):
    dst_cursor = dst_connection.cursor()
    dst_cursor.execute(
        "set sql_mode = 'ANSI_QUOTES';")
    dst_connection.commit()
    dst_cursor.close()


def main():
    # parse arguments
    args = parse_args()

    # get connection data
    mysql_dsn = read_config(args.ini, args.mysql_section)
    pgsql_dsn = read_config(args.ini, args.pgsql_section)

    if args.command == "m2p":
        # open connections
        src_connection = mysql.connect(**mysql_dsn)
        dst_connection = pgsql.connect(**pgsql_dsn)

        # set mysql mode
        mysql_set_mode(src_connection)
    elif args.command == "p2m":
        # open connections
        src_connection = pgsql.connect(**pgsql_dsn)
        dst_connection = mysql.connect(**mysql_dsn)

        # set mysql mode
        mysql_set_mode(dst_connection)
    else:
        print("No converter selected.")
        return 1

    # copy src to dst
    copy_book(src_connection, dst_connection)
    copy_story(src_connection, dst_connection)
    copy_tag(src_connection, dst_connection)

    if args.command == "m2p":
        psql_set_sequence(dst_connection)

    src_connection.close()
    dst_connection.close()


if __name__ == "__main__":
    main()
