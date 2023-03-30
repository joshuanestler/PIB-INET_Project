using Microsoft.VisualBasic.FileIO;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace Pokewordle.Shared.Util
{
    public static class Translations
    {
        private const string POKEMON_NAMES_FILE = @"Resources/LangugagePokemonNames.csv";
        private const string CSV_DELIMITERS = ",";
        private static readonly ConcurrentDictionary<string, string> s_PokemonNameTranslated = new();

        public static async void LoadLanguage(HttpClient httpClient, int languageColumn)
        {
            Console.WriteLine($"Attempting to fetch file '{POKEMON_NAMES_FILE}'");
            string str = await httpClient.GetStringAsync(POKEMON_NAMES_FILE);

            if (str is null)
            {
                Console.WriteLine("Failed to fetch language file");
                return;
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            using MemoryStream stream = new(byteArray);

            lock (s_PokemonNameTranslated)
            {
                s_PokemonNameTranslated.Clear();
                //Add names based on column in csv file
                using TextFieldParser parser = new(stream);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(CSV_DELIMITERS);
                while (!parser.EndOfData)
                {
                    //Processing row
                    if (parser.ReadFields() is string[] fields)
                    {
                        s_PokemonNameTranslated[fields[0]] = fields[languageColumn];
                        Console.WriteLine($"Read {fields[0]} translating to '{fields[languageColumn]}'");
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
