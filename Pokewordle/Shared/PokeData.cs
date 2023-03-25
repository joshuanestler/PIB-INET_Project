using System.Drawing;

namespace Pokewordle.Shared
{
    public class PokeData
    {
        public readonly string Name;

        public readonly string Type1;
        public readonly Color Type1Color;
        public string Type1ColorStr => ConvertToHex(Type1Color);

        public readonly string? Type2;
        public readonly Color? Type2Color;
        public string Type2ColorStr => ConvertToHex(Type2Color);

        public int Generation { get; set; }

        public readonly int Height_m;
        public int Weight_kg { get; set; }
        //public int EvolutionType { get; set; }

        public List<string> Abilities { get; set; }

        private static string ConvertToHex(System.Drawing.Color? c)
        {
            if (c is null) {
                return "#AAAAAA";
            }
            
            Color color = (Color)c;
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}
