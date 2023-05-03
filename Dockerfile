FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
EXPOSE 3000
WORKDIR /app
COPY *.sln ./
COPY ./GhibliUniverse/*.csproj ./GhibliUniverse/
COPY ./GhibliUniverseTests/*.csproj ./GhibliUniverseTests/
RUN dotnet restore
COPY ./ ./
RUN dotnet build

FROM build AS test
WORKDIR /app/GhibliUniverseTests
RUN dotnet test

FROM build AS run
WORKDIR /app/GhibliUniverse
ENTRYPOINT ["dotnet", "run"]

