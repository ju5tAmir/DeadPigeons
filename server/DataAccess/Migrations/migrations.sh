#!/bin/bash
# The purpose of this script is to add new columns to the AspNet* tables.
# You can edit the entity in DataAccess/Entities/ and then run this.
# WARNING: It drops all previous database records

# Go to the rootDir
cd ../../../

# Migration title (Generates a random alphanumeric string of length 12)
NAME=$(for i in {1..12}; do echo -n "${RANDOM}" | tr -dc 'A-Za-z0-9' | head -c 1; done)

# Hard reset on docker
echo "Stage: Docker reset"
if docker compose down --remove-orphans > /dev/null 2>&1 && docker compose up -d > /dev/null 2>&1; then
    echo "Done"
else
    echo "Error: Docker reset failed"
    exit 1
fi

# Add migrations 
echo "Stage: Add migrations"
if dotnet ef migrations add $NAME --project server/DataAccess/DataAccess.csproj --startup-project ./server/API/API.csproj > /dev/null 2>&1; then
    echo "Done"
else
    echo "Error: Migration creation failed"
    exit 1
fi

# Update the database
echo "Stage: Update database"
if dotnet ef database update --project server/DataAccess/DataAccess.csproj --startup-project ./server/API/API.csproj > /dev/null 2>&1; then
    echo "Done"
else
    echo "Error: Database update failed"
    exit 1
fi
