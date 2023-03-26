using System.Text;

namespace Pokewordle.Shared
{
    public static class HeaderGenerator
    {
        private static readonly Dictionary<ColumnType, string> s_Headers = new()
        {
            {ColumnType.NAME, "Pokémon"},
            {ColumnType.TYPE1, "Type 1"},
            {ColumnType.TYPE2, "Type 2"},
            {ColumnType.GENERATION, "Gen"},
            {ColumnType.HEIGHT, "Height (m)"},
            {ColumnType.WEIGHT, "Weight (kg)"},
            {ColumnType.EVOLUTION, "Evolution"},
            {ColumnType.ABILITIES, "Abilities"},
        };

        /*
         <tr>
                    <th>Pokémon</th>
                    <th>Type 1</th>
                    <th>Type 2</th>
                    <th>Gen</th>
                    <th>Height (m)</th>
                    <th>Weight (kg)</th>
                    <th>Evolution</th>
                    <th>Abilities</th>
                </tr>
        */

        public static string ToHeaderString(IEnumerable<ColumnType> columnTypes)
        {
            StringBuilder sb = new();
            sb.AppendLine("<tr>");
            foreach(ColumnType columnType in columnTypes)
            {
                sb.Append("<th>");
                if (s_Headers.TryGetValue(columnType, out string? header) && header is not null)
                {
                    sb.Append(header);
                } else
                {
                    sb.Append("ERROR");
                }
                sb.AppendLine("</th>");
            }
            sb.AppendLine("</tr>");
            return sb.ToString();
        }
    }
}
