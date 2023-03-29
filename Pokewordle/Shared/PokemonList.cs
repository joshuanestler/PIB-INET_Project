using PokeApiNet;

namespace Pokewordle.Shared;

public class PokemonList {
    private readonly PokeApiClient client;
    
    public List<string> PokemonNames { get; }
    
    public PokemonList() {
        client = new PokeApiClient();
        PokemonNames = new List<string>();
        client.GetResourceAsync<Pokedex>(0).Result.PokemonEntries.ForEach(pokemon => PokemonNames.Add(pokemon.PokemonSpecies.Name));
    }
    
    public List<string> FindPokemonBySubstring(string name) {
        return PokemonNames.FindAll(pokemon => pokemon.StartsWith(name));
    }
}