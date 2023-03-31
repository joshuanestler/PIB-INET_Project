namespace Pokewordle.Shared
{
    public enum ColumnType
    {
        SPRITE,
        NAME,
        NAME_LOCAL,
        TYPE1,
        TYPE2,
        TYPES,
        GENERATION,
        HEIGHT,
        WEIGHT,
        EVOLUTION,
        ABILITIES,

        HP,
        ATK,
        DEF,
        SPA,
        SPD,
        SPE,
        BST,

        MAXSTATS,
        MINSTATS
    }

    public static class ColumnTypeUtils {
        // String -> ColumnType
        public static ColumnType FromString(string str) {
        
            bool tryParse = Enum.TryParse<ColumnType>(str, out ColumnType columnType);
            if (!tryParse) {
                columnType = ColumnType.NAME;
            }
            return columnType;
        }
        
        public static bool TryParseString(string str, out ColumnType columnType) {
            return Enum.TryParse<ColumnType>(str, out columnType);
        }
    }
}

