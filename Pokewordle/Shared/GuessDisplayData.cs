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

            if (pokeDataToGuess.Name.Equals(pokeDataGuessed.Name))
            {
                dictionaryBuilder.Add(ColumnType.NAME, new SimpleTableCell(pokeDataGuessed.Name, ColorScheme.COLOR_CORRECT, htmlId: "name"));
            } else
            {
                dictionaryBuilder.Add(ColumnType.NAME, new SimpleTableCell(pokeDataGuessed.Name, ColorScheme.COLOR_MISTAKE, htmlId: "name"));
            }

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

            dictionaryBuilder.Add(ColumnType.HEIGHT, AsGradientTableCell(pokeDataToGuess.Height_m, pokeDataGuessed.Height_m, 2, htmlId: "height"));
            dictionaryBuilder.Add(ColumnType.WEIGHT, AsGradientTableCell(pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg, 40, htmlId: "weight"));


            ColumnData = dictionaryBuilder.ToImmutable();
        }

        private static int PercentualOffset(int baseValue, int offsetValue, int offsetPercent)
        {
            double diff = offsetValue - baseValue;
            return baseValue + (int)Math.Round(diff / 100 * offsetPercent);
        }

        private static Color PercentualColorOffset(Color baseColor, Color offsetColor, int offsetPercent)
        {
            int r = PercentualOffset(baseColor.R, offsetColor.R, offsetPercent);
            int g = PercentualOffset(baseColor.G, offsetColor.G, offsetPercent);
            int b = PercentualOffset(baseColor.B, offsetColor.B, offsetPercent);
            return Color.FromArgb(r, g, b);
        }

        private static int AsPercentLimit100(int value, int value100Percent)
        {
            if (value >= value100Percent)
            {
                return 100;
            }

            return (int)Math.Round((100d / (double)value100Percent) * value);
        }

        private static ITableCell AsGradientTableCell(int targetValue, int guessValue, int maxOffsetValue, string htmlClass = "", string htmlId = "")
        {
            int difference = Math.Min(Math.Abs(guessValue - targetValue), maxOffsetValue);
            int percent = AsPercentLimit100(difference, maxOffsetValue);
            Color correctColor = ColorScheme.COLOR_CORRECT;
            Color mistakeColor = ColorScheme.COLOR_MISTAKE;

            char appendArrow;
            Color upperColor;
            Color lowerColor;

            if (targetValue == guessValue)
            {
                upperColor = correctColor;
                lowerColor = correctColor;
                appendArrow = ' ';
            } else if (targetValue < guessValue)
            {
                upperColor = correctColor;
                lowerColor = PercentualColorOffset(correctColor, mistakeColor, percent);
                appendArrow = '↓';
            } else
            {
                lowerColor = correctColor;
                upperColor = PercentualColorOffset(correctColor, mistakeColor, percent);
                appendArrow = '↑';
            }

            return new GradientTableCell(guessValue.ToString() + ' ' + appendArrow, upperColor, lowerColor, 0, htmlClass: htmlClass, htmlId: htmlId);
        }

        public string ToRowString(IEnumerable<ColumnType> columnTypes)
        {
            StringBuilder sb = new();
            sb.AppendLine("<tr>");
            foreach(ColumnType columnType in columnTypes)
            {
                if (ColumnData.TryGetValue(columnType, out ITableCell? tableCell) && tableCell is not null)
                {
                    sb.AppendLine(tableCell.ToTableCellString());
                } else
                {
                    sb.AppendLine(EmptyCell.ToTableCellString());
                }
            }
            sb.AppendLine("</tr>");
            return sb.ToString();
        }


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

        private static readonly Dictionary<string, Color> typeColors = new()
            {
                { "normal", Color.Beige },
                { "grass", Color.LawnGreen },
                { "water", Color.Aqua },
                { "fire", Color.OrangeRed },
                { "electric", Color.Yellow },
                { "ground", Color.SandyBrown },
                { "flying", Color.SkyBlue },
                { "rock", Color.SaddleBrown },
                { "ice", Color.AliceBlue },
                { "fighting", Color.DarkRed },
                { "psychic", Color.Pink },
                { "ghost", Color.Purple },
                { "bug", Color.Lime },
                { "dark", Color.Black },
                { "dragon", Color.DarkBlue },
                { "steel", Color.Silver },
                { "fairy", Color.Pink },
                { "poison", Color.Purple }
            };
        private static Color? TypeNameToColor(string typeName)
        {
            return typeColors.TryGetValue(typeName, out Color value) ? value : null;
        }
    }
}
