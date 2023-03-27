using System.Drawing;

namespace Pokewordle.Shared
{
    public enum MatchingResult
    {
        ALL,
        PARTIAL,
        PARTIAL_1_OF_2,
        PARTIAL_1_OF_3,
        PARTIAL_2_OF_3,
        NONE,
    }

    public static class MatchingResultExtensions
    {
        public static Color ToTruePartialFalseColor(this MatchingResult matchingResult)
        {
            return matchingResult switch
            {
                MatchingResult.NONE => ColorScheme.COLOR_MISTAKE,
                MatchingResult.ALL => ColorScheme.COLOR_CORRECT,
                _ => ColorScheme.COLOR_SEMI_CORRECT_MISTAKE
            };
        }

        public static Color ToBinaryColor(this MatchingResult matchingResult)
        {
            if (matchingResult == MatchingResult.ALL)
            {
                return ColorScheme.COLOR_CORRECT;
            }
            return ColorScheme.COLOR_MISTAKE;
        }

    }
}
