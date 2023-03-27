using System.Drawing;

namespace Pokewordle.Shared.HtmlUtil
{
    public readonly record struct PokeTypeTableCell : ITableCell
    {
        public readonly string[] TypeNames;
        public readonly string Background;
        public readonly string HtmlClass;
        public readonly string HtmlId;

        public PokeTypeTableCell(IEnumerable<string> typeNames, string background, string htmlClass = "", string htmlId = "")
        {
            this.TypeNames = typeNames.ToArray();
            this.Background = background;
            this.HtmlClass = htmlClass;
            this.HtmlId = htmlId;
        }

        public static string TypeNameToColorHexString(string typeName)
        {
            Color? color = ColorScheme.TypeNameToColor(typeName);
            return Convert.ColorToHexString(color, ColorScheme.COLOR_TYPE_NOT_FOUND);
        }

    }
}
