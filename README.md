# Lustiges Taschenbuch Datenbank v2

Database to manage the collection of "Lustiges Taschenbuch".

## Features

* Books, Categories, Content and Tags
* Search
* Covers
* REST-API

## Technology

* [.NET Core 3.1](https://www.microsoft.com/net/core)
* [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/)
* [MariaDB](https://mariadb.org/)
* [jQuery](http://jquery.com/)
* [jQuery-UI](http://jqueryui.com/)
* [jBox](https://github.com/StephanWagner/jBox)
* [jquery validation](https://jqueryvalidation.org/)
* [GraphicsMagick](http://www.graphicsmagick.org/)
* [nodejs](https://nodejs.org/)
* [gulpjs](http://gulpjs.com/)

## How to run

You need the latest **.NET Core**, **ASP.NET Core** and **MariaDB** to run this application.

## Build

**Build to run on local**

```sh
dotnet restore
dotnet build
dotnet run
```

**Build and publish**

Run in PowerShell or bash:

```sh
$ dotnet publish -c Release /p:Version=1.0-$(git rev-parse --short HEAD)
$ dotnet /path/to/ltbdb2.dll
```

**or**

use `make`.

```sh
$ make publish
$ dotnet /path/to/ltbdb2.dll
```

## Configuration

```json
{
	"Kestrel": {
		"EndPoints": {
			"Http": {
				"Url": "http://127.0.0.1:5000"
			},
			"Https": {
				"Url": "https://127.0.0.1:5001",
				"Certificate": {
					"Path": "/foo/bar/cert.p12|pfx",
					"Password": "cert_password"
				}
			}
		}
	},
	"Settings": {
		"ConnectionString": "Server=localhost;Database=ltbdb;User=ltbdb;Password=ltbdb",
		"ItemsPerPage": 18,
		"RecentItems": 18,
		"Storage": "./wwwroot/images/",
		"NoImage": "/content/no-image.png",
		"CDNPath": "/images/",
		"GraphicsMagick": "gm",
		"Username": "",
		"Password": "",
		"KeyStore": "",
		"SecurityKey": "1234567890123456"
	}
}
```

## Options

* **ConnectionString**  
MariaDB/MySQL connection string.  
`Server=[host];Database=[database];User=[username];Password=[password]`
* **ItemsPerPage**  
Books per page to display.
* **RecentItems**  
Books per page to display on start page.
* **Storage**  
Image storage path in filesystem (upload).
* **NoImage**  
Path to `no-image` file.
* **CDNPath**  
Path to images for download from "wwwroot".
* **GraphicsMagick**  
Path to GraphicsMagick binary or `gm`.
* **Username**  
Login username.
* **Password**  
Login password.
* **KeyStore**  
Directory to store encryption key files (leave empty to use in-memory).
* **SecurityKey**  
JWT bearer token encryption key. Min length 16 characters.

## Logging

Configure logging in `NLog.config` and copy this file to publish directory. Also check `logsettings.production.json` and set the appropriate values.

## REST API

### Login / Token

**Request**

```sh
$ curl -X POST -H "Content-Type: application/json" http://localhost/api/v1/login -d '{ "username": "<username>", "password": "<password>" }'
```

**Response**

```json
{
    "Token": "<token>"
}
```

### Get all books

Request

```sh
$ curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book
```

**Response**

```json
[
    {
        "Id": 1,
        "Number": 1,
        "Title": "Title",
        "Category": "Category",
        "Created": "2019-12-29T16:34:47",
        "Filename": null,
        "Stories": [],
        "Tags": []
    },
    {
        "Id": 2,
        "Number": 2,
        "Title": "Title",
        "Category": "Category",
        "Created": "2019-12-29T16:35:48",
        "Filename": null,
        "Stories": [],
        "Tags": []
	}
]
```

### Get a book by id

**Request**

```sh
$ curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id>
```

Response

```json
{
    "Id": 1,
    "Number": 1,
    "Title": "Title",
    "Category": "Category",
    "Created": "2019-12-29T16:34:47",
    "Filename": null,
    "Stories": [],
    "Tags": []
}
```

### Create a book

**Request**

```sh
$ curl -X PUT -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book -d '{ "number": <number>, "title": "<title>", "category", "<category>", stories: [ "<story 1>", "<story 2>" ], tags: [ "<tag 1>", "<tag 2>" ] }'
```

**Response**

```json
{
    "Id": 1234
}
```

### Edit a book

**Request**

```sh
$ curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id> -d '{ "number": <number>, "title": "<title>", "category", "<category>", stories: [ "<story 1>", "<story 2>" ], tags: [ "<tag 1>", "<tag 2>" ] }'
```

**Response**

None

### Delete a book

**Request**

```sh
$ curl -X DELETE -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id>
```

**Response**

None

### Get image paths for a book

**Request**

```sh
$ curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/<id>
```

**Response**

```json
{
    "Thumbnail": "/path/to/image/thumb/image.jpg",
    "Image": "/path/to/image/image.jpg"
}
```

Paths can be `null`.

### Set image for a book

**Request**

```sh
$ curl -X POST -H "Authorization: Bearer <token>" -H "Content-Type: multipart/form-data" -F "image=@file.png" http://localhost/api/v1/image/<id>
```

**Response**

None

### Delete image from a book

**Request**

```sh
$ curl -X DELETE -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/1
```

**Response**

None