using System.Drawing;
using System.Text;

namespace Pokewordle.Shared.HtmlUtil
{
    public class GradientTableCell : ITableCell
    {
        public readonly string DisplayString;
        public readonly string FontColor;
        public readonly string Degrees;
        public readonly string Background1;
        public readonly string Background2;
        public readonly string HtmlClass;
        public readonly string HtmlId;

        public GradientTableCell(
            string displayString,
            Color background1, Color background2,
            int degrees = 0, Color? fontColor = null,
            string htmlClass = "", string htmlId = "")
        {
            DisplayString = displayString;
            FontColor = Convert.ColorToHexString(fontColor, ColorScheme.COLOR_TYPE_NOT_FOUND);
            Degrees = degrees.ToString();
            Background1 = Convert.ColorToHexString(background1);
            Background2 = Convert.ColorToHexString(background2);
            HtmlClass = htmlClass;
            HtmlId = htmlId;
        }

        private static int PercentualOffset(int baseValue, int offsetValue, int offsetPercent)
        {
            double diff = offsetValue - baseValue;
            return baseValue + (int)Math.Round(diff / 100 * offsetPercent);
        }

        private static Color PercentualColorOffset(Color baseColor, Color offsetColor, int offsetPercent)
        {
            int r = PercentualOffset(baseColor.R, offsetColor.R, offsetPercent);
            int g = PercentualOffset(baseColor.G, offsetColor.G, offsetPercent);
            int b = PercentualOffset(baseColor.B, offsetColor.B, offsetPercent);
            return Color.FromArgb(r, g, b);
        }

        private static int AsPercentLimit100(int value, int value100Percent)
        {
            if (value >= value100Percent)
            {
                return 100;
            }

            return (int)Math.Round((100d / (double)value100Percent) * value);
        }

        public static GradientTableCell FromValues(int targetValue, int guessValue, int maxOffsetValue, string htmlClass = "", string htmlId = "")
        {
            int difference = Math.Min(Math.Abs(guessValue - targetValue), maxOffsetValue);
            int percent = AsPercentLimit100(difference, maxOffsetValue);
            Color correctColor = ColorScheme.COLOR_CORRECT;
            Color mistakeColor = ColorScheme.COLOR_MISTAKE;

            char appendArrow;
            Color upperColor;
            Color lowerColor;

            if (targetValue == guessValue)
            {
                upperColor = correctColor;
                lowerColor = correctColor;
                appendArrow = ' ';
            }
            else if (targetValue < guessValue)
            {
                upperColor = correctColor;
                lowerColor = PercentualColorOffset(correctColor, mistakeColor, percent);
                appendArrow = '↓';
            }
            else
            {
                lowerColor = correctColor;
                upperColor = PercentualColorOffset(correctColor, mistakeColor, percent);
                appendArrow = '↑';
            }

            return new GradientTableCell(guessValue.ToString() + ' ' + appendArrow, upperColor, lowerColor, 0, htmlClass: htmlClass, htmlId: htmlId);
        }

    }
}
