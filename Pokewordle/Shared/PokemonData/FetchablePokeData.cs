using PokeApiNet;
using Pokewordle.Shared.Extensions;
using System.Collections.Immutable;

namespace Pokewordle.Shared.PokemonData
{

    public class FetchablePokeData : IPokeData
    {
        public string Name { get; }

        public IImmutableList<string> Types { get; }

        public IImmutableList<string> FilledTypes { get; }

        private int _generation = 0;
        private bool _generationFetched = false;

        public float Height_m { get; }

        public float Weight_kg { get; }

        public int HP { get; }
        public int Atk { get; }
        public int Def { get; }
        public int SpA { get; }
        public int SpD { get; }
        public int Spe { get; }

        public int BST { get; }

        public IImmutableList<string> MaxStatNames { get; }
        public IImmutableList<string> MinStatNames { get; }

        public IImmutableList<string> Abilities { get; }

        public string SpriteUrl { get; }

        private readonly Pokemon apiPokemon;
        private readonly PokeApiClient pokeApiClient;

        public FetchablePokeData(Pokemon pokemon, PokeApiClient pokeApiClient)
        {
            this.pokeApiClient = pokeApiClient;
            this.apiPokemon = pokemon;

            Name = apiPokemon.Name;
            Types = apiPokemon.Types.ConvertAll(type => type.Type.Name).ToImmutableList();
            FilledTypes =PokemonDataHelper.BuildTypeList(Types, 2);
            Height_m = apiPokemon.Height / 10f;
            Weight_kg = apiPokemon.Weight / 10f;

            HP = apiPokemon.Stats[0].BaseStat;
            Atk = apiPokemon.Stats[1].BaseStat;
            Def = apiPokemon.Stats[2].BaseStat;
            SpA = apiPokemon.Stats[3].BaseStat;
            SpD = apiPokemon.Stats[4].BaseStat;
            Spe = apiPokemon.Stats[5].BaseStat;

            BST = HP + Atk + Def + SpA + SpD + Spe;

            int[] stats = { HP, Atk, Def, SpA, SpD, Spe };
            string[] statNames = { "HP", "Atk", "Def", "SpA", "SpD", "Spe" };
            int minStat = stats.Aggregate(HP, Math.Min);
            int maxStat = stats.Aggregate(HP, Math.Max);
            List<string> minStatNames = new();
            List<string> maxStatNames = new();
            for(int i = 0; i < stats.Length; i++)
            {
                if (stats[i] == minStat) minStatNames.Add(statNames[i]);
                if (stats[i] == maxStat) maxStatNames.Add(statNames[i]);
            }
            MinStatNames = minStatNames.ToImmutableList();
            MaxStatNames = maxStatNames.ToImmutableList();

            Abilities = apiPokemon.Abilities.ConvertAll(pkmnAbility => pkmnAbility.Ability.Name).ToImmutableList();
            SpriteUrl = apiPokemon.Sprites.FrontDefault;
        }

        public async Task<int> GetGeneration()
        {
            if (_generationFetched)
            {
                return _generation;
            }
            _generation = (await pokeApiClient.GetGeneration(apiPokemon)).Id;
            return _generation;
        }

    }
}
