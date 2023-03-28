using System.Collections.Concurrent;

namespace Pokewordle.Shared.Util
{
    public static class Translations
    {
        private static readonly ConcurrentDictionary<string, string> s_PokemonNameTranslated;

        public static void LoadLanguage()
        {
            lock(s_PokemonNameTranslated)
            {
                s_PokemonNameTranslated.Clear();
                //Add names based on column in csv file
            }
        }

        public static string Translate(string englishName)
        {
            if (s_PokemonNameTranslated.TryGetValue(englishName, out var pokemonName))
            {
                return pokemonName;
            }
            return englishName;
        }


    }
}
