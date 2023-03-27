using PokeApiNet;
using System.Collections.Immutable;
using System.Drawing;
using System.Text;

namespace Pokewordle.Shared.PokemonData
{
    public class PokeData : IPokeData
    {
        public string Name { get; }

        public IImmutableList<string> Types { get; }

        //public readonly int Generation;

        public int Height_m { get; }
        public int Weight_kg { get; }
        //public int EvolutionType { get; set; }

        public IImmutableList<string> Abilities { get; }

        public PokeData(Pokemon pokemon)
        {
            Name = pokemon.Name;
            Types = pokemon.Types.ConvertAll(pkmnType => pkmnType.Type.Name).ToImmutableList();
            //Generation = pokemon.
            Height_m = pokemon.Height;
            Weight_kg = pokemon.Weight;
            Abilities = pokemon.Abilities.ConvertAll(pokemonAbility => pokemonAbility.Ability.Name).ToImmutableList();
        }

    }
}
