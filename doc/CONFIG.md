# Configuration

## Example configuration

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
        "PgSql": "Host=localhost;Database=ltbdb;Username=ltbdb;Password=ltbdb"
    },
    "Database": {
        "Provider": "PgSql"
    },
    "FeatureFlags": {
        "ShowTagsPage": false
    },
    "Settings": {
        "ItemsPerPage": 15,
        "RecentItems": 15,
        "Storage": "imageroot",
        "DefaultImage": "/content/no-image.png",
        "GraphicsMagick": "gm",
        "Username": "demo",
        "Password": "demo",
        "KeyStore": "keystore",
        "JwtSigningKey": "1234567890123456",
        "JwtExpireTime": 5
    }
}
```

## Options (appsettings.json)

**Section: ConnectionStrings**

* **PgSql**  
PosgreSql connection string.  
Default: `Host=[host];Database=[database];Username=[username];Password=[password][;SearchPath=schema,public]`

* **MySql**  
MariaDB/MySQL connection string.  
Default: `Server=[host];Database=[database];User=[username];Password=[password]`

**Section: Database**

* **Provider**  
Set database provider.  
Default: `PgSql`. Values: `MySql`, `PgSql`.

**Section: FeatureFlags**

* **ShowTagsPage**:  
Enable tags overview page.  
Default: `false`.

**Section: Settings**

* **ItemsPerPage**  
Books per page to display.  
Default `15`.
* **RecentItems**  
Books per page to display on start page.  
Default `15`.
* **Storage**  
Image storage path in filesystem (upload).  
Default `"" (imageroot in application root)`.
* **DefaultImage**  
Path to `no-image` file.  
Default `"/content/no-image.png"`.
* **GraphicsMagick**  
Path to GraphicsMagick binary or `gm`.  
Default `"gm"`.
* **Username**  
Login username.  
Default `null`.
* **Password**  
Login password.  
Default `null`.
* **KeyStore**  
Directory to store encryption key files (leave empty to use in-memory).  
Default `null`.
* **JwtSigningKey**  
JWT access token encryption key. Minimum length 16 characters.  
Default `null`.
* **JwtExpire**  
JWT access token expire in minutes.  
Default `null`.
