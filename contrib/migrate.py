# Script to migrate from MongoDB to MySQL/MariaDB

# requisites needed
#python-pymongo
#python-mysql-connector

import pymongo
import mysql.connector

# init mongodb
mgoClient = pymongo.MongoClient("mongodb://127.0.0.1:27017")
mgoDb = mgoClient["ltbdb"]
mgoBookCollection = mgoDb["book"]

# init mysql
myClient = mysql.connector.connect(host="localhost", user="ltbdb", passwd="ltbdb", database="ltbdb")
myCursor = myClient.cursor()

# get all users
for book in mgoBookCollection.find():
	# values
	number = book["Number"]
	title = book["Title"]
	category = book["Category"]
	created = book["Created"]
	filename = book["Filename"]
	stories = book["Stories"]
	tags = book["Tags"]
	
	# statement
	sql = "insert into book (number, title, category, created, filename) values(%s, %s, %s, %s, %s)"
	val = (number, title, category, created, filename)
	
	# execute
	myCursor.execute(sql, val)
	myClient.commit()
	id = myCursor.lastrowid
	
	print("Inserted book:", number, title)
	
	# extract storiey from book
	if stories is not None:
		for story in stories:
			# statement
			sql = "insert into story (name, bookid) values (%s, %s)"
			val = (story, id)
			
			# execute
			myCursor.execute(sql, val)
			myClient.commit()
			
			print("Inserted story: ", story, id)

	# extract tags from book
	if tags is not None:
		for tag in tags:
			# statement
			sql = "insert into tag (name, bookid) values (%s, %s)"
			val = (tag, id)
			
			# execute
			myCursor.execute(sql, val)
			myClient.commit()
			
			print("Inserted tag: ", tag, id)
