using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PokeApiNet;
using System.Text;

namespace Pokewordle.Shared.Util;

public class SharedSettings {
    private static readonly List<ColumnType> DefaultColumnTypes = new() 
        { ColumnType.SPRITE, ColumnType.NAME_LOCAL, ColumnType.GENERATION, ColumnType.TYPES, ColumnType.ABILITIES, ColumnType.MAXSTATS, ColumnType.HEIGHT };

    public static readonly List<string> Languages = new()
        { "english", "japanese", "french", "german", "spanish", "italian", "korean", "chinese" };
    
    private readonly IJSRuntime _jsRuntime;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    
    private Theme _selectedTheme = Theme.Forest;
    private ISet<int> _enabledGenerations = new HashSet<int>()
    {
        1, 2, 3, 4, 5, 6, 7, 8, 9,
    };
    private string _selectedLanguage = Languages[0];
    private int _numberOfObfuscatedFields = 0;
    private int _guessHistoryLimit = 0;
    private bool _allowDuplicateGuesses = false;
    private List<ColumnType> _selectedColumnTypes = DefaultColumnTypes.ToList();

    public SharedSettings(IJSRuntime jsRuntime, HttpClient httpClient, NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    public static async Task<SharedSettings> InitializeSettings(IJSRuntime jsRuntime, PokeApiClient pokeApiClient, 
        HttpClient httpClient, NavigationManager navigationManager) {

        SharedSettings settings = new SharedSettings(jsRuntime, httpClient, navigationManager);
        
        settings._selectedTheme = ThemeUtils.FromString(await settings.GetSettingAsync("theme"));
        
        string? language = await settings.GetSettingAsync("language");
        if (language is not null && Languages.Contains(language)) {
            settings._selectedLanguage = language;
        }
        
        if (int.TryParse(await settings.GetSettingAsync("obfuscatedFields"), out int obfuscatedFields) && obfuscatedFields > 0) {
            settings._numberOfObfuscatedFields = obfuscatedFields;
        }

        if (int.TryParse(await settings.GetSettingAsync("guess_history_limit"), out int guessHistoryLimit) && guessHistoryLimit > 0) {
            settings._guessHistoryLimit = guessHistoryLimit;
        }
        
        if (bool.TryParse(await settings.GetSettingAsync("allow_duplicate_guesses"), out bool allowDuplicateGuesses)) {
            settings._allowDuplicateGuesses = allowDuplicateGuesses;
        }

        string? enabledGenerations = await settings.GetSettingAsync("enabled_generations");
        if (enabledGenerations is not null)
        {
            ISet<int> enabledGenerationList = new HashSet<int>();
            string[] generationNumbers = enabledGenerations.Split(',');
            foreach(string generationNumber in generationNumbers)
            {
                if (int.TryParse(generationNumber, out int parsedGeneration))
                {
                    enabledGenerationList.Add(parsedGeneration);
                }
            }
            settings._enabledGenerations = enabledGenerationList;
        }

        string? columnTypes = await settings.GetSettingAsync("columnTypes");
        if (columnTypes is not null && TryDeserializeColumnTypes(columnTypes, out List<ColumnType> columnTypesParsed)) {
            settings._selectedColumnTypes = columnTypesParsed;
        }
        
        // Initialize translations
        
        int index = Languages.IndexOf(settings._selectedLanguage);
        await Translations.Initialize(httpClient, pokeApiClient,index + 1);

        return settings;
    }

    public Theme SelectedTheme {
        get => _selectedTheme;
        set {
            _selectedTheme = value;
            SetSettingAsync("theme", value.ToString()).ConfigureAwait(false);
            _navigationManager.NavigateTo(_navigationManager.Uri, false);
        } 
    }


    public string SelectedLanguage {
        get => _selectedLanguage;
        set {
            _selectedLanguage = value;
            SetSettingAsync("language", value).ConfigureAwait(false);
            Translations.LoadLanguage(_httpClient, Languages.IndexOf(value) + 1);
        } 
    }


    public int NumberOfObfuscatedFields {
        get => _numberOfObfuscatedFields;
        set {
            if (value < 0) {
                value = 0;
            }
            _numberOfObfuscatedFields = value;
            SetSettingAsync("obfuscatedFields", value.ToString()).ConfigureAwait(false);
        }
    }


    public int GuessHistoryLimit {
        get => _guessHistoryLimit;
        set {
            if (value < 0) {
                value = 0;
            }
            _guessHistoryLimit = value;
            SetSettingAsync("guess_history_limit", value.ToString()).ConfigureAwait(false);
        }
    }


    public bool AllowDuplicateGuesses {
        get => _allowDuplicateGuesses;
        set {
            _allowDuplicateGuesses = value;
            SetSettingAsync("allow_duplicate_guesses", value.ToString()).ConfigureAwait(false);
        }
    }
    
    public List<ColumnType> SelectedColumnTypes {
        get => _selectedColumnTypes.ToList();
        set {
            _selectedColumnTypes = value.ToList();
            SetSettingAsync("columnTypes", SerializeColumnTypes(value)).ConfigureAwait(false);
        }
    }

    public ISet<int> EnabledGenerations
    {
        get => _enabledGenerations.ToHashSet();
        set
        {
            _enabledGenerations = value.ToHashSet();
            SetSettingAsync("enabled_generations", value.Aggregate(new StringBuilder(), (StringBuilder sb, int i) => sb.Append($"{i},")).ToString()).ConfigureAwait(false);
        }
    }

    private async Task<string?> GetSettingAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }
    
    // Get setting from local storage synchronously
    private string? GetSetting(string key) {
        return _jsRuntime.InvokeAsync<string>("localStorage.getItem", key).GetAwaiter().GetResult();
    }

    private async Task SetSettingAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }
    
    // Convert list to string
    private static string SerializeColumnTypes(IEnumerable<ColumnType> list) {
        return string.Join(";", list);
    }
    
    // Deserialize string to ColumnType list
    private static bool TryDeserializeColumnTypes(string serialized, out List<ColumnType> columnTypes) {
        columnTypes = new();
        foreach (string str in serialized.Split(';')) {
            if (ColumnTypeUtils.TryParseString(str, out ColumnType columnType)) {
                columnTypes.Add(columnType);
            }
        }
        return columnTypes.Count > 0;
    }
}