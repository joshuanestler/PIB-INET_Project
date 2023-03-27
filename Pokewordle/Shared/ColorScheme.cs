using System.Drawing;

namespace Pokewordle.Shared
{
    public static class ColorScheme
    {
        public static Color COLOR_CORRECT { get; private set; } = Color.FromArgb(60, 204, 52);
        public static Color COLOR_MISTAKE { get; private set; } = Color.FromArgb(255, 30, 0);
        public static Color COLOR_SEMI_CORRECT_MISTAKE { get; private set; } = Color.Yellow;


        public static Color COLOR_TYPE_NOT_FOUND { get; private set; } = Color.FromArgb(170, 170, 170);

        private static readonly Dictionary<string, Color> typeColors = new()
            {
                { "normal", Color.Beige },
                { "grass", Color.LawnGreen },
                { "water", Color.Aqua },
                { "fire", Color.OrangeRed },
                { "electric", Color.Yellow },
                { "ground", Color.SandyBrown },
                { "flying", Color.SkyBlue },
                { "rock", Color.SaddleBrown },
                { "ice", Color.AliceBlue },
                { "fighting", Color.DarkRed },
                { "psychic", Color.Pink },
                { "ghost", Color.Purple },
                { "bug", Color.Lime },
                { "dark", Color.Black },
                { "dragon", Color.DarkBlue },
                { "steel", Color.Silver },
                { "fairy", Color.Pink },
                { "poison", Color.Purple }
            };
        public static Color? TypeNameToColor(string typeName)
        {
            return typeColors.TryGetValue(typeName.ToLower(), out Color value) ? value : null;
        }
    }

}
