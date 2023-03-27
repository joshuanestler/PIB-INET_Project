using PokeApiNet;
using System.Collections.Immutable;

namespace Pokewordle.Shared.PokemonData
{
    public class PokemonDataHelper
    {
        public static IImmutableList<string> BuildTypeList(Pokemon apiPokemon, int minimumListCount, string fillerTypeName = "none")
        {
            List<string> typeList = apiPokemon.Types.ConvertAll(pkmnType => pkmnType.Type.Name);
            while (typeList.Count < minimumListCount)
            {
                typeList.Add(fillerTypeName);
            }
            return typeList.ToImmutableList();
        }

    }
}
