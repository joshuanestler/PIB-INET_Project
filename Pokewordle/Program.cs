using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;
using PokeApiNet;
using Pokewordle;
using Pokewordle.Shared;
using Pokewordle.Shared.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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

await builder.Build().RunAsync();
