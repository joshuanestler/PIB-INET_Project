using PokeApiNet;
using System.Collections.Immutable;
using System.Text;

namespace Pokewordle.Shared.PokemonData
{
    public interface IPokeData
    {
        string Name { get; }

        IImmutableList<string> Types { get; }
        //ALWAYS CONTAINS AT LEAST 2 VALUES
        IImmutableList<string> FilledTypes { get; }

        //Generation: Pokemon -> Species -> Generation

        float Height_m { get; }
        float Weight_kg { get; }
        //public int EvolutionType { get; set; }

        IImmutableList<string> Abilities { get; }

        Task<int> GetGeneration();
    }
}
