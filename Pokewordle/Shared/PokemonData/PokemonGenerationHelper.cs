using System.Text;

namespace Pokewordle.Shared.PokemonData
{
    public static class PokemonGenerationHelper
    {
        private static readonly List<int> s_generationLowerBound = new();
        private static readonly List<int> s_generationUpperBound = new();

        public static void Initialize(in int maxPokemonId)
        {
            for(int i = 0; i < 10; i++)
            {
                s_generationLowerBound.Add(0);
                s_generationUpperBound.Add(maxPokemonId);
            }
        }

        public static void AddInformation(in int pokemonId, int generation)
        {
            generation = generation - 1;
            int upperboundBelow = pokemonId - 1;
            for(int i = 0; i < generation; i++)
            {
                s_generationUpperBound[i] = Math.Min(s_generationUpperBound[i], upperboundBelow);
            }

            int lowerboundAbove = pokemonId + 1;
            for (int i = 0; i < generation; i++)
            {
                s_generationLowerBound[i] = Math.Max(s_generationLowerBound[i], lowerboundAbove);
            }

        }

        public static bool TryGetRandomIdWithinGenerations(in ISet<int> generations, out int id)
        {
            //TODO: very primitive implementation, can be improved a lot
            List<int> values = new();
            foreach(int generation in generations)
            {
                for(int i = s_generationLowerBound[generation - 1]; i <= s_generationUpperBound[generation - 1]; i++)
                {
                    values.Add(i);
                }
            }

            if (values.Count > 0)
            {
                id = values[Random.Shared.Next(values.Count)];
                return true;
            }
            id = 0;
            return false;

        }

        private static int[] GenerationFirstIndex = { 0, 151, 251, 386, 493, 649, 721, 809, 905, 1010 };
        public static bool TryGetRandomIdWithinGenerationsV2(in ISet<int> generations, out int id)
        {
            StringBuilder sb = new();
            List<int> values = new();
            foreach (int generation in generations)
            {
                sb.Append(generation);
                sb.Append(' ');
                for (int i = GenerationFirstIndex[generation - 1]; i < GenerationFirstIndex[generation]; i++)
                {
                    values.Add(i);
                }
            }

            if (values.Count > 0)
            {
                id = values[Random.Shared.Next(values.Count)];
                Console.WriteLine($"Selected random pokemon (id={id}) within generations: {sb}");
                return true;
            }
            id = 0;
            return false;

        }

    }
}
