# Install

## How to run

You need the latest **.NET Core**, **ASP.NET Core** and **MariaDB** to run this application.

## Database

### MariaDB

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
mysql -u ltbdb -p ltbdb < contrib/database-create-mysql.sql
```

### PostgreSQL

Create user.

```sql
create user ltbdb with password 'password';
```

Create database.

```sql
create database ltbdb with owner ltbdb encoding 'UTF8' lc_collate = 'de_DE.UTF-8' lc_ctype = 'de_DE.UTF-8' template template0;
```

Remove create rights for public.

```sql
\c ltbdb

revoke create on schema public from public; 
grant create on schema public to ltbdb;
```

Import schema.

```sh
psql -U ltbdb -h localhost -d ltbdb < contrib/database-create-psql.sql 
```

## Build

### Build and run

```sh
dotnet run --project src/LtbDb2/LtbDb2.csproj
```

### Build and publish

Run in PowerShell or bash:

```sh
dotnet publish -c Release /p:Version=1.0-$(git rev-parse --short HEAD) -o publish src/LtbDb2
dotnet publish/LtbDb2.dll
```

**or**

use `make`.

```sh
make publish
dotnet publish/LtbDb2.dll
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
                "SslProtocols": ["Tls12", "Tls13"],
                "Certificate": {
                    "Path": "/foo/bar/cert.p12|pfx",
                    "Password": "cert_password"
                }
            }
        }
    },
    "ConnectionStrings": {
        "Default": "Host=localhost;Database=ltbdb;Username=ltbdb;Password=ltbdb"
    },
    "Database": {
        "Provider": "PgSql"
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
Set database provider. Default: `PgSql`. Values: `MySql`, `PgSql`.

**Section: Settings**

* **ItemsPerPage**  
Books per page to display.  Default `18`.
* **RecentItems**  
Books per page to display on start page.  Default `18`.
* **Storage**  
Image storage path in filesystem (upload). Default `"./wwwroot/images"`.
* **DefaultImage**  
Path to `no-image` file. Default `"/content/no-image.png"`.
* **ImageWebPath**  
Path to images for download from "wwwroot". Default `"/images/"`.
* **GraphicsMagick**  
Path to GraphicsMagick binary or `gm`. Default `"gm"`.
* **Username**  
Login username. Default `null`.
* **Password**  
Login password. Default `null`.
* **KeyStore**  
Directory to store encryption key files (leave empty to use in-memory). Default `null`.
* **JwtSigningKey**  
JWT access token encryption key. Minimum length 16 characters. Default `null`.
* **JwtExpire**  
JWT access token expire in minutes. Default `null`.

## Logging

Configure logging in `NLog.config` and copy this file to publish directory. 

```xml
<nlog>
  <rules>
    <logger name="System.*" finalMinLevel="Warn" />
    <logger name="Microsoft.*" finalMinLevel="Warn" />
    <logger name="Microsoft.Hosting.Lifetime*" finalMinLevel="Info" />
    <logger name="*" minlevel="Info" writeTo="console,file" />
  </rules>
</nlog>
```