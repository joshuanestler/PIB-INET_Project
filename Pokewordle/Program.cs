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

SettingsService settingsService = host.Services.GetService<SettingsService>();

if (settingsService is null) {
    Console.WriteLine("Settings service is null!");
}

Enum.TryParse(await settingsService.GetSettingAsync("theme"), out Theme theme);
Console.WriteLine("Theme: " + theme);
SharedSettings.SelectedTheme = theme;

string language = await settingsService.GetSettingAsync("language");
if (!SharedSettings.Languages.Contains(language)) {
    language = SharedSettings.Languages[0];
}
SharedSettings.SelectedLanguage = language;
int index = SharedSettings.Languages.IndexOf(language);


await Translations.Initialize(httpClient, pokeApiClient,index + 1);

await host.RunAsync();
