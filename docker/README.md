# Notes - Docker

Dockerfile is found in root of repository.

## Build image

```sh
docker build --no-cache -t ltbdb2-core .
```

## Run image

```sh
docker run \
	-it \
	--rm \
	-p 5000:5000 \
	-e ConnectionStrings__Default='Host=<PostgresServer>;Database=ltbdb;Username=ltbdb;Password=ltbdb' \
	-e Settings__JwtSigningKey=1234567890123456 \
	-e Settings__Username=test \
	-e Settings__Password=test \
	# -v $(pwd)/storage/key-store:/app/key-store \
	# -v $(pwd)/storage/images:/app/wwwroot/images \
	ltbdb2-core
```