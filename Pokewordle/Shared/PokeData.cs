using PokeApiNet;
using System.Collections.Immutable;
using System.Drawing;

namespace Pokewordle.Shared
{
    public class PokeData
    {
        public readonly string Name;

        public readonly string? Type1;

        public readonly string? Type2;

        //public readonly int Generation;

        public readonly int Height_m;
        public readonly int Weight_kg;
        //public int EvolutionType { get; set; }

        public readonly IImmutableList<string> Abilities;

        public PokeData(Pokemon pokemon)
        {
            Name = pokemon.Name;
            Type1 = 0 < pokemon.Types.Count ? pokemon.Types[0].Type.Name : "";
            Type2 = 1 < pokemon.Types.Count ? pokemon.Types[1].Type.Name : "";
            //Generation = pokemon.
            Height_m = pokemon.Height;
            Weight_kg = pokemon.Weight;
            Abilities = pokemon.Abilities.ConvertAll(pokemonAbility => pokemonAbility.Ability.Name).ToImmutableList();
        }

    }
}
