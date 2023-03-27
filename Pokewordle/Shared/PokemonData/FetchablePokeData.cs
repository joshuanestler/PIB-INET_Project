﻿using PokeApiNet;
using System.Collections.Immutable;

namespace Pokewordle.Shared.PokemonData
{
    public class FetchablePokeData : IPokeData
    {
        public string Name => _nameFetchable.Value;
        private readonly FetchableData<string> _nameFetchable;

        public IImmutableList<string> Types => _typesFetchable.Value;
        private readonly FetchableData<IImmutableList<string>> _typesFetchable;

        public float Height_m => _heightFetchable.Value;
        private readonly FetchableData<float> _heightFetchable;

        public float Weight_kg => _weightFetchable.Value;
        private readonly FetchableData<float> _weightFetchable;

        public IImmutableList<string> Abilities => _abilitiesFetchable.Value;
        private readonly FetchableData<IImmutableList<string>> _abilitiesFetchable;

        private readonly Pokemon apiPokemon;
        public FetchablePokeData(Pokemon pokemon)
        {
            this.apiPokemon = pokemon;
            _nameFetchable = new(() => apiPokemon.Name);
            _typesFetchable = new(() => apiPokemon.Types.ConvertAll(pkmnType => pkmnType.Type.Name).ToImmutableList());
            _heightFetchable = new(() => apiPokemon.Height / 10f);
            _weightFetchable = new(() => apiPokemon.Weight / 10f);
            _abilitiesFetchable = new(() => apiPokemon.Abilities.ConvertAll(pkmnAbility => pkmnAbility.Ability.Name).ToImmutableList());
        }
    }
}