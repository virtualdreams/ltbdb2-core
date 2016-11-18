# Lustiges Taschenbuch Datenbank v2

Portierung nach ASP.NET Core.

Datenbank, um die private Sammlung von Lustigen Taschenbüchern zu tracken.

## Features

* Bücher + Inhalte
* Kategorien
* Suche
* Covers
* Tags
* Authentifizierung

## Frameworks / Tools

* [.NET Core](https://www.microsoft.com/net/core) (v1.0 / v1.1)
* [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
* [MongoDB](https://www.mongodb.com/)
* [jQuery](http://jquery.com/)
* [jQuery-UI](http://jqueryui.com/)
* [jBox](https://github.com/StephanWagner/jBox)
* [jquery validation](https://jqueryvalidation.org/)
* [GraphicsMagick](http://www.graphicsmagick.org/)
* [nodejs](https://nodejs.org/) (v6.9.1 LTS)
* [gulpjs](http://gulpjs.com/)

## Build and deploy

	dotnet restore
	dotnet build --configuration Release
	dotnet publish --configuration Release