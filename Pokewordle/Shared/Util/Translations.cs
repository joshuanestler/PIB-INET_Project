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
        private static readonly Dictionary<string, string> s_BaseNameToTranslatedName = new();
        private static readonly Dictionary<string, string> s_TranslatedNameToBaseName = new();
        private static readonly Dictionary<string, string> s_BaseNamesToLookupName = new();

        public static async Task Initialize(HttpClient httpClient, IDictionary<string, string> baseNameToValidName)
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

            s_BaseNamesToLookupName.Clear();
            //Add names based on column in csv file
            using TextFieldParser parser = new(stream);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(CSV_DELIMITERS);
            while (!parser.EndOfData)
            {
                //Processing row
                if (parser.ReadFields() is string[] fields)
                {
                    string baseName = fields[1].ToLower()
                        .Replace("'", "")
                        .Replace(".", "")
                        .Replace(":", "")
                        .Replace('é', 'e')
                        .Replace(' ', '-');
                    if (baseNameToValidName.TryGetValue(baseName, out string validName))
                    {
                        s_BaseNamesToLookupName[baseName] = validName;
                    } else
                    {
                        //Console.WriteLine($"Failed to find valid name for english name {baseName}!");
                    }
                }
            }
        }



        public static async Task LoadLanguage(HttpClient httpClient, int languageColumn)
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

            s_BaseNameToTranslatedName.Clear();
            s_TranslatedNameToBaseName.Clear();
            //Add names based on column in csv file
            using TextFieldParser parser = new(stream);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(CSV_DELIMITERS);
            while (!parser.EndOfData)
            {
                //Processing row
                if (parser.ReadFields() is string[] fields)
                {
                    string baseName = fields[1].ToLower()
                        .Replace("'", "")
                        .Replace(".", "")
                        .Replace(":", "")
                        .Replace('é', 'e')
                        .Replace(' ', '-');
                    if (s_BaseNamesToLookupName.ContainsKey(baseName))
                    {
                        string translatedName = fields[languageColumn];
                        s_BaseNameToTranslatedName[baseName] = translatedName;
                        s_TranslatedNameToBaseName[translatedName] = baseName;
                    }
                    //Console.WriteLine($"Read {fields[0]} translating to '{fields[languageColumn]}'");
                }
            }
        }
        
        public static string GetRandomLookupName()
        {
            Random random = new();
            return s_BaseNamesToLookupName.Values.ElementAt(random.Next(s_BaseNamesToLookupName.Count));
        }

        public static IList<string> GetTranslatedNames()
        {
            return s_BaseNameToTranslatedName.Values.ToList();
        }

        public static string TranslateToSelectedLanguage(string englishName)
        {
            if (s_BaseNamesToLookupName.TryGetValue(englishName, out var pokemonName))
            {
                return pokemonName;
            }
            return englishName;
        }

        public static string TranslateToEnglish(string translatedName)
        {
            if (s_TranslatedNameToBaseName.TryGetValue(translatedName, out var englishName))
            {
                return englishName;
            }
            throw new ArgumentException();
        }

        public static string GetLookupName(string englishName)
        {
            if (s_BaseNamesToLookupName.TryGetValue(englishName, out var lookupName))
            {
                return lookupName;
            }
            throw new ArgumentException();
        }
    }
}
