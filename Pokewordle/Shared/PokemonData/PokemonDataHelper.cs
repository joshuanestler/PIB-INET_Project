using PokeApiNet;
using System.Collections.Immutable;

namespace Pokewordle.Shared.PokemonData
{
    public class PokemonDataHelper
    {
        public static IImmutableList<string> BuildTypeList(IEnumerable<string> typeNames, int minimumListCount, string fillerTypeName = "none")
        {
            List<string> typeList = new(typeNames);
            while (typeList.Count < minimumListCount)
            {
                typeList.Add(fillerTypeName);
            }
            return typeList.ToImmutableList();
        }

    }
}
