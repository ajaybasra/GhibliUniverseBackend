FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
EXPOSE 3000
# Sets directory from which commands are run. WORKDIR is like a cd inside the container.
WORKDIR /app
# .sln and .csproj are copied to the directories where they belong, * wildcard is used so that name doesn't have to be specified.
COPY *.sln ./ 

# Restores the dependencies/tools of the project as distinct layers.
COPY ./GhibliUniverse.Console/*.csproj ./GhibliUniverse.Console/
COPY ./GhibliUniverse.API/*.csproj ./GhibliUniverse.API/
COPY ./GhibliUniverse.Tests/*.csproj ./GhibliUniverse.Tests/
RUN dotnet restore
COPY ./ ./
RUN dotnet build

FROM build AS test
WORKDIR /app/GhibliUniverse.Tests
RUN dotnet test

FROM build AS publish
WORKDIR /app/GhibliUniverse
RUN dotnet publish -c Release -o publish

# Creates runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish app/GhibliUniverse/publish ./
ENTRYPOINT ["dotnet", "GhibliUniverse.dll"]

