using Pokewordle.Shared.HtmlUtil;

namespace Pokewordle.Shared.GuessDisplayData
{
    public interface IGuessDisplayData
    {
        IList<ITableCell> GetRowCells(IEnumerable<ColumnType> columnTypes);
    }
}
