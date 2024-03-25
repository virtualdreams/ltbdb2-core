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
	-e LTBDB__ConnectionStrings__PgSql='Host=<PostgresServer>;Database=ltbdb;Username=ltbdb;Password=ltbdb' \
	-e LTBDB__Database__Provider=PgSql \
	-e LTBDB__Settings__JwtSigningKey=12345678901234561234567890123456 \
	-e LTBDB__Settings__Username=test \
	-e LTBDB__Settings__Password=test \
	# -v $(pwd)/storage/key-store:/data/keystore \
	# -v $(pwd)/storage/images:/data/images \
	# or
	# -v ltbdb-vol:/data
	ltbdb2-core
```