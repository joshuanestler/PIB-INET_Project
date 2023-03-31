using Pokewordle.Components.Cells;
using System.Drawing;

namespace Pokewordle.Shared.HtmlUtil
{
    public readonly record struct PokeTypeCellData : ICellData
    {
        public ColumnType ColumnType { get; }
        public readonly string[] TypeNames;
        public readonly string Background;
        public readonly string HtmlClass;
        public readonly string HtmlId;
        public Type CellType => typeof(PokeTypeCell);

        public PokeTypeCellData(ColumnType columnType, IEnumerable<string> typeNames, Color background, string htmlClass = "", string htmlId = "")
        {
            this.ColumnType = columnType;

            this.TypeNames = typeNames.ToArray();
            this.Background = Convert.ColorToHexString(background);
            this.HtmlClass = htmlClass;
            this.HtmlId = htmlId;
        }

        public string GetColumnWidth()
        {
            return HeaderGenerator.GetColumnWidth(ColumnType).ToString();
        }

        public static string TypeNameToColorHexString(string typeName)
        {
            Color? color = ColorScheme.TypeNameToColor(typeName);
            return Convert.ColorToHexString(color, ColorScheme.COLOR_TYPE_NOT_FOUND);
        }

    }
}
