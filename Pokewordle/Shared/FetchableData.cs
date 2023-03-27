namespace Pokewordle.Shared
{
    public class FetchableData<TOut>
    {
        private TOut _value = null;
        public TOut Value { get => _value ?? GetValue(); }

        private Func<TOut> _ValueGetter;

        public FetchableData(Func<TOut> valueGetter)
        {
            this._ValueGetter = valueGetter;
        }

        private TOut? GetValue()
        {
            _value = _ValueGetter.Invoke();
            return _value;
        }

    }
}
