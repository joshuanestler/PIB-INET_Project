using System.Drawing;

namespace Pokewordle.Shared.HtmlUtil
{
    public interface ICellData
    {
        ColumnType ColumnType { get; }

        string GetColumnWidth();
    }
}
