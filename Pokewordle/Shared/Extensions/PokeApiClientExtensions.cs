using PokeApiNet;

namespace Pokewordle.Shared.Extensions
{
    public static class PokeApiClientExtensions
    {
        public static async Task<PokemonSpecies> GetPokemonSpecies(this PokeApiClient client, Pokemon pokemon)
        {
            return await client.GetResourceAsync<PokemonSpecies>(pokemon.Species);
        }

        public static async Task<Generation> GetGeneration(this PokeApiClient client, Pokemon pokemon)
        {
            PokemonSpecies pokemonSpecies = await client.GetPokemonSpecies(pokemon);
            return await client.GetResourceAsync<Generation>(pokemonSpecies.Generation);
        }

    }
}
