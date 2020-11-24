#!/bin/bash
pushd ../../src/Events.Data.Postgres

read -p "Migration Name: " name

dotnet ef migrations add $name --msbuildprojectextensionspath ./obj/local/

popd

read -p "Press any key to close..."