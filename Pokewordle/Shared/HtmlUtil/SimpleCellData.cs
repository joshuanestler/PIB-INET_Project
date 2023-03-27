using Pokewordle.Components.Cells;
using System.Drawing;
using System.Text;

namespace Pokewordle.Shared.HtmlUtil
{
    public readonly record struct SimpleCellData : ICellData
    {
        public readonly string DisplayString;
        public readonly string FontColor;
        public readonly string Background;
        public readonly string HtmlClass;
        public readonly string HtmlId;

        public Type CellType => typeof(SimpleCell);

        public SimpleCellData(string displayString = "", Color? background = null, Color? fontColor = null,
            string htmlClass = "", string htmlId = "")
        {
            DisplayString = displayString;
            Background = Convert.ColorToHexString(background, ColorScheme.COLOR_TYPE_NOT_FOUND);
            FontColor = Convert.ColorToHexString(fontColor, ColorScheme.COLOR_TYPE_NOT_FOUND);
            HtmlClass = htmlClass;
            HtmlId = htmlId;
        }

    }
}
