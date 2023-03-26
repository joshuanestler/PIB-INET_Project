using PokeApiNet;
using System.Collections.Immutable;
using System.Drawing;
using System.Text;

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
            List<string> sharedTypes = new();
            nonSharedTypes = new List<string>();

            foreach(string type in Types)
            {
                if (pokeData.Types.Contains(type))
                {
                    sharedTypes.Add(type);
                } else
                {
                    nonSharedTypes.Add(type);
                }
            }

            if (pokeData.Types.Count == 1 && this.Types.Count == 1)
            {
                sharedTypes.Add("none");
            }

            //prevent issues in case a pokemon is actually typeless (is currently only possible with very nieche moves and only during combat but eh now its fixed either way)
            nonSharedTypes.Add("none");
            nonSharedTypes.Add("none");
            return sharedTypes;
        }

        public MatchingResult MatchTypes(PokeData pokeData)
        {
            int matchCount = 0;
            foreach (string type in Types)
            {
                if (pokeData.Types.Contains(type))
                {
                    matchCount++;
                }
            }
            if (this.Types.Count == pokeData.Types.Count)
            {
                if (matchCount == this.Types.Count)
                {
                    return MatchingResult.ALL;
                } else if (matchCount > 0)
                {
                    return MatchingResult.PARTIAL;
                } else
                {
                    return MatchingResult.NONE;
                }
            } else
            {
                if (matchCount > 0)
                {
                    return MatchingResult.PARTIAL;
                }
                else
                {
                    return MatchingResult.NONE;
                }
            }
        }

        public string GetDisplayTypesString()
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach(string typeName in Types)
            {
                if (!first)
                {
                    sb.AppendLine();
                }
                sb.Append(typeName);
                first = false;
            }
            return sb.ToString();
        }
    }
}
