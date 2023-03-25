using System.Drawing;

namespace Pokewordle.Shared.HtmlUtil
{
    public static class Convert
    {
        public static string ColorToHexString(Color? color)
        {
            if (color is null)
            {
                return "#AAAAAA";
            }
            return ColorToHexString(color.Value);
        }
        public static string ColorToHexString(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

    }
}
