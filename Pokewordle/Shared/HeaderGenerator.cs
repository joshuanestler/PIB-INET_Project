using System.Text;

namespace Pokewordle.Shared
{
    public static class HeaderGenerator
    {
        private static readonly Dictionary<ColumnType, float> s_ColumnWidths = new()
        {
            {ColumnType.SPRITE, 2},
            {ColumnType.NAME, 8},
            {ColumnType.TYPE1, 5},
            {ColumnType.TYPE2, 5},
            {ColumnType.TYPES, 5},
            {ColumnType.GENERATION, 3},
            {ColumnType.HEIGHT, 4},
            {ColumnType.WEIGHT, 4},
            {ColumnType.EVOLUTION, 8},
            {ColumnType.ABILITIES, 4},
        };

        public static float GetColumnWidth(ColumnType columnType)
        {
            if (s_ColumnWidths.TryGetValue(columnType, out float value))
            {
                return value;
            }
            return 1;
        }

        private static readonly Dictionary<ColumnType, string> s_Headers = new()
        {
            {ColumnType.SPRITE, "Sprite"},
            {ColumnType.NAME, "Pokémon"},
            {ColumnType.TYPE1, "Type 1"},
            {ColumnType.TYPE2, "Type 2"},
            {ColumnType.TYPES, "Type"},
            {ColumnType.GENERATION, "Gen"},
            {ColumnType.HEIGHT, "Height (m)"},
            {ColumnType.WEIGHT, "Weight (kg)"},
            {ColumnType.EVOLUTION, "Evolution"},
            {ColumnType.ABILITIES, "Abilities"},
        };

        public static string ToHeaderString(IEnumerable<ColumnType> columnTypes)
        {
            StringBuilder sb = new();
            sb.AppendLine("<tr>");
            foreach (ColumnType columnType in columnTypes)
            {
                // sb.Append($"<th style=\"min-width: {GetColumnWidth(columnType)}em; max-width: {GetColumnWidth(columnType)}em;\">");
                sb.Append($"<th style=\"width: {GetColumnWidth(columnType)}em \">");
                if (s_Headers.TryGetValue(columnType, out string? header) && header is not null)
                {
                    sb.Append(header);
                } else
                {
                    sb.Append("ERROR");
                }
                sb.AppendLine("</th>");
                //sb.AppendLine("<th/>");
            }
            sb.AppendLine("</tr>");
            return sb.ToString();
        }
    }
}
