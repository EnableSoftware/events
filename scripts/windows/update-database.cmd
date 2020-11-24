@echo off
pushd ..\..\src\Events.Data.Postgres
dotnet ef database update --msbuildprojectextensionspath ./obj/local/
popd