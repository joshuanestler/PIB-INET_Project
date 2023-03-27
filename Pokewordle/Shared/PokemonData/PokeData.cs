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
        public IImmutableList<string> FilledTypes { get; }

        //public readonly int Generation;

        public float Height_m { get; }
        public float Weight_kg { get; }
        //public int EvolutionType { get; set; }

        public IImmutableList<string> Abilities { get; }

        public PokeData(Pokemon pokemon)
        {
            Name = pokemon.Name;
            Types = pokemon.Types.ConvertAll(type => type.Type.Name).ToImmutableList();
            FilledTypes = PokemonDataHelper.BuildTypeList(Types, 2);
            //Generation = pokemon.
            Height_m = pokemon.Height / 10f;
            Weight_kg = pokemon.Weight / 10f;
            Abilities = pokemon.Abilities.ConvertAll(pokemonAbility => pokemonAbility.Ability.Name).ToImmutableList();
            PokeApiClient pokeApiClient = new();
        }

    }
}
