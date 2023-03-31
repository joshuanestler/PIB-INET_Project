using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;
using PokeApiNet;
using Pokewordle;
using Pokewordle.Shared;
using Pokewordle.Shared.Services;
using Pokewordle.Shared.Util;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

HttpClient httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
PokeApiClient pokeApiClient = new PokeApiClient();

builder.Services.AddSingleton<SettingsService>();

builder.Services.AddScoped(sp => httpClient);
builder.Services.AddScoped(sp => pokeApiClient);
builder.Services.AddScoped(sp => new PokemonList());
builder.Services.AddFluentUIComponents();

WebAssemblyHost host = builder.Build();

SettingsService settingsService = host.Services?.GetService<SettingsService>()!;

// Retrieve saved theme setting from local storage
Enum.TryParse(await settingsService.GetSettingAsync("theme"), out Theme theme);
Console.WriteLine("Theme: " + theme);
SharedSettings.SelectedTheme = theme;

// Retrieve saved language setting from local storage
string language = await settingsService.GetSettingAsync("language");
if (!SharedSettings.Languages.Contains(language)) {
    language = SharedSettings.Languages[0];
}
SharedSettings.SelectedLanguage = language;
int index = SharedSettings.Languages.IndexOf(language);

// Retrieve saved obfuscation setting from local storage
bool tryParse = int.TryParse(await settingsService.GetSettingAsync("obfuscation"), out int obfuscate);
if (!tryParse) {
    obfuscate = 0;
}
SharedSettings.Obfuscation = obfuscate;

// Retrieve saved guess_history_limit setting from local storage
tryParse = int.TryParse(await settingsService.GetSettingAsync("guess_history_limit"), out int guessHistoryLimit);
if (!tryParse) {
    guessHistoryLimit = 0;
}
SharedSettings.GuessHistoryLimit = guessHistoryLimit;

// Retrieve saved allow_duplicate_guesses setting from local storage
tryParse = bool.TryParse(await settingsService.GetSettingAsync("allow_duplicate_guesses"), out bool allowDuplicateGuesses);
if (!tryParse) {
    allowDuplicateGuesses = false;
}
SharedSettings.AllowDuplicateGuesses = allowDuplicateGuesses;

// Initialize translations
await Translations.Initialize(httpClient, pokeApiClient,index + 1);

// Start application
await host.RunAsync();
