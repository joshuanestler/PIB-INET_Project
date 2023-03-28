using Pokewordle.Components.Cells;
using System.Drawing;

namespace Pokewordle.Shared.HtmlUtil
{
    public readonly record struct ImageCellData : ICellData
    {
        public ColumnType ColumnType { get; }
        public readonly string Url;
        public readonly string FontColor;
        public readonly string Background;
        public readonly string HtmlClass;
        public readonly string HtmlId;

        public Type CellType => typeof(SimpleCell);

        public ImageCellData(ColumnType columnType, string url = "", Color? background = null, Color? fontColor = null,
            string htmlClass = "", string htmlId = "")
        {
            this.ColumnType = columnType;

            Url = url;
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
