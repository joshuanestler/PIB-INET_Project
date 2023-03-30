namespace Pokewordle.Shared.Util
{
    public static class SharedSettings
    {
        public static readonly List<ColumnType> GameColumnTypes = new() { ColumnType.SPRITE, ColumnType.NAME, ColumnType.GENERATION, ColumnType.TYPES, ColumnType.ABILITIES, ColumnType.MAXSTATS, ColumnType.HEIGHT };

        /// <summary>
        /// Allow the Player to set an amount of fields that will randomly be selected to be hidden per guess.
        /// </summary>
        public static int Obfuscation = 1;

        /// <summary>
        /// If greater than 0, the earlier guesses will be hidden.
        /// </summary>
        public static int GuessHistoryLimit = 0;

        /// <summary>
        /// Prevent pokemon from being guessed multiple times.
        /// Only matters if Obfuscation > 0 or GuessHistoryLimit > 0
        /// </summary>
        public static bool AllowDuplicateGuesses = false;

    }
}
