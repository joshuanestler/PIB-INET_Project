namespace Pokewordle.Shared.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach(T item in items)
            {
                action.Invoke(item);
            }
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, Predicate<T> filter)
        {
            foreach (T item in items)
            {
                if (filter.Invoke(item))
                {
                    yield return item;
                }
            }
        }


    }
}
