using PokeApiNet;
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

            dictionaryBuilder.Add(ColumnType.GENERATION, new(CreateGenerationCell));

            ColumnData = dictionaryBuilder.ToImmutable();
        }

        private async Task<ICellData> CreateNameCell()
        {
            return new SimpleCellData(ColumnType.NAME, pokeDataGuessed.Name.FirstCharToUpper(),
                pokeDataToGuess.Name.Equals(pokeDataGuessed.Name) ? ColorScheme.COLOR_CORRECT : ColorScheme.COLOR_MISTAKE,
                htmlId: "name");
        }

        private async Task<ICellData> CreateType1Cell()
        {
            if (pokeDataGuessed.IsType1Shared(pokeDataToGuess, out string typeName))
            {
                return new PokeTypeCellData(ColumnType.TYPE1, new string[] { typeName }, ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type1");
            }
            else
            {
                return new PokeTypeCellData(ColumnType.TYPE1, new string[] { typeName }, ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type1");
            }
        }
        private async Task<ICellData> CreateType2Cell()
        {
            if (pokeDataGuessed.IsType2Shared(pokeDataToGuess, out string typeName))
            {
                return new PokeTypeCellData(ColumnType.TYPE2, new string[] { typeName }, ColorScheme.COLOR_CORRECT, htmlClass: "game-pokemon-type-field", htmlId: "type2");
            }
            else
            {
                return new PokeTypeCellData(ColumnType.TYPE2, new string[] { typeName }, ColorScheme.COLOR_MISTAKE, htmlClass: "game-pokemon-type-field", htmlId: "type2");
            }
        }
        private async Task<ICellData> CreateTypesCell()
        {
            return new PokeTypeCellData(ColumnType.TYPES, pokeDataGuessed.Types,
                pokeDataGuessed.MatchTypes(pokeDataToGuess).ToTruePartialFalseColor(),
                htmlClass: "game-pokemon-type-field", htmlId: "type2"
                );
        }

        private async Task<ICellData> CreateHeightCell()
        {
            return GradientCellData.FromValues(ColumnType.HEIGHT, pokeDataToGuess.Height_m, pokeDataGuessed.Height_m, 2, htmlId: "height");
        }

        private async Task<ICellData> CreateWeightCell()
        {
            return GradientCellData.FromValues(ColumnType.WEIGHT, pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg, 2, htmlId: "weight");
        }

        public async Task<IList<ICellData>> GetRowCells(IEnumerable<ColumnType> columnTypes)
        {
            List<ICellData> tableCells = new List<ICellData>();
            foreach (ColumnType columnType in columnTypes)
            {
                if (ColumnData.TryGetValue(columnType, out FetchableData<ICellData>? tableCellFetchable) && tableCellFetchable is not null)
                {
                    tableCells.Add(await tableCellFetchable.FetchValue());
                }
                else
                {
                    tableCells.Add(EmptyCell);
                }
            }
            return tableCells;
        }

        private async Task<ICellData> CreateGenerationCell()
        {
            Task<int> generationGuessedTask = pokeDataGuessed.GetGeneration();
            Task<int> generationToGuessTask = pokeDataToGuess.GetGeneration();
            int generationGuessed = await generationGuessedTask;
            int generationToGuess = await generationToGuessTask;
            return new SimpleCellData(ColumnType.GENERATION, generationGuessed.ToString(),
                generationToGuess.Equals(generationGuessed) ? ColorScheme.COLOR_CORRECT : ColorScheme.COLOR_MISTAKE,
                htmlId: "gen");
        }
    }
}
