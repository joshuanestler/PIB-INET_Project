using System.Drawing;
using System.Text;

namespace Pokewordle.Shared.HtmlUtil
{
    public readonly record struct SimpleTableCell : ITableCell
    {
        private readonly string DisplayString;
        private readonly string FontColor;
        private readonly string Background;
        private readonly string HtmlClass;
        private readonly string HtmlId;

        public SimpleTableCell(string displayString = "", Color? background = null, Color? fontColor = null,
            string htmlClass = "", string htmlId = "")
        {
            DisplayString = displayString;
            Background = Convert.ColorToHexString(background);
            FontColor = Convert.ColorToHexString(fontColor);
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

            sb.Append($"style = \"background-color: {Background};\"");
            sb.Append(">\n");

            //sb.Append($"<pre style = \"background-color: {Background};\">");

            sb.Append(DisplayString);

            //sb.Append("</pre>\n");
            sb.Append("</td>");

            return sb.ToString();
        }

    }
}
