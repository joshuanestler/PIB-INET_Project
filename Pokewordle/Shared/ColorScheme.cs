using System.Drawing;

namespace Pokewordle.Shared
{
    public static class ColorScheme
    {
       public static Color COLOR_CORRECT { get; private set; } = Color.FromArgb(60, 204, 52);
       public static Color COLOR_MISTAKE { get; private set; } = Color.FromArgb(255, 30, 0);
    }
}
