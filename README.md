# Ghibli Universe API

An ASP.NET Core Web API crafted for managing voice actors, films, and reviews associated with the amazing Studio Ghibli films.
<img src="https://static.wikia.nocookie.net/studio-ghibli/images/b/b8/Spirited_Sea.png/revision/latest?cb=20210613010930">

## Usage

### Pre-requisites

- .NET 7+
- Docker

### Running API

To run the entire API including a local containerised PostgreSQL database, run:

```
docker-compose build
docker-compose up -d
```

Open your browser and navigate to http://localhost:8080/swagger/index.html. This opens up Swagger UI allowing you to make calls to the API which talk to a persisting PostgreSQL db.

### Running Console Project

To run the console project, run `dotnet run --project GhibliUniverse.Console [arguments go here]`. There is no documentation for the arguments, so go to `ArgumentProcessor.cs` -> `Process` to see what kinds of args can be passed.


### Running Tests

To execute the test suite, navigate to the root directory and run:

`dotnet test`

## Accessing Live Version

Visit the Swagger documentation [here](https://ghibliuniverse.svc.platform.myobdev.com/swagger/index.html) to learn more about the three endpoints and to directly interact with the API.

## Solution structure

### Overview

- `GhibliUnivere.API` contains source code for API
- `GhibliUniverse.IntegrationTests` contains integrations tests for the controllers
- `GhibliUniverse.Console` contains source code for console project
- `GhibliUniverse.Console.Tests` contains unit tests for the argument process functionality provided by the console project
- `GhibliUniverse.Core` contains the core logic which is utilised by both the api and console projects, such as the service and repository layers
- `GhibliUniverse.Core.Tests` contains unit tests for the service layer 

## Hexagonal Architecture

The hexagonal architecture pattern has been used to create a loosely coupled project which is thus flexible, meaning that we can easily swap out components such as the DB, console interface or API without needing to change the core application logic.

In our case, the core project is at the center, encapsulating the core application logic without any external dependencies.

The API and console projects are primary adapters, serving as entry points into the core project. They depend on the core project to access and utilise the business logic. They translate external inputs into calls to the core logic and adapt core outputs for external presentation or communication.

[diagram goes here]

## Database ERD

Below you can see that we have a one-to-many relationship between `Film` and `Review` and a many-to-many relationship betweem `Film` and `VoiceActor` which is resolved by an intermediate table.

`Review` cannot exist without a parent `Film` (composition) and when a film is deleted, so are associated reviews.


<img width="342" alt="Screenshot 2023-09-06 at 11 22 00 AM" src="https://github.com/myob-fma/ajay-project-ghibli/assets/66146062/e41a00e8-7cda-4471-977e-4f5d9afa77b3">

## Testing Strategy

### Unit tests

Unit tests have been written for both the Console and Core project, testing things such as the argument processor and the service classes. Mocking has been used when appropiate to achieve isolation and achieve a limited scope. For example, the depedencies of the service classes are all mocked out (the repository layer in this case).

### Integrations tests

All three controllers are both tested using an in-memory database, and an in-memory server.

