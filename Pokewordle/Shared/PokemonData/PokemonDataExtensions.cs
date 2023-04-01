using PokeApiNet;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pokewordle.Shared.PokemonData
{
    public static class PokemonDataExtensions
    {

        public static IList<string> FindSharedTypes(this IPokeData pokeData, IPokeData compareData, out IList<string> nonSharedTypes)
        {
            List<string> sharedTypes = new();
            nonSharedTypes = new List<string>();

            foreach (string type in pokeData.Types)
            {
                if (compareData.Types.Contains(type))
                {
                    sharedTypes.Add(type);
                }
                else
                {
                    nonSharedTypes.Add(type);
                }
            }

            if (compareData.Types.Count == 1 && pokeData.Types.Count == 1)
            {
                sharedTypes.Add("none");
            }

            //prevent issues in case a pokemon is actually typeless (is currently only possible with very nieche moves and only during combat but eh now its fixed either way)
            nonSharedTypes.Add("none");
            nonSharedTypes.Add("none");
            return sharedTypes;
        }

        public static List<string> FindSharedAbilities(this IPokeData pokeData, IPokeData compareData)
        {
            List<string> sharedTypes = new();

            foreach (string type in pokeData.Abilities)
            {
                if (compareData.Types.Contains(type))
                {
                    sharedTypes.Add(type);
                }
            }

            return sharedTypes;
        }

        public static bool IsType1Shared(this IPokeData pokeData, IPokeData compareData, out string type1)
        {
            type1 = pokeData.FilledTypes[0];
            return compareData.FilledTypes.Contains(type1);
        }
        public static bool IsType2Shared(this IPokeData pokeData, IPokeData compareData, out string type2)
        {
            type2 = pokeData.FilledTypes[1];
            return compareData.FilledTypes.Contains(type2);
        }

        public static List<string> GetWeaknesses(this IPokeData pokeData)
        {
            if (pokeData.Types.Count == 1)
            {
                return PokeTypeHelper.Weaknesses[pokeData.FilledTypes[0]].ToList();
            }
            return PokeTypeHelper.GetWeaknesses(pokeData.FilledTypes[0], pokeData.FilledTypes[1]);
        }

        public static List<string> GetEffectiveTypes(this IPokeData pokeData)
        {
            if (pokeData.Types.Count == 1)
            {
                return PokeTypeHelper.Effectives[pokeData.FilledTypes[0]].ToList();
            }
            return PokeTypeHelper.GetEffectives(pokeData.FilledTypes[0], pokeData.FilledTypes[1]);
        }

        public static List<string> GetResistances(this IPokeData pokeData)
        {
            if (pokeData.Types.Count == 1)
            {
                return PokeTypeHelper.Resistances[pokeData.FilledTypes[0]].ToList();
            }
            return PokeTypeHelper.GetResistances(pokeData.FilledTypes[0], pokeData.FilledTypes[1]);
        }

        public static List<string> GetImmunities(this IPokeData pokeData)
        {
            if (pokeData.Types.Count == 1)
            {
                return PokeTypeHelper.Immunities[pokeData.FilledTypes[0]].ToList();
            }
            return PokeTypeHelper.GetImmunities(pokeData.FilledTypes[0], pokeData.FilledTypes[1]);
        }

        public static List<string> GetResistancesAndImmunities(this IPokeData pokeData)
        {
            if (pokeData.Types.Count == 1)
            {
                return PokeTypeHelper.ResistancesAndImmunities[pokeData.FilledTypes[0]].ToList();
            }
            return PokeTypeHelper.GetResistancesAndImmunities(pokeData.FilledTypes[0], pokeData.FilledTypes[1]);
        }

        public static MatchingResult Match<T>(this IPokeData pokeData, IPokeData compareData, Func<IPokeData, IEnumerable<T>> valuePicker)
        {
            IList<T> pokeValues = valuePicker.Invoke(pokeData).ToList();
            IList<T> compareValues = valuePicker.Invoke(compareData).ToList();
            int matchCount = 0;
            foreach (T type in pokeValues)
            {
                if (compareValues.Contains(type))
                {
                    matchCount++;
                }
            }
            if (pokeValues.Count == compareValues.Count)
            {
                if (matchCount == pokeValues.Count)
                {
                    return MatchingResult.ALL;
                }
                else if (matchCount > 0)
                {
                    return MatchingResult.PARTIAL;
                }
                else
                {
                    return MatchingResult.NONE;
                }
            }
            else
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

        public static string GetDisplayTypesString(this IPokeData pokeData)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (string typeName in pokeData.Types)
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
