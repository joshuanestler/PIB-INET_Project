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
        public static bool IsType1Shared(this IPokeData pokeData, IPokeData compareData, out string type1)
        {
            //TODO: consider always adding none at least once to Types list in IPokeData to get rid of the if
            if (pokeData.Types.Count < 1 || compareData.Types.Count < 1)
            {
                type1 = "ERROR: No types assigned to pokemon!";
                return false;
            }

            type1 = pokeData.Types[0];
            return pokeData.Types[0].Equals(compareData.Types[0]);
        }
        public static bool IsType2Shared(this IPokeData pokeData, IPokeData compareData, out string type2)
        {
            //TODO: consider always adding none twice to Types list in IPokeData to get rid of the ifs
            if (compareData.Types.Count < 2)
            {
                if (pokeData.Types.Count >= 2)
                {
                    type2 = pokeData.Types[1];
                    return false;
                } else
                {
                    type2 = "none";
                    return true;
                }
            }

            if (pokeData.Types.Count < 2)
            {
                type2 = "none";
                return false;
            }

            type2 = pokeData.Types[1];
            return pokeData.Types[1].Equals(compareData.Types[1]);
        }

        public static MatchingResult MatchTypes(this IPokeData pokeData, IPokeData compareData)
        {
            int matchCount = 0;
            foreach (string type in pokeData.Types)
            {
                if (compareData.Types.Contains(type))
                {
                    matchCount++;
                }
            }
            if (pokeData.Types.Count == compareData.Types.Count)
            {
                if (matchCount == pokeData.Types.Count)
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
