using Pokewordle.Shared.HtmlUtil;

namespace Pokewordle.Shared.GuessDisplayData
{
    public interface IGuessDisplayData
    {
        Task<IList<ICellData>> GetRowCells(IEnumerable<ColumnType> columnTypes);
    }
}
