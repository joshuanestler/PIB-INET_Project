using Microsoft.VisualBasic.FileIO;
using System.Collections.Concurrent;

namespace Pokewordle.Shared.Util
{
    public static class Translations
    {
        private const string POKEMON_NAMES_FILE = @"";
        private const string CSV_DELIMITERS = ",";
        private static readonly ConcurrentDictionary<string, string> s_PokemonNameTranslated = new();

        public static void LoadLanguage(int languageColumn)
        {
            lock(s_PokemonNameTranslated)
            {
                s_PokemonNameTranslated.Clear();
                //Add names based on column in csv file
                using TextFieldParser parser = new(POKEMON_NAMES_FILE);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(CSV_DELIMITERS);
                while (!parser.EndOfData)
                {
                    //Processing row
                    if (parser.ReadFields() is string[] fields)
                    {
                        s_PokemonNameTranslated[fields[0]] = fields[languageColumn];
                    }
                }

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
