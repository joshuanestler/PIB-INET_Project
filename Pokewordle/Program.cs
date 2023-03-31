using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;
using PokeApiNet;
using Pokewordle;
using Pokewordle.Shared;
using Pokewordle.Shared.Util;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

IJSRuntime jsRuntime = builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
HttpClient httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
PokeApiClient pokeApiClient = new PokeApiClient();

builder.Services.AddSingleton<SharedSettings>(await SharedSettings.InitializeSettings(jsRuntime, pokeApiClient, httpClient));

builder.Services.AddScoped(sp => httpClient);
builder.Services.AddScoped(sp => pokeApiClient);
builder.Services.AddScoped(sp => new PokemonList());
builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
