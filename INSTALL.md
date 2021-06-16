# Install

## How to run

You need the latest **.NET Core**, **ASP.NET Core** and **MariaDB** to run this application.

## Database

**MariaDB**

Create database.

```sql
# mysql

create database ltbdb;
```

Import schema.

```sh
mysql -u ltbdb -p ltbdb < contrib/database-create-mysql.sql
```

**PostgreSQL**

Create database.

```sql
# su - postgres -c psql

create database ltbdb with owner ltbdb encoding 'UTF8' lc_collate = 'de_DE.UTF-8' lc_ctype = 'de_DE.UTF-8' template template0;
```

Import schema.

```sh
psql -U ltbdb -h localhost -d ltbdb < contrib/database-create-psql.sql 
```

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