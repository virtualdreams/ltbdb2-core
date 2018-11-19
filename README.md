# Lustiges Taschenbuch Datenbank v2

Datenbank, um die Sammlung von Lustigen Taschenbüchern zu tracken.

## Features

* Bücher und Inhalte
* Kategorien
* Suche
* Covers
* Tags
* Authentifizierung
* Simple WebAPI to manage books

## Technology

* [.NET Core 2.1](https://www.microsoft.com/net/core)
* [ASP.NET Core 2.1](https://docs.microsoft.com/en-us/aspnet/core/)
* [MongoDB](https://www.mongodb.com/)
* [jQuery](http://jquery.com/)
* [jQuery-UI](http://jqueryui.com/)
* [jBox](https://github.com/StephanWagner/jBox)
* [jquery validation](https://jqueryvalidation.org/)
* [GraphicsMagick](http://www.graphicsmagick.org/)
* [nodejs](https://nodejs.org/)
* [gulpjs](http://gulpjs.com/)

## How to run

You need the latest **.NET Core**, **ASP.NET Core** and **MongoDB** to run this application.

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
dotnet publish -c Release /p:Version=1.0-$(git rev-parse --short HEAD)
dotnet /path/to/ltbdb2.dll
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
		"MongoDB": "mongodb://127.0.0.1/",
		"Database": "ltbdb",
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

* **MongoDB**: MongoDB connection string
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
* **SecurityKey**: JWT bearer token key

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

Additionally review `logsettings.Production.json`.

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