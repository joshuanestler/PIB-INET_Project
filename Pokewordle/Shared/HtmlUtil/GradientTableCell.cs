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
            FontColor = Convert.ColorToHexString(fontColor);
            Degrees = degrees.ToString();
            Background1 = Convert.ColorToHexString(background1);
            Background2 = Convert.ColorToHexString(background2);
            HtmlClass = htmlClass;
            HtmlId = htmlId;
        }

        public string ToTableCellString()
        {
            StringBuilder sb = new();
            sb.Append("<td ");
            if (HtmlClass.Length > 0)
            {
                sb.Append($"class=\"{HtmlClass}\" ");
            }
            if (HtmlId.Length > 0)
            {
                sb.Append($"id=\"{HtmlId}\" ");
            }
            sb.Append($"style = \"background: linear-gradient({Degrees}deg, {Background1}, {Background2});\" ");
            sb.Append(">");

            sb.Append(DisplayString);

            sb.Append("</td>");

            return sb.ToString();
        }
    }
}
