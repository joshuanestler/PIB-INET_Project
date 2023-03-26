using PokeApiNet;
using System.Collections.Immutable;
using System.Drawing;

namespace Pokewordle.Shared
{
    public class PokeData
    {
        public readonly string Name;

        public readonly ImmutableHashSet<string> Types;

        //public readonly int Generation;

        public readonly int Height_m;
        public readonly int Weight_kg;
        //public int EvolutionType { get; set; }

        public readonly IImmutableList<string> Abilities;

        public PokeData(Pokemon pokemon)
        {
            Name = pokemon.Name;
            Types = pokemon.Types.ConvertAll(pkmnType => pkmnType.Type.Name).ToImmutableHashSet();
            //Generation = pokemon.
            Height_m = pokemon.Height;
            Weight_kg = pokemon.Weight;
            Abilities = pokemon.Abilities.ConvertAll(pokemonAbility => pokemonAbility.Ability.Name).ToImmutableList();
        }

        public IList<string> FindSharedTypes(PokeData pokeData, out IList<string> nonSharedTypes)
        {
            List<string> result = new();
            nonSharedTypes = new List<string>();

            foreach(string type in Types)
            {
                if (pokeData.Types.Contains(type))
                {
                    result.Add(type);
                } else
                {
                    nonSharedTypes.Add(type);
                }
            }
            //prevent issues in case a pokemon is actually typeless (is currently only possible with very nieche moves and only during combat but eh now its fixed either way)
            nonSharedTypes.Add("none");
            nonSharedTypes.Add("none");
            return result;
        }
    }
}
