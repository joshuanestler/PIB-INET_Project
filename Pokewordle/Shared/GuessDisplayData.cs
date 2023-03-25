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
            dictionaryBuilder.Add(ColumnType.HEIGHT, AsGradientTableCell(pokeDataToGuess.Height_m, pokeDataGuessed.Height_m));
            dictionaryBuilder.Add(ColumnType.WEIGHT, AsGradientTableCell(pokeDataToGuess.Weight_kg, pokeDataGuessed.Weight_kg));

            ColumnData = dictionaryBuilder.ToImmutable();
        }

        private static ITableCell AsGradientTableCell(int targetValue, int guessValue)
        {
            return new MoreLessTableCell(guessValue.ToString(), Color.Red, Color.Green, 0);
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
