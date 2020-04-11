@echo off
pushd ..\..\src\Events.Data.Migrations
dotnet ef database update
popd