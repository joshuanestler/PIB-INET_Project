using System.Drawing;

namespace Pokewordle.Shared
{
    public static class ColorScheme
    {
        public static Color COLOR_CORRECT { get; private set; } = Color.FromArgb(0, 128, 36);
        public static Color COLOR_MISTAKE { get; private set; } = Color.FromArgb(128, 0, 19);
 
        public static Color COLOR_SEMI_CORRECT_MISTAKE { get; private set; } = Color.FromArgb(191, 153, 3);

        public static Color COLOR_TYPE_NOT_FOUND { get; private set; } = Color.FromArgb(170, 170, 170);

        private static readonly Dictionary<string, Color> typeColors = new()
            {
                { "normal", Color.FromArgb(168, 168, 120) },
                { "grass", Color.FromArgb(120, 200, 80) },
                { "water", Color.FromArgb(104, 144, 240) },
                { "fire", Color.FromArgb(240, 128, 48) },
                { "electric", Color.FromArgb(248, 208, 48) },
                { "ground", Color.FromArgb(224, 192, 104) },
                { "flying", Color.FromArgb(168, 144, 240) },
                { "rock", Color.FromArgb(184, 160, 56) },
                { "ice", Color.FromArgb(152, 216, 216) },
                { "fighting", Color.FromArgb(192, 48, 40) },
                { "psychic", Color.FromArgb(248, 88, 136) },
                { "ghost", Color.FromArgb(112, 88, 152) },
                { "bug", Color.FromArgb(168, 184, 32) },
                { "dark", Color.FromArgb(112, 88, 72) },
                { "dragon", Color.FromArgb(112, 56, 248) },
                { "steel", Color.FromArgb(184, 184, 208) },
                { "fairy", Color.FromArgb(238, 153, 172) },
                { "poison", Color.FromArgb(160, 64, 160) }
            };
        public static Color? TypeNameToColor(string typeName)
        {
            return typeColors.TryGetValue(typeName.ToLower(), out Color value) ? value : null;
        }
    }

}
