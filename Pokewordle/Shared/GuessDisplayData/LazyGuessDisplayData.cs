﻿using PokeApiNet;
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

            dictionaryBuilder.Add(ColumnType.SPRITE, new(CreateSpriteCell));

            dictionaryBuilder.Add(ColumnType.NAME, new(CreateNameCell));

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
            return GradientCellData.FromValues(ColumnType.HEIGHT, pokeDataToGuess.Height_m, pokeDataGuessed.Height_m, pokeDataToGuess.Height_m / 5, htmlId: "height");
        }

        private async Task<ICellData> CreateWeightCell()
        {
            return GradientCellData.FromValues(ColumnType.WEIGHT, pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg, pokeDataToGuess.Weight_kg / 5, htmlId: "weight");
        }

        #region StatCells

        private async Task<ICellData> CreateHpCell()
        {
            return GradientCellData.FromValues(ColumnType.HP, pokeDataToGuess.HP, pokeDataGuessed.HP, 30, htmlId: "hp");
        }
        
        private async Task<ICellData> CreateAtkCell()
        {
            return GradientCellData.FromValues(ColumnType.ATK, pokeDataToGuess.Atk, pokeDataGuessed.Atk, 30, htmlId: "atk");
        }

        private async Task<ICellData> CreateDefCell()
        {
            return GradientCellData.FromValues(ColumnType.DEF, pokeDataToGuess.Def, pokeDataGuessed.Def, 30, htmlId: "def");
        }

        private async Task<ICellData> CreateSpACell()
        {
            return GradientCellData.FromValues(ColumnType.SPA, pokeDataToGuess.SpA, pokeDataGuessed.SpA, 30, htmlId: "spa");
        }

        private async Task<ICellData> CreateSpDCell()
        {
            return GradientCellData.FromValues(ColumnType.SPD, pokeDataToGuess.SpD, pokeDataGuessed.SpD, 30, htmlId: "spd");
        }

        private async Task<ICellData> CreateSpeCell()
        {
            return GradientCellData.FromValues(ColumnType.SPE, pokeDataToGuess.Spe, pokeDataGuessed.Spe, 30, htmlId: "spe");
        }

        private async Task<ICellData> CreateBstCell()
        {
            return GradientCellData.FromValues(ColumnType.BST, pokeDataToGuess.BST, pokeDataGuessed.BST, 30, htmlId: "hp");
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
    }
}
