using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;
using PokeApiNet;
using Pokewordle;
using Pokewordle.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

HttpClient httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
//BRUH NOT ALLOWED CLIENT SIDE httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
PokeApiClient pokeApiClient = new PokeApiClient();

builder.Services.AddScoped(sp => httpClient);
builder.Services.AddScoped(sp => pokeApiClient);
builder.Services.AddScoped(sp => new PokemonList());
builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
