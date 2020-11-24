FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /db

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ./src/Events.Data.Model ./Events.Data.Model
COPY ./src/Events.Data.Postgres ./Events.Data.Postgres
COPY ./src/Events.Server/appsettings.json ./Events.Server/appsettings.json

WORKDIR /db/Events.Data.Postgres

RUN chmod +x ./entrypoint.sh
CMD "/bin/sh" "./entrypoint.sh"