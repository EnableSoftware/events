@echo off
pushd ..\src\Events.Data.Migrations
set /p name="Migration Name: "
dotnet ef migrations add %name%
popd
pause