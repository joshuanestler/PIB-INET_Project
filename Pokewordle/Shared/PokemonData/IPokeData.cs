using PokeApiNet;
using System.Collections.Immutable;
using System.Text;

namespace Pokewordle.Shared.PokemonData
{
    public interface IPokeData
    {
        string Name { get; }

        IImmutableList<string> Types { get; }

        //public readonly int Generation;

        int Height_m { get; }
        int Weight_kg { get; }
        //public int EvolutionType { get; set; }

        IImmutableList<string> Abilities { get; }


        IList<string> FindSharedTypes(PokeData pokeData, out IList<string> nonSharedTypes);
        MatchingResult MatchTypes(PokeData pokeData);
        string GetDisplayTypesString();

    }
}
