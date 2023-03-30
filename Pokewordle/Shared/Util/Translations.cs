﻿using Microsoft.VisualBasic.FileIO;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using PokeApiNet;

namespace Pokewordle.Shared.Util
{
    public static class Translations
    {
        private const string POKEMON_NAMES_FILE = @"Resources/LanguagePokemonNames.csv";
        private const string CSV_DELIMITERS = ",";
        private static readonly Dictionary<string, string> s_BaseNameToTranslatedName = new();
        private static readonly Dictionary<string, string> s_TranslatedNameToBaseName = new();
        private static readonly Dictionary<string, string> s_BaseNamesToLookupName = new();
        private static int loadedColumn = 0;

        
        /// <summary>
        /// Pokemons which have forms cannot be requested simply by their species name unfortunately
        /// TODO: This is a temporary workaround, find a better way to pull pokemon names to suggest for guessing.
        /// </summary>
        private static readonly List<string> _UnFixedNames = new() { "deoxys", "wormadam", "giratina", "shaymin", "basculin", "darmanitan", "tornadus", "thundurus", "landorus", "keldeo", "meloetta", "meowstic", "aegislash", "pumpkaboo", "gourgeist", "zygarde", "oricorio", "lycanroc", "wishiwashi", "minior", " mimikyu", " toxtricity", "eiscue", "indeedee", "morpeko", "urshifu", "basculegion", "enamorus" };
        private static readonly List<string> _FixedNames = new() { "deoxys-attack", "wormadam-trash", "giratina-origin", "shaymin-sky", "basculin-blue-striped", "darmanitan-standard", "tornadus-therian", "tornadus-therian", "tornadus-therian", "keledo-resolute", "meloetta-aria", "meowstic-female", "aegislash-shield", "pumpkaboo-average", "gourgeist-average", "zygarde-complete", "oricorio-baile", "lycanroc-midnight", "wishiwashi-school", "", "mimikyu-busted", "toxtricity-low-key", "eiscue-ice", "indeedee-male", "morpeko-full-belly", "urshifu-rapid-strike", "basculegion-male", "enamorus-incarnate" };

        
        public static async Task Initialize(HttpClient httpClient, PokeApiClient PokeClient, int initialLanguageColumn = 0)
        {
            // Load pokedex data
            List<string> tempPokemonNames = new List<string>();
            Pokedex pokedex = await PokeClient.GetResourceAsync<Pokedex>(1);
            foreach(PokemonEntry pokemon in pokedex.PokemonEntries)
            {
                tempPokemonNames.Add(pokemon.PokemonSpecies.Name);
            }

            Dictionary<string, string> baseNameToLookupName = new();

            List<string> tempNamesToRemove = new();
            foreach (string pokemonName in tempPokemonNames)
            {
                int index = _UnFixedNames.IndexOf(pokemonName);
                if (index >= 0)
                {
                    string? fixedPokemonName = _FixedNames[index];
                    //Only add the fixed name if a solution has been found!
                    if (fixedPokemonName is not null && !fixedPokemonName.Equals(string.Empty))
                    {
                        baseNameToLookupName.Add(pokemonName, fixedPokemonName);
                    }
                }
                else
                {
                    baseNameToLookupName.Add(pokemonName, pokemonName);
                }
            }
            tempPokemonNames.RemoveAll(name => tempNamesToRemove.Contains(name));
            

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
                    if (baseNameToLookupName.TryGetValue(baseName, out string? validName) && validName is not null)
                    {
                        s_BaseNamesToLookupName[baseName] = validName;
                    } else
                    {
                        //Console.WriteLine($"Failed to find valid name for english name {baseName}!");
                    }
                }
            }

            if (loadedColumn == 0)
            {
                await LoadLanguage(httpClient, initialLanguageColumn);
            }
        }



        public static async Task LoadLanguage(HttpClient httpClient, int languageColumn)
        {
            Console.WriteLine($"Attempting to fetch language file '{POKEMON_NAMES_FILE}' with lang column {languageColumn}");
            string str = await httpClient.GetStringAsync(POKEMON_NAMES_FILE);

            if (str is null)
            {
                Console.WriteLine("Failed to fetch language file");
                return;
            }

            loadedColumn = languageColumn;

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
