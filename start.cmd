@echo off
pushd .\scripts\windows

ECHO [Setting up docker] postgres (localhost:5442)
call docker.cmd
ECHO.

ECHO [Running migrations]
call update-database.cmd
ECHO.

ECHO [Starting server]
call start-server.cmd
ECHO.

popd
