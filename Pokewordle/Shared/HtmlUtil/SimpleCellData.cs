using Pokewordle.Components.Cells;
using System.Drawing;
using System.Text;

namespace Pokewordle.Shared.HtmlUtil
{
    public readonly record struct SimpleCellData : ICellData
    {
        public ColumnType ColumnType { get; }
        public readonly string DisplayString;
        public readonly string FontColor;
        public readonly string Background;
        public readonly string HtmlClass;
        public readonly string HtmlId;

        public Type CellType => typeof(SimpleCell);

        public SimpleCellData(ColumnType columnType, string displayString = "", Color? background = null, Color? fontColor = null,
            string htmlClass = "", string htmlId = "")
        {
            this.ColumnType = columnType;

            DisplayString = displayString;
            Background = Convert.ColorToHexString(background, ColorScheme.COLOR_TYPE_NOT_FOUND);
            FontColor = Convert.ColorToHexString(fontColor, ColorScheme.COLOR_TYPE_NOT_FOUND);
            HtmlClass = htmlClass;
            HtmlId = htmlId;
        }

        public string GetColumnWidth()
        {
            return HeaderGenerator.GetColumnWidth(ColumnType).ToString();
        }

    }
}
