using Pokewordle.Shared.Extensions;
using Pokewordle.Shared.HtmlUtil;
using Pokewordle.Shared.PokemonData;
using System.Collections.Immutable;

namespace Pokewordle.Shared.GuessDisplayData
{
    public class LazyGuessDisplayData : IGuessDisplayData
    {
        public readonly IImmutableDictionary<ColumnType, FetchableData<ITableCell>> ColumnData;
        public static readonly ITableCell EmptyCell = new SimpleTableCell();

        private readonly IPokeData pokeDataGuessed;
        private readonly IPokeData pokeDataToGuess;

        public LazyGuessDisplayData(IPokeData pokeDataToGuess, IPokeData pokeDataGuessed)
        {
            this.pokeDataToGuess = pokeDataToGuess ?? throw new ArgumentNullException("Pokemon to guess was null!");
            this.pokeDataGuessed = pokeDataGuessed ?? throw new ArgumentNullException("Pokemon guessed was null!");
            ImmutableDictionary<ColumnType, FetchableData<ITableCell>>.Builder dictionaryBuilder = ImmutableDictionary.CreateBuilder<ColumnType, FetchableData<ITableCell>>();

            dictionaryBuilder.Add(ColumnType.NAME, new(CreateNameCell));

            dictionaryBuilder.Add(ColumnType.TYPE1, new(CreateType1Cell));
            dictionaryBuilder.Add(ColumnType.TYPE2, new(CreateType2Cell));
            dictionaryBuilder.Add(ColumnType.TYPES, new(CreateTypesCell));

            dictionaryBuilder.Add(ColumnType.HEIGHT, new(CreateHeightCell));
            dictionaryBuilder.Add(ColumnType.WEIGHT, new(CreateWeightCell));

            ColumnData = dictionaryBuilder.ToImmutable();
        }

        private ITableCell CreateNameCell()
        {
            return new SimpleTableCell(pokeDataGuessed.Name.FirstCharToUpper(),
                pokeDataToGuess.Name.Equals(pokeDataGuessed.Name) ? ColorScheme.COLOR_CORRECT : ColorScheme.COLOR_MISTAKE,
                htmlId: "name");
        }

        private ITableCell CreateType1Cell()
        {
            if (pokeDataGuessed.IsType1Shared(pokeDataToGuess, out string typeName))
            {
                return new PokeTypeTableCell(new string[] { typeName }, ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type1");
            }
            else
            {
                return new PokeTypeTableCell(new string[] { typeName }, ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type1");
            }
        }
        private ITableCell CreateType2Cell()
        {
            if (pokeDataGuessed.IsType2Shared(pokeDataToGuess, out string typeName))
            {
                return new PokeTypeTableCell(new string[] { typeName }, ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type2");
            }
            else
            {
                return new PokeTypeTableCell(new string[] { typeName }, ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type2");
            }
        }
        private ITableCell CreateTypesCell()
        {
            return new PokeTypeTableCell(pokeDataGuessed.Types,
                pokeDataGuessed.MatchTypes(pokeDataToGuess).ToTruePartialFalseColor(),
                htmlClass: "game-pokemon-type-field", htmlId: "type2"
                );
        }

        private ITableCell CreateHeightCell()
        {
            return GradientTableCell.FromValues(pokeDataToGuess.Height_m, pokeDataGuessed.Height_m, 2, htmlId: "height");
        }

        private ITableCell CreateWeightCell()
        {
            return GradientTableCell.FromValues(pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg, 2, htmlId: "weight");
        }

        public IList<ITableCell> GetRowCells(IEnumerable<ColumnType> columnTypes)
        {
            List<ITableCell> tableCells = new List<ITableCell>();
            foreach (ColumnType columnType in columnTypes)
            {
                if (ColumnData.TryGetValue(columnType, out FetchableData<ITableCell> tableCellFetchable) && tableCellFetchable is not null)
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
