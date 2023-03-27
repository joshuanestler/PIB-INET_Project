using Pokewordle.Shared.Extensions;
using Pokewordle.Shared.HtmlUtil;
using Pokewordle.Shared.PokemonData;
using System.Collections.Immutable;

namespace Pokewordle.Shared
{
    public class LazyGuessDisplayData
    {
        public readonly IImmutableDictionary<ColumnType, FetchableData<ITableCell>> ColumnData;
        public static readonly ITableCell EmptyCell = new SimpleTableCell();

        private readonly IPokeData pokeDataGuessed;
        private readonly IPokeData pokeDataToGuess;

        public LazyGuessDisplayData(IPokeData pokeDataToGuess, IPokeData pokeDataGuessed)
        {
            this.pokeDataToGuess = pokeDataToGuess;
            this.pokeDataGuessed = pokeDataGuessed;
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
                return new SimpleTableCell(typeName, ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type1");
            } else
            {
                return new SimpleTableCell(typeName, ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type1");
            }
        }
        private ITableCell CreateType2Cell()
        {
            if (pokeDataGuessed.IsType2Shared(pokeDataToGuess, out string typeName))
            {
                return new SimpleTableCell(typeName, ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type2");
            }
            else
            {
                return new SimpleTableCell(typeName, ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type2");
            }
        }
        private ITableCell CreateTypesCell()
        {
            return new PokeTypeTableCell(pokeDataGuessed.Types,
                HtmlUtil.Convert.ColorToHexString(pokeDataGuessed.MatchTypes(pokeDataToGuess).ToTruePartialFalseColor())
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

    }
}
