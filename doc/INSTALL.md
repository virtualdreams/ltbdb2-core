# Install

## Prerequisites

You need the latest **.NET Core**, **ASP.NET Core** and **PostgreSQL** or **MariaDB** to run this application.

## From source

### Run from source

```sh
dotnet run --project src/LtbDb2/LtbDb2.csproj
```

### Publish and run from source

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

## Docker

### Build docker image

Builder docker image.

```sh
docker build --no-cache -t ltbdb2-core .
```

### RUn docker image

Run image with host mount.

```sh
docker run -d \
    -p 5000:5000 \
    -e LTBDB__ConnectionString_PgSql=Host=hostname;Database=ltbdb;Username=ltbdb;Password=ltbdb \
    -e LTBDB__Settings__JwtSigningKey=1234567890123456 \
    -e LTBDB__Settings__Username=demo \
    -e LTBDB__Settings__Password=demo \
    -v /path/to/data:/data ltbdb2-core
```

Run image with named volume.

```sh
docker volume create ltbdb2
docker run -d \
    -p 5000:5000 \
    -e LTBDB__ConnectionString_PgSql=Host=hostname;Database=ltbdb;Username=ltbdb;Password=ltbdb \
    -e LTBDB__Settings__JwtSigningKey=1234567890123456 \
    -e LTBDB__Settings__Username=demo \
    -e LTBDB__Settings__Password=demo \
    -v ltbdb2:/data ltbdb2-core
```
