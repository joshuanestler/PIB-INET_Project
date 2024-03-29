﻿using Pokewordle.Shared.HtmlUtil;
using System.Collections.Immutable;

namespace Pokewordle.Shared.GuessDisplayData
{
    public interface IGuessDisplayData
    {
        string GetPokemonName();
        bool GetIsCorrect();
        Task<IList<ICellData>> GetRowCells(IEnumerable<ColumnType> columnTypes);
        IImmutableList<ColumnType> GetObfuscationOrder(IEnumerable<ColumnType> columnTypes);
    }
}
