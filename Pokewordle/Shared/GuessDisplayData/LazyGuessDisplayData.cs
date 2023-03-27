using Pokewordle.Shared.Extensions;
using Pokewordle.Shared.HtmlUtil;
using Pokewordle.Shared.PokemonData;
using System.Collections.Immutable;

namespace Pokewordle.Shared.GuessDisplayData
{
    public class LazyGuessDisplayData : IGuessDisplayData
    {
        public readonly IImmutableDictionary<ColumnType, FetchableData<ICellData>> ColumnData;
        public static readonly ICellData EmptyCell = new SimpleCellData();

        private readonly IPokeData pokeDataGuessed;
        private readonly IPokeData pokeDataToGuess;

        public LazyGuessDisplayData(IPokeData pokeDataToGuess, IPokeData pokeDataGuessed)
        {
            this.pokeDataToGuess = pokeDataToGuess ?? throw new ArgumentNullException("Pokemon to guess was null!");
            this.pokeDataGuessed = pokeDataGuessed ?? throw new ArgumentNullException("Pokemon guessed was null!");
            ImmutableDictionary<ColumnType, FetchableData<ICellData>>.Builder dictionaryBuilder = ImmutableDictionary.CreateBuilder<ColumnType, FetchableData<ICellData>>();

            dictionaryBuilder.Add(ColumnType.NAME, new(CreateNameCell));

            dictionaryBuilder.Add(ColumnType.TYPE1, new(CreateType1Cell));
            dictionaryBuilder.Add(ColumnType.TYPE2, new(CreateType2Cell));
            dictionaryBuilder.Add(ColumnType.TYPES, new(CreateTypesCell));

            dictionaryBuilder.Add(ColumnType.HEIGHT, new(CreateHeightCell));
            dictionaryBuilder.Add(ColumnType.WEIGHT, new(CreateWeightCell));

            ColumnData = dictionaryBuilder.ToImmutable();
        }

        private ICellData CreateNameCell()
        {
            return new SimpleCellData(pokeDataGuessed.Name.FirstCharToUpper(),
                pokeDataToGuess.Name.Equals(pokeDataGuessed.Name) ? ColorScheme.COLOR_CORRECT : ColorScheme.COLOR_MISTAKE,
                htmlId: "name");
        }

        private ICellData CreateType1Cell()
        {
            if (pokeDataGuessed.IsType1Shared(pokeDataToGuess, out string typeName))
            {
                return new PokeTypeCellData(new string[] { typeName }, ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type1");
            }
            else
            {
                return new PokeTypeCellData(new string[] { typeName }, ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type1");
            }
        }
        private ICellData CreateType2Cell()
        {
            if (pokeDataGuessed.IsType2Shared(pokeDataToGuess, out string typeName))
            {
                return new PokeTypeCellData(new string[] { typeName }, ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type2");
            }
            else
            {
                return new PokeTypeCellData(new string[] { typeName }, ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type2");
            }
        }
        private ICellData CreateTypesCell()
        {
            return new PokeTypeCellData(pokeDataGuessed.Types,
                pokeDataGuessed.MatchTypes(pokeDataToGuess).ToTruePartialFalseColor(),
                htmlClass: "game-pokemon-type-field", htmlId: "type2"
                );
        }

        private ICellData CreateHeightCell()
        {
            return GradientCellData.FromValues(pokeDataToGuess.Height_m, pokeDataGuessed.Height_m, 2, htmlId: "height");
        }

        private ICellData CreateWeightCell()
        {
            return GradientCellData.FromValues(pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg, 2, htmlId: "weight");
        }

        public IList<ICellData> GetRowCells(IEnumerable<ColumnType> columnTypes)
        {
            List<ICellData> tableCells = new List<ICellData>();
            foreach (ColumnType columnType in columnTypes)
            {
                if (ColumnData.TryGetValue(columnType, out FetchableData<ICellData>? tableCellFetchable) && tableCellFetchable is not null)
                {
                    tableCells.Add(tableCellFetchable.Value);
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
