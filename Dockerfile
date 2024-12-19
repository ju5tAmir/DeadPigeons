# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln ./
COPY server/API/*.csproj ./server/API/
COPY server/DataAccess/*.csproj ./server/DataAccess/
COPY server/Service/*.csproj ./server/Service/
COPY server/ApiIntegrationTests/*.csproj ./server/ApiIntegrationTests/
RUN dotnet restore

# Copy the rest of the files
COPY server/API/. ./API/
COPY server/DataAccess/. ./DataAccess/
COPY server/Service/. ./Service/
COPY server/ApiIntegrationTests/. ./ApiIntegrationTests/

# Publish the app in release mode
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Api.dll"]