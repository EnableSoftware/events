#!/bin/bash
pushd ../../src/Events.Data.Migrations

read -p "Migration Name: " name

dotnet ef migrations add $name

popd

read -p "Press any key to close..."