project = src/LtbDb2

.PHONY: all
all: clean restore publish

.PHONY: install-npm
install-npm: clean-npm
	cd $(project) && npm ci

.PHONY: clean-npm
clean-npm:
	cd $(project) && rm -rf node_modules

.PHONY: grunt
grunt:
	@if [ -d "$(project)/node_modules" ]; then \
		cd $(project) && ./node_modules/grunt/bin/grunt; \
	else \
		echo "'grunt' not installed. Please run 'make install-npm'."; \
	fi 

.PHONY: restore
restore:
	dotnet restore

.PHONY: build
build:
	dotnet build -c Release

.PHONY: publish
publish:
	dotnet publish -c Release /p:Version=1.0-$$(git rev-parse --short HEAD) -o publish $(project)

.PHONY: clean
clean:
	rm -rf publish
	cd $(project) && rm -rf bin
	cd $(project) && rm -rf obj
	cd $(project) && rm -rf node_modules
