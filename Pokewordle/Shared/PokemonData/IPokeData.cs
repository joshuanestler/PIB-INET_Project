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

        float Height_m { get; }
        float Weight_kg { get; }
        //public int EvolutionType { get; set; }
        string SpriteUrl { get; }

        IImmutableList<string> Abilities { get; }

        int HP { get; }
        int Atk { get; }
        int Def { get; }
        int SpA { get; }
        int SpD { get; }
        int Spe { get; }



        Task<int> GetGeneration();
    }
}
