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

        public static MatchingResult MatchTypes(this IPokeData pokeData, PokeData compareData)
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
