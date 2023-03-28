using Pokewordle.Shared.Extensions;
using Pokewordle.Shared.HtmlUtil;
using Pokewordle.Shared.PokemonData;
using System.Collections.Immutable;
using System.Drawing;
using System.Text;

namespace Pokewordle.Shared.GuessDisplayData
{
    public readonly record struct GuessDisplayData : IGuessDisplayData
    {
        public readonly IImmutableDictionary<ColumnType, ICellData> ColumnData;
        public static readonly ICellData EmptyCell = new SimpleCellData();

        public GuessDisplayData(IPokeData pokeDataToGuess, IPokeData pokeDataGuessed)
        {
            ImmutableDictionary<ColumnType, ICellData>.Builder dictionaryBuilder = ImmutableDictionary.CreateBuilder<ColumnType, ICellData>();

            dictionaryBuilder.Add(ColumnType.NAME, new SimpleCellData(ColumnType.NAME, pokeDataGuessed.Name.FirstCharToUpper(),
                pokeDataToGuess.Name.Equals(pokeDataGuessed.Name) ? ColorScheme.COLOR_CORRECT : ColorScheme.COLOR_MISTAKE,
                htmlId: "name"));


            IList<string> sharedTypes = pokeDataGuessed.FindSharedTypes(pokeDataToGuess, out IList<string> nonSharedTypes);
            switch (sharedTypes.Count)
            {
                case 0:
                    dictionaryBuilder.Add(ColumnType.TYPE1, new SimpleCellData(ColumnType.TYPE1, nonSharedTypes[0], ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type1"));
                    dictionaryBuilder.Add(ColumnType.TYPE2, new SimpleCellData(ColumnType.TYPE2, nonSharedTypes[1], ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type1"));
                    break;
                case 1:
                    dictionaryBuilder.Add(ColumnType.TYPE1, new SimpleCellData(ColumnType.TYPE1, sharedTypes[0], ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type1"));
                    dictionaryBuilder.Add(ColumnType.TYPE2, new SimpleCellData(ColumnType.TYPE2, nonSharedTypes[0], ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type2"));
                    break;
                case 2:
                default:
                    dictionaryBuilder.Add(ColumnType.TYPE1, new SimpleCellData(ColumnType.TYPE1, sharedTypes[0], ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type1"));
                    dictionaryBuilder.Add(ColumnType.TYPE2, new SimpleCellData(ColumnType.TYPE2, sharedTypes[1], ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type2"));
                    break;
            }

            dictionaryBuilder.Add(ColumnType.TYPES, new PokeTypeCellData(ColumnType.TYPES, pokeDataGuessed.Types,
                pokeDataGuessed.MatchTypes(pokeDataToGuess).ToTruePartialFalseColor())
                );

            dictionaryBuilder.Add(ColumnType.HEIGHT, GradientCellData.FromValues(ColumnType.HEIGHT, pokeDataToGuess.Height_m, pokeDataGuessed.Height_m, 2, htmlId: "height"));
            dictionaryBuilder.Add(ColumnType.WEIGHT, GradientCellData.FromValues(ColumnType.WEIGHT, pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg, 40, htmlId: "weight"));


            ColumnData = dictionaryBuilder.ToImmutable();
        }

        public async Task<IList<ICellData>> GetRowCells(IEnumerable<ColumnType> columnTypes)
        {
            IList<ICellData> tableCells = new List<ICellData>();
            foreach (ColumnType columnType in columnTypes)
            {
                if (ColumnData.TryGetValue(columnType, out ICellData? tableCell) && tableCell is not null)
                {
                    tableCells.Add(tableCell);
                }
                else
                {
                    tableCells.Add(EmptyCell);
                }
            }
            return tableCells;
        }

    }
}
