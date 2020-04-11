#!/bin/bash
pushd ./scripts/linux

echo "[Setting up docker] portainer (localhost:9000), pgadmin (localhost:5050), postgres (localhost:5442)"
sh docker.sh
echo

echo "[Running migrations]"
sh update-database.sh
echo

echo "[Starting server]"
sh start-server.sh
echo

popd