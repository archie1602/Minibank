# .NET Minibank API

## About
This is a simple implementation of a mini-bank on .NET 6, C#, Postgresql as a graduation project for the course [.NET Tinkoff.Fintech](https://fintech.tinkoff.ru/study/fintech/.Net/)

Course completion certificate: [certificate](https://github.com/archie1602/Minibank/blob/master/fintech_certificate.pdf)

## Features
- Authorization using JWT tokens
- Show transfer history between different accounts
- Working with users:
    - create user
    - get all users
    - get one user by id
    - edit user
    - remove user
- Working with bank accounts:
    - create account
    - get all accounts
    - get one account by id
    - close account
    - calculate the transfer commission
    - transfer money between two accounts

## Technology stack
This project uses the following:
1. technologies: .NET 6, ASP.NET Core 6 Web Api, REST API, LINQ, TPL (async-await approach), EF Core 6, postgreSQL docker, docker-compose
2. libraries: [FluentValidation](https://docs.fluentvalidation.net/en/latest/), [AutoMapper](https://docs.automapper.org/en/stable/), SwaggerUI, [Moq](https://moq.github.io/moq4/) and [xUnit](https://xunit.net/) for unit-testing

## Architecture
The project architecture is based on the [onion](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/) architecture and uses 3 projects to separate the logic: Core, Data, Web.

## Usage
### Build from source using docker-compose
1. Open the terminal or powershell
2. Clone this repository
3. Change the directory to Minibank:
    ```bash
    cd Minibank
    ```
4. Run docker-compose using the command:
    ```bash
    docker-compose -f build/docker-compose.yml up
    ```
5. To use swagger, open a browser and go to `http://localhost:2109`. If you need to connect to the database, use the following settings: </br>
**Address:** localhost </br>
**Port:** 1602 </br>
**POSTGRES_USER:** minibank-postgres-user </br>
**POSTGRES_PASSWORD:** qwerty12345 </br>

6. To use the bank's API, you must first authorize using the swagger UI (green button in the upper right corner). To authorize, use one of the following demo clients: [demo-clients](https://demo.duendesoftware.com/)

## License
This project does not have a license. Therefore, it cannot be used, modified, or distributed. This project is used as a resume for employers.