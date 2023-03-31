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
        public readonly IImmutableList<ColumnType> ObfuscationOrder;

        public static readonly ICellData EmptyCell = new SimpleCellData();

        private readonly IPokeData pokeDataGuessed;
        private readonly IPokeData pokeDataToGuess;

        public bool IsCorrect { get; }

        public LazyGuessDisplayData(IPokeData pokeDataToGuess, IPokeData pokeDataGuessed)
        {
            this.pokeDataToGuess = pokeDataToGuess ?? throw new ArgumentNullException("Pokemon to guess was null!");
            this.pokeDataGuessed = pokeDataGuessed ?? throw new ArgumentNullException("Pokemon guessed was null!");

            IsCorrect = pokeDataToGuess.Name.Equals(pokeDataGuessed.Name);

            List<ColumnType> columnTypes = Enum.GetValues<ColumnType>().ToList();
            columnTypes.Remove(ColumnType.NAME);
            columnTypes.Remove(ColumnType.NAME_LOCAL);
            columnTypes.Remove(ColumnType.SPRITE);
            Random random = new Random();
            ImmutableList<ColumnType>.Builder listBuilder = ImmutableList.CreateBuilder<ColumnType>();
            while (columnTypes.Count > 0)
            {
                int index = random.Next(columnTypes.Count);
                listBuilder.Add(columnTypes[index]);
                columnTypes.RemoveAt(index);
            }
            listBuilder.Add(ColumnType.SPRITE);
            listBuilder.Add(ColumnType.NAME_LOCAL);
            listBuilder.Add(ColumnType.NAME);
            ObfuscationOrder = listBuilder.ToImmutable();

            ImmutableDictionary<ColumnType, FetchableData<ICellData>>.Builder dictionaryBuilder = ImmutableDictionary.CreateBuilder<ColumnType, FetchableData<ICellData>>();

            dictionaryBuilder.Add(ColumnType.SPRITE, new(CreateSpriteCell));

            dictionaryBuilder.Add(ColumnType.NAME, new(CreateNameCell));
            dictionaryBuilder.Add(ColumnType.NAME_LOCAL, new(CreateLocalizedNameCell));

            dictionaryBuilder.Add(ColumnType.TYPE1, new(CreateType1Cell));
            dictionaryBuilder.Add(ColumnType.TYPE2, new(CreateType2Cell));
            dictionaryBuilder.Add(ColumnType.TYPES, new(CreateTypesCell));
            dictionaryBuilder.Add(ColumnType.ABILITIES, new(CreateAbilitiesCell));

            dictionaryBuilder.Add(ColumnType.HEIGHT, new(CreateHeightCell));
            dictionaryBuilder.Add(ColumnType.WEIGHT, new(CreateWeightCell));

            dictionaryBuilder.Add(ColumnType.GENERATION, new(CreateGenerationCell));

            dictionaryBuilder.Add(ColumnType.HP, new(CreateHpCell));
            dictionaryBuilder.Add(ColumnType.ATK, new(CreateAtkCell));
            dictionaryBuilder.Add(ColumnType.DEF, new(CreateDefCell));
            dictionaryBuilder.Add(ColumnType.SPA, new(CreateSpACell));
            dictionaryBuilder.Add(ColumnType.SPD, new(CreateSpDCell));
            dictionaryBuilder.Add(ColumnType.SPE, new(CreateSpeCell));
            dictionaryBuilder.Add(ColumnType.BST, new(CreateBstCell));

            dictionaryBuilder.Add(ColumnType.MAXSTATS, new(CreateMaxStatsCell));
            dictionaryBuilder.Add(ColumnType.MINSTATS, new(CreateMinStatsCell));

            ColumnData = dictionaryBuilder.ToImmutable();
        }

        private async Task<ICellData> CreateSpriteCell()
        {
            return new ImageCellData(ColumnType.SPRITE, pokeDataGuessed.SpriteUrl,
                pokeDataToGuess.Name.Equals(pokeDataGuessed.Name) ? ColorScheme.COLOR_CORRECT : ColorScheme.COLOR_MISTAKE,
                htmlId: "sprite");
        }

        private async Task<ICellData> CreateNameCell()
        {
            return new SimpleCellData(ColumnType.NAME, pokeDataGuessed.Name.FirstCharToUpper(),
                pokeDataToGuess.Name.Equals(pokeDataGuessed.Name) ? ColorScheme.COLOR_CORRECT : ColorScheme.COLOR_MISTAKE,
                htmlId: "name");
        }

        private async Task<ICellData> CreateLocalizedNameCell()
        {
            Console.WriteLine(pokeDataGuessed.NameLocalized);
            return new SimpleCellData(ColumnType.NAME_LOCAL, pokeDataGuessed.NameLocalized.FirstCharToUpper(),
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
                pokeDataGuessed.Match(pokeDataToGuess, pokeData => pokeData.Types).ToTruePartialFalseColor(),
                htmlClass: "game-pokemon-type-field", htmlId: "types"
                );
        }
        private async Task<ICellData> CreateAbilitiesCell()
        {
            return new SimpleCellData(ColumnType.ABILITIES, pokeDataGuessed.Abilities.Aggregate((item, otherItems) => otherItems + ", " + item),
                pokeDataGuessed.Match(pokeDataToGuess, pokeData => pokeData.Abilities).ToTruePartialFalseColor(),
                htmlClass: "game-pokemon-abilities-field", htmlId: "abilities"
                );
        }

        private async Task<ICellData> CreateHeightCell()
        {
            return new ArrowCellData(ColumnType.HEIGHT, pokeDataGuessed.Height_m, pokeDataToGuess.Height_m);
        }

        private async Task<ICellData> CreateWeightCell()
        {
            return new ArrowCellData(ColumnType.WEIGHT, pokeDataGuessed.Weight_kg, pokeDataToGuess.Weight_kg);
        }

        #region StatCells

        private async Task<ICellData> CreateHpCell()
        {

            return new ArrowCellData(ColumnType.HP, pokeDataGuessed.HP, pokeDataToGuess.HP);
        }
        
        private async Task<ICellData> CreateAtkCell()
        {
            return new ArrowCellData(ColumnType.ATK, pokeDataGuessed.Atk, pokeDataToGuess.Atk);
        }

        private async Task<ICellData> CreateDefCell()
        {
            return new ArrowCellData(ColumnType.DEF, pokeDataGuessed.Def, pokeDataToGuess.Def);
        }

        private async Task<ICellData> CreateSpACell()
        {
            return new ArrowCellData(ColumnType.SPA, pokeDataGuessed.SpA, pokeDataToGuess.SpA);
        }

        private async Task<ICellData> CreateSpDCell()
        {
            return new ArrowCellData(ColumnType.SPD, pokeDataGuessed.SpD, pokeDataToGuess.SpD);
        }

        private async Task<ICellData> CreateSpeCell()
        {
            return new ArrowCellData(ColumnType.SPE, pokeDataGuessed.Spe, pokeDataToGuess.Spe);
        }

        private async Task<ICellData> CreateBstCell()
        {
            return new ArrowCellData(ColumnType.BST, pokeDataGuessed.BST, pokeDataToGuess.BST);
        }


        private async Task<ICellData> CreateMinStatsCell()
        {
            return new SimpleCellData(ColumnType.MINSTATS, pokeDataGuessed.MinStatNames.Aggregate((item, otherItems) => otherItems + ", " + item),
                pokeDataGuessed.Match(pokeDataToGuess, pokeData => pokeData.MinStatNames).ToTruePartialFalseColor(),
                htmlId: "minstats"
                );
        }

        private async Task<ICellData> CreateMaxStatsCell()
        {
            return new SimpleCellData(ColumnType.MAXSTATS, pokeDataGuessed.MaxStatNames.Aggregate((item, otherItems) => otherItems + ", " + item),
                pokeDataGuessed.Match(pokeDataToGuess, pokeData => pokeData.MaxStatNames).ToTruePartialFalseColor(),
                htmlId: "maxstats"
                );
        }

        #endregion StatCells


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

        public IImmutableList<ColumnType> GetObfuscationOrder(IEnumerable<ColumnType> columnTypes)
        {
            return ObfuscationOrder.Where(ct => columnTypes.Contains(ct)).ToImmutableList();
        }

        public string GetPokemonName()
        {
            return pokeDataGuessed.Name;
        }

        public bool GetIsCorrect()
        {
            return IsCorrect;
        }
    }
}
