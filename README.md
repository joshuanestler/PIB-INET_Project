# PIB-INET_Project

This is a project for the course "Internet Technologien" at the University of Applied Sciences in Saarland. 

This project is Written using [ASP.NET](https://learn.microsoft.com/en-us/aspnet/overview) and [Blazor WebAssembly](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor).

## Description

The application is a pokémon game in which the user must guess a pokémon. The user gets a hint (in form of differences in specific values of the guessed pokemon to the pokémon he has to guess).

The pokémon data is fetched from the [PokéAPI](https://pokeapi.co/).
The pokémon can be guessed in some different languages. The language is chosen by the user in the settings.

It allows for some variations/complications in the game:
- The user can choose in the settings to hide random values of his/her guessed pokémon.
- The user can choose to only show the last X guesses he/she made.

## Installation

Ensure that you have the [.NET Core SDK 7](https://dotnet.microsoft.com/download) installed.

Customize your `Properties/launchSettings.json` file to your needs (i.e. like so).

```json
{
  "profiles": {
    "Pokewordle": {
      "commandName": "Project",
      "applicationUrl": "https://<YOUR_IP>:55864;http://<YOUR_IP>:55865"
    }
  }
}
```

Download the git repository and run the following commands in the `/Pokewordle` directory of the project:

```bash
dotnet restore
dotnet build
dotnet run
```
