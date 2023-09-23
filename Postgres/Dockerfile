FROM postgres:15.4-bullseye as db
WORKDIR /app
COPY ./scripts/init.sh /docker-entrypoint-initdb.d
COPY ./scripts/create-table.sql ./scripts/db/create-table.sql