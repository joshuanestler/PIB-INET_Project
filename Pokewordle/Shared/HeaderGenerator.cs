using System.Text;

namespace Pokewordle.Shared
{
    public static class HeaderGenerator
    {
        private static readonly Dictionary<ColumnType, float> s_ColumnWidths = new()
        {
            {ColumnType.SPRITE, 3},
            {ColumnType.NAME, 10},
            {ColumnType.TYPE1, 6},
            {ColumnType.TYPE2, 6},
            {ColumnType.TYPES, 6},
            {ColumnType.GENERATION, 5},
            {ColumnType.HEIGHT, 5},
            {ColumnType.WEIGHT, 5},
            {ColumnType.EVOLUTION, 8},
            {ColumnType.ABILITIES, 5},
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
            {ColumnType.SPRITE, "Pokémon"},
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
            foreach(ColumnType columnType in columnTypes)
            {
                sb.Append($"<th style=\"min-width: {GetColumnWidth(columnType)}em; max-width: {GetColumnWidth(columnType)}em;\">");
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
            sb.AppendLine("<th/>");
            sb.AppendLine("</tr>");
            return sb.ToString();
        }
    }
}
