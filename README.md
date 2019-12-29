# Lustiges Taschenbuch Datenbank v2

Datenbank, um die Sammlung von Lustigen Taschenbüchern zu tracken.

## Features

* Books, Categories, Content and Tags
* Search
* Covers
* Authentification
* REST-API

## Technology

* [.NET Core 2.2](https://www.microsoft.com/net/core)
* [ASP.NET Core 2.2](https://docs.microsoft.com/en-us/aspnet/core/)
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

### Build

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

### Configuration

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
		"SecurityKey": ""
	}
}
```

#### Options

* **ConnectionString**: MariaDB/MySQL connection string `Server=[host];Database=[database];User=[username];Password=[password]`
* **Database**: MongoDB collection name
* **ItemsPerPage**: Books per page to display
* **RecentItems**: Books per page to display on start page 
* **Storage**: Image storage file path
* **NoImage**: Path to "no-image" file
* **CDNPath**: Path to images for download from "www-data"
* **GraphicsMagick**: Path to GraphicsMagick binary or gm
* **Username**: Login username
* **Password**: Login password
* **KeyStore**: Directory to store encryption key files (leave empty to use memory)
* **SecurityKey**: JWT bearer token key, min length 16 characters

### Logging

Configure logging in `NLog.config` and copy this file to publish directory.

```xml
<?xml version="1.0" encoding="utf-8" ?>
  <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
        autoReload="true"
        internalLogLevel="Warn"
        internalLogFile="nlog-internal.log">

  <!-- Load the ASP.NET Core plugin -->
  <extensions>
    <add assembly="NLog.Web.AspNetCore"/>
  </extensions>

  <!-- the targets to write to -->
  <targets>
    <!-- write logs to file -->
    <target xsi:type="File" name="file" fileName="ltbdb-${shortdate}.log" layout="${longdate} ${pad:padding=-5:inner=${uppercase:${level}}} ${logger} ${message} ${exception}" />

    <!-- write logs to console -->
    <target xsi:type="ColoredConsole" name="console" layout="${pad:padding=-5:inner=${uppercase:${level}}} ${logger} ${message} ${exception}" />

    <!-- write to the void -->
    <target xsi:type="Null" name="blackhole" />
  </targets>

  <!-- rules to map from logger name to target -->
  <rules>
    <logger name="ltbdb.*" minlevel="Info" writeTo="console" />
    <logger name="Microsoft.*" minlevel="Trace" writeTo="blackhole" final="true" />
    <logger name="*" minlevel="Debug" writeTo="file" />
  </rules>
</nlog>
```

Also check `logsettings.production.json` and set the appropriate values.

```json
{
	"Logging": {
		"Console": {
			"LogLevel": {
				"Default": "None"
			}
		}
	}
}
```

## REST API

### Login / Token

**Request**

```sh
$ curl -X POST -H "Content-Type: application/json" http://localhost/api/v1/token -d '{ "username": "<username>", "password": "<password>" }'
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
$ curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/1
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
$ curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book -d '{ "number": <number>, "title": "<title>", "category", "<category>", stories: [ "<story 1>", "<story 2>" ], tags: [ "<tag 1>", "<tag 2>" ] }'
```

**Response**

*HTTP200*

### Delete a book

**Request**

```sh
$ curl -X DELETE -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/1
```

**Response**

*HTTP200*