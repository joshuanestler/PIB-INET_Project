namespace Pokewordle.Shared
{
    public class FetchableData<TOut>
    {
        private TOut _value;
        private bool _valueFetched = false;
        public TOut Value { get => _valueFetched ? _value : FetchValue(); }

        private Func<TOut> _ValueGetter;

        public FetchableData(Func<TOut> valueGetter)
        {
            this._ValueGetter = valueGetter;
        }

        private TOut FetchValue()
        {
            _value = _ValueGetter.Invoke();
            _valueFetched = true;
            return _value;
        }
    }

}
