# Lustiges Taschenbuch Datenbank v2

Database to manage the collection of "Lustiges Taschenbuch".

## Features

* Books, Categories, Content and Tags
* Search
* Covers
* REST-API

## Technology

* [.NET 5.0](https://dotnet.microsoft.com/)
* [ASP.NET Core 5.0](https://dotnet.microsoft.com/)
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
dotnet publish -c Release /p:Version=1.0-$(git rev-parse --short HEAD) -o publish src/LtbDb2
dotnet /publish/LtbDb2.dll
```

**or**

use `make`.

```sh
make publish
dotnet /publish/LtbDb2.dll
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
    "ConnectionStrings": {
        "Default": "Server=localhost;Database=ltbdb;User=ltbdb;Password=ltbdb"
    },
    "Database": {
        "Provider": "MySql"
    },
    "Settings": {
        "ItemsPerPage": 18,
        "RecentItems": 18,
        "Storage": "./wwwroot/images/",
        "DefaultImage": "/content/no-image.png",
        "ImageWebPath": "/images/",
        "GraphicsMagick": "gm",
        "Username": "",
        "Password": "",
        "KeyStore": "",
        "AccessTokenKey": "1234567890123456"
    }
}
```

## Options (appsettings.json)

**Section: ConnectionStrings**

* **Default**  
MariaDB/MySQL connection string.  
`Server=[host];Database=[database];User=[username];Password=[password]`  
PosgreSql connection string.  
`Host=[host];Database=[database];Username=[username];Password=[password][;SearchPath=schema,public]`

**Section: Database**

* **Provider**  
MySql or PgSql

**Section: Settings**

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
* **AccessTokenKey**  
JWT access token encryption key. Min length 16 characters.
* **AccessTokenExpire**  
JWT access token expire in minutes.

## Logging

Configure logging in `NLog.config` and copy this file to publish directory. Also check `logsettings.production.json` and set the appropriate values.

## REST API

### Authentication

**Request**

```sh
curl -X POST -H "Content-Type: application/json" http://localhost/api/v1/user/authenticate -d '{ "username": "<username>", "password": "<password>" }'
```

**Response**

*Success*

```json
{
    "Token": "<token>",
    "Type": "Bearer",
    "ExpiresIn": <time_in_minutes>
}
```

*BadRequest*

```json
[
    {
        "Field": "<fieldname>",
        "Messages": [
            "<message>"
        ]
    }
]
```

### Get all books

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book[?category=Category&tag=Tag]
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| category | | The category to filter. | no |
| tag | | The tag to filter. | no |

**Response**

*Success*

```json
[
    {
        "Id": 1,
        "Number": 1,
        "Title": "Title 1",
        "Category": "Category 1",
        "Created": "2019-12-29T16:34:47",
        "Modified": "2019-12-29T16:34:47",
        "Filename": null,
        "Stories": [],
        "Tags": []
    },
    {
        "Id": 2,
        "Number": 2,
        "Title": "Title 1",
        "Category": "Category 1",
        "Created": "2019-12-29T16:35:48",
        "Modified": "2019-12-29T16:35:48",
        "Filename": null,
        "Stories": [],
        "Tags": []
	}
]
```

### Get a book by id

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id>
```

**Response**

*Success*

```json
{
    "Id": 1,
    "Number": 1,
    "Title": "Title 1",
    "Category": "Category 1",
    "Created": "2019-12-29T16:34:47",
    "Modofied": "2019-12-29T16:34:47",
    "Filename": null,
    "Stories": [],
    "Tags": []
}
```

### Create a book

**Request**

```sh
curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book -d '{ "number": <number>, "title": "<title>", "category", "<category>", stories: [ "<story 1>", "<story 2>" ], tags: [ "<tag 1>", "<tag 2>" ] }'
```

**Response**

*Success*

None

*BadRequest*

```json
[
    {
        "Field": "<fieldname>",
        "Messages": [
            "<message>"
        ]
    }
]
```

### Edit a book

**Request**

```sh
curl -X PUT -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id> -d '{ "number": <number>, "title": "<title>", "category", "<category>", stories: [ "<story 1>", "<story 2>" ], tags: [ "<tag 1>", "<tag 2>" ] }'
```

**Response**

*Success*

None

*BadRequest*

```json
[
    {
        "Field": "<fieldname>",
        "Messages": [
            "<message>"
        ]
    }
]
```

### Delete a book

**Request**

```sh
curl -X DELETE -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/book/<id>
```

**Response**

*Success*

None

### Get image for a book

**Request**

```sh
curl -X GET -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/<id>
```

Request parameters

| Name | Value | Description | Required |
|:--- |:--- |:--- |:---
| type | thumbnail | Get thumbnail instead of the full image. | no |

**Response**

*Success*

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
curl -X PUT -H "Authorization: Bearer <token>" -H "Content-Type: multipart/form-data" -F "image=@file.png" http://localhost/api/v1/image/<id>
```

**Response**

*Success*

None

### Delete image from a book

**Request**

```sh
curl -X DELETE -H "Content-Type: application/json" -H "Authorization: Bearer <token>" http://localhost/api/v1/image/1
```

**Response**

*Success*

None