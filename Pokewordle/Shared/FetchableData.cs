using PokeApiNet;

namespace Pokewordle.Shared
{
    public class FetchableData<TOut>
    {
        private TOut _value = default;
        private bool _valueFetched = false;

        private Func<Task<TOut>> _ValueGetter;

        public FetchableData(Func<Task<TOut>> valueGetter)
        {
            this._ValueGetter = valueGetter;
        }

        public async Task<TOut> FetchValue()
        {
            if (_valueFetched) {
                return _value;
            }
            _value = await _ValueGetter.Invoke();
            _valueFetched = true;
            return _value;
        }
    }
}
