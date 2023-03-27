using Pokewordle.Shared.Extensions;
using Pokewordle.Shared.HtmlUtil;
using System.Collections.Immutable;
using System.Drawing;
using System.Text;

namespace Pokewordle.Shared
{
    public readonly record struct GuessDisplayData
    {
        public readonly IImmutableDictionary<ColumnType, ITableCell> ColumnData;
        public static readonly ITableCell EmptyCell = new SimpleTableCell();

        public GuessDisplayData(PokeData pokeDataToGuess, PokeData pokeDataGuessed)
        {
            ImmutableDictionary<ColumnType, ITableCell>.Builder dictionaryBuilder = ImmutableDictionary.CreateBuilder<ColumnType, ITableCell>();

            dictionaryBuilder.Add(ColumnType.NAME, new SimpleTableCell(pokeDataGuessed.Name.FirstCharToUpper(),
                pokeDataToGuess.Name.Equals(pokeDataGuessed.Name) ? ColorScheme.COLOR_CORRECT : ColorScheme.COLOR_MISTAKE, 
                htmlId: "name"));


            IList<string> sharedTypes = pokeDataGuessed.FindSharedTypes(pokeDataToGuess, out IList<string> nonSharedTypes);
            switch(sharedTypes.Count)
            {
                case 0:
                    dictionaryBuilder.Add(ColumnType.TYPE1, new SimpleTableCell(nonSharedTypes[0], ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type1"));
                    dictionaryBuilder.Add(ColumnType.TYPE2, new SimpleTableCell(nonSharedTypes[1], ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type1"));
                    break;
                case 1:
                    dictionaryBuilder.Add(ColumnType.TYPE1, new SimpleTableCell(sharedTypes[0], ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type1"));
                    dictionaryBuilder.Add(ColumnType.TYPE2, new SimpleTableCell(nonSharedTypes[0], ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type2"));
                    break;
                case 2:
                default:
                    dictionaryBuilder.Add(ColumnType.TYPE1, new SimpleTableCell(sharedTypes[0], ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type1"));
                    dictionaryBuilder.Add(ColumnType.TYPE2, new SimpleTableCell(sharedTypes[1], ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type2"));
                    break;
            }
            
            dictionaryBuilder.Add(ColumnType.TYPES, new PokeTypeTableCell(pokeDataGuessed.Types, 
                HtmlUtil.Convert.ColorToHexString(pokeDataGuessed.MatchTypes(pokeDataToGuess).ToTruePartialFalseColor()))
                );

            dictionaryBuilder.Add(ColumnType.HEIGHT, GradientTableCell.FromValues(pokeDataToGuess.Height_m, pokeDataGuessed.Height_m, 2, htmlId: "height"));
            dictionaryBuilder.Add(ColumnType.WEIGHT, GradientTableCell.FromValues(pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg, 40, htmlId: "weight"));


            ColumnData = dictionaryBuilder.ToImmutable();
        }




        //public string ToRowString(IEnumerable<ColumnType> columnTypes)
        //{
        //    StringBuilder sb = new();
        //    sb.AppendLine("<tr>");
        //    foreach(ColumnType columnType in columnTypes)
        //    {
        //        if (ColumnData.TryGetValue(columnType, out ITableCell? tableCell) && tableCell is not null)
        //        {
        //            sb.AppendLine(tableCell.ToTableCellString());
        //        } else
        //        {
        //            sb.AppendLine(EmptyCell.ToTableCellString());
        //        }
        //    }
        //    sb.AppendLine("</tr>");
        //    return sb.ToString();
        //}


        public IList<ITableCell> GetTableCells(IEnumerable<ColumnType> columnTypes)
        {
            List<ITableCell> tableCells = new List<ITableCell>();
            foreach(ColumnType columnType in columnTypes)
            {
                if (ColumnData.TryGetValue(columnType, out ITableCell? tableCell) && tableCell is not null)
                {
                    tableCells.Add(tableCell);
                } else
                {
                    tableCells.Add(EmptyCell);
                }
            }
            return tableCells;
        }

    }
}
