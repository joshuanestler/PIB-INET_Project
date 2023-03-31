namespace Pokewordle.Shared.Util;

public enum Theme
{
    Forest, Vaporwave, Beach, Melon, Ocean, Sand, Peach, Hal9000
}

public static class ThemeUtils
{
    public static Theme FromString(string? str) {
        if (str is null) {
            return Theme.Forest;
        }
        
        bool tryParse = Enum.TryParse<Theme>(str, out Theme theme);
        if (!tryParse) {
            theme = Theme.Forest;
        }
        return theme;
    }
}