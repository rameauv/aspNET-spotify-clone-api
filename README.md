
# Back-end for a clone of Spotify made with Asp.net Core

  

>

  

## About

  

This project is a back-end for a clone of Spotify made with Asp.net Core

  

## Getting Started

  

Getting up and running is as easy as 1, 2, 3.

  

1. Make sure you have [Dotnet core](https://dotnet.microsoft.com/en-us/download) installed and a [PostgreSQL](https://www.postgresql.org/) database running.

2. Install your dependencies

  

```

cd path/to/repo; yarn or npm install

```

  

3. Start your app

  

```

cd path/to/repo; cd API; dotnet run

```

  

## Testing

  

Simply run `dotnet test at the racine of the repo`.


## Configuration

  
The configuration must be set in a `appsettings.json` file in the `API` folder.
An exemple configuration file, `appsettings.json.exemple` is provided in the `API` folder.


```

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Jwt": {
    "Issuer": "https:/issuer/",
    "Audience": "https://audience/",
    "AccessTokenKey": "This is a sample secret key - please don't use in production environment.'",
    "RefreshTokenKey": "This is a sample secret key - please don't use in production environment.'",
    "AccessTokenExpiryInMinutes": 5,
    "RefreshTokenExpiryInMinutes": 7
  },
  "ConnectionStrings": {
    "DBContext": "Host=localhost;Database=MyDbName;Username=postgres;Password=myPassword"
  },
  "Spotify": {
    "ClientId": "client Id",
    "ClientSecret": "client secret"
  }
}

```

Launch settings are available in the file `repo/API/Properties/launchSettings.json


## Routes

A playground is available at the path `swagger/index.html`


## Doc

The documentation is available in the `doc` folder.

## Todo

- More Unit tests
- UAT tests
- E2e tests
- Better documentation
- More code comments
- Use an authentication service like Auth0
