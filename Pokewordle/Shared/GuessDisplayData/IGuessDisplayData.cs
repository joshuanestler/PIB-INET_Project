using Pokewordle.Shared.HtmlUtil;

namespace Pokewordle.Shared.GuessDisplayData
{
    public interface IGuessDisplayData
    {
        IList<ICellData> GetRowCells(IEnumerable<ColumnType> columnTypes);
    }
}
