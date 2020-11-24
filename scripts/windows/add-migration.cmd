@echo off
pushd ..\..\src\Events.Data.Postgres
set /p name="Migration Name: "
dotnet ef migrations add %name% --msbuildprojectextensionspath ./obj/local/
popd
pause