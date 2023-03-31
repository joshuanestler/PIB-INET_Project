using System.Drawing;

namespace Pokewordle.Shared.HtmlUtil
{
    public static class Convert
    {
        public static string ColorToHexString(Color? color, Color colorIfNull)
        {
            if (color is null)
            {
                return ColorToHexString(colorIfNull);
            }
            return ColorToHexString(color.Value);
        }
        public static string ColorToHexString(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

    }
}
