using PokeApiNet;

namespace Pokewordle.Shared;

public class PokemonList {
    private readonly PokeApiClient client;
    
    public List<string> PokemonNames { get; }
    
    public PokemonList() : this(new PokeApiClient()) { }

    public PokemonList(PokeApiClient pokeApiClient) {
        client = pokeApiClient;
        PokemonNames = new List<string>();
        client.GetResourceAsync<Pokedex>(0).Result.PokemonEntries.ForEach(pokemon => PokemonNames.Add(pokemon.PokemonSpecies.Name));
    }
    
    public List<string> FindPokemonBySubstring(string name) {
        return PokemonNames.FindAll(pokemon => pokemon.StartsWith(name));
    }
}
