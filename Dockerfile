FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
# .sln and .csproj are copied to the directories where they belong, * wildcard is used so that name doesn't have to be specified.
COPY *.sln ./ 
COPY ./GhibliUniverse.Console/*.csproj ./GhibliUniverse.Console/
COPY ./GhibliUniverse.API/*.csproj ./GhibliUniverse.API/
COPY ./GhibliUniverse.Core/*.csproj ./GhibliUniverse.Core/
COPY ./GhibliUniverse.Console.Tests/*.csproj ./GhibliUniverse.Console.Tests/
COPY ./GhibliUniverse.Core.Tests/*.csproj ./GhibliUniverse.Core.Tests/

RUN dotnet restore
COPY ./ ./

RUN dotnet build

FROM base AS test
RUN dotnet test

FROM base AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
EXPOSE 3000
ENTRYPOINT ["dotnet", "GhibliUniverse.API.dll"]
# ENTRYPOINT ["/bin/sh"]
