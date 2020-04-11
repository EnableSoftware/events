#!/bin/bash
pushd ../../src/Events.Data.Migrations
dotnet ef database update
popd