FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT 1
WORKDIR /app

COPY ./*.sln Directory.Build.props ./
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
RUN dotnet restore 

COPY ./src ./src
RUN dotnet build -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Events.Server.dll"]
