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

            dictionaryBuilder.Add(ColumnType.NAME, new SimpleTableCell(pokeDataGuessed.Name));
            dictionaryBuilder.Add(ColumnType.HEIGHT, AsGradientTableCell(pokeDataToGuess.Height_m, pokeDataGuessed.Height_m, 2));
            dictionaryBuilder.Add(ColumnType.WEIGHT, AsGradientTableCell(pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg, 40));

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

        private static ITableCell AsGradientTableCell(int targetValue, int guessValue, int maxOffsetValue)
        {
            int difference = Math.Min(Math.Abs(guessValue - targetValue), maxOffsetValue);
            int percent = AsPercentLimit100(difference, maxOffsetValue);

            Color correctColor = Color.Green;
            Color wrongColor = Color.Red;

            Color upperColor;
            Color lowerColor;
            if (targetValue < guessValue)
            {
                upperColor = correctColor;
                lowerColor = PercentualColorOffset(correctColor, wrongColor, percent);
            } else
            {
                lowerColor = correctColor;
                upperColor = PercentualColorOffset(correctColor, wrongColor, percent);
            }

            return new GradientTableCell(guessValue.ToString(), upperColor, lowerColor, 0);
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
