using System.Drawing;
using System.Text;

namespace Pokewordle.Shared.HtmlUtil
{
    public readonly record struct GradientTableCell : ITableCell
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

        private static int PercentualOffset(int baseValue, int offsetValue, float offsetPercent)
        {
            double diff = offsetValue - baseValue;
            return baseValue + (int)Math.Round(diff / 100 * offsetPercent);
        }

        private static Color PercentualColorOffset(Color baseColor, Color offsetColor, float offsetPercent)
        {
            int r = PercentualOffset(baseColor.R, offsetColor.R, offsetPercent);
            int g = PercentualOffset(baseColor.G, offsetColor.G, offsetPercent);
            int b = PercentualOffset(baseColor.B, offsetColor.B, offsetPercent);
            return Color.FromArgb(r, g, b);
        }

        private static float AsPercentLimit0to100(float value, float value100Percent)
        {
            if (value >= value100Percent)
            {
                return 100;
            }

            float calculated = (100f / value100Percent) * value;

            if (calculated <= 0)
            {
                return 0;
            }

            return calculated;
        }

        public static GradientTableCell FromValues(float targetValue, float guessValue, float maxOffsetValue, string htmlClass = "", string htmlId = "")
        {
            float difference = Math.Min(Math.Abs(guessValue - targetValue), maxOffsetValue);
            float percent = AsPercentLimit0to100(difference, maxOffsetValue);
            Color correctColor = ColorScheme.COLOR_CORRECT;
            Color mistakeColor = ColorScheme.COLOR_MISTAKE;

            char arrow;
            Color upperColor;
            Color lowerColor;

            if (targetValue == guessValue)
            {
                upperColor = correctColor;
                lowerColor = correctColor;
                arrow = ' ';
            }
            else if (targetValue < guessValue)
            {
                upperColor = correctColor;
                lowerColor = PercentualColorOffset(correctColor, mistakeColor, percent);
                arrow = '↓';
            }
            else
            {
                lowerColor = correctColor;
                upperColor = PercentualColorOffset(correctColor, mistakeColor, percent);
                arrow = '↑';
            }

            return new GradientTableCell(arrow.ToString() + ' ' + guessValue.ToString(), upperColor, lowerColor, 0, htmlClass: htmlClass, htmlId: htmlId);
        }

    }
}
