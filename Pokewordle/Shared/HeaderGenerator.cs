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

            {ColumnType.HP, 4},
            {ColumnType.ATK, 4},
            {ColumnType.DEF, 4},
            {ColumnType.SPA, 4},
            {ColumnType.SPD, 4},
            {ColumnType.SPE, 4},
            {ColumnType.BST, 4},

            {ColumnType.MINSTATS, 5},
            {ColumnType.MAXSTATS, 5},
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

            {ColumnType.HP, "HP"},
            {ColumnType.ATK, "Atk"},
            {ColumnType.DEF, "Def"},
            {ColumnType.SPA, "SpA"},
            {ColumnType.SPD, "SpD"},
            {ColumnType.SPE, "Spe"},
            {ColumnType.BST, "BST"},

            {ColumnType.MINSTATS, "Min Stats"},
            {ColumnType.MAXSTATS, "Max Stats"},
        };

        public static string GetHeader(ColumnType columnType)
        {
            if (s_Headers.TryGetValue(columnType, out string? header) && header is not null)
            {
                return header;
            }
            else
            {
                return "ERROR";
            }
        }

        public static string ToHeaderString(IEnumerable<ColumnType> columnTypes)
        {
            StringBuilder sb = new();
            sb.AppendLine("<tr>");
            foreach (ColumnType columnType in columnTypes)
            {
                // sb.Append($"<th style=\"min-width: {GetColumnWidth(columnType)}em; max-width: {GetColumnWidth(columnType)}em;\">");
                sb.Append($"<th style=\"width: {GetColumnWidth(columnType)}em \">");
                sb.Append(GetHeader(columnType));
                sb.AppendLine("</th>");
                //sb.AppendLine("<th/>");
            }
            sb.AppendLine("</tr>");
            return sb.ToString();
        }
    }
}
