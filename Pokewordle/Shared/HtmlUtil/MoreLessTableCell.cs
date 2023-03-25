using System.Drawing;
using System.Text;

namespace Pokewordle.Shared.HtmlUtil
{
    public class MoreLessTableCell : ITableCell
    {
        private readonly string DisplayString;
        private readonly string FontColor;
        private readonly string Degrees;
        private readonly string Background1;
        private readonly string Background2;
        private readonly string HtmlClass;
        private readonly string HtmlId;

        public MoreLessTableCell(
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
