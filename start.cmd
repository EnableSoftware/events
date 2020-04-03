@echo off
pushd scripts

ECHO [Setting up docker] portainer (localhost:9000), pgadmin (localhost:5050), postgres (localhost:5442)
call docker.cmd
ECHO.

ECHO [Running migrations]
call update-database.cmd
ECHO.

ECHO [Starting server]
call start-server.cmd
ECHO.

popd