using System.Drawing;

namespace Pokewordle.Shared
{
    public class PokeData
    {
        public string Name { get; set; }
        
        public string Type1 { get; set; }
        public Color Type1Color { get; set; }
        public string Type1ColorStr { get => ConvertToHex(Type1Color); }

        public string? Type2 { get; set; }
        public Color? Type2Color { get; set; }
        public string Type2ColorStr { get => Type1Color.ToString(); }

        public int Generation { get; set; }

        public int Height_m { get; set; }
        public int Weight_kg { get; set; }
        //public int EvolutionType { get; set; }

        public List<string> Abilities { get; set; }

        private static String ConvertToHex(System.Drawing.Color? c)
        {
            if (c is null)
            {
                return "#AAAAAA";
            }
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

    }
}
