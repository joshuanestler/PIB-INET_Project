using System.Xml.Linq;

namespace Pokewordle.Shared.Util
{
    public static class ThingsThatShouldExist
    {
        public static TElement[] CreateInitializedArray<TElement>(int length) where TElement : new()
        {
            TElement[] arr = new TElement[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = new();
            }
            return arr;
        }
        public static TElement[] CreateInitializedArray<TElement>(int length, Action<TElement> postCreateAction) where TElement : new()
        {
            TElement[] arr = new TElement[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = new();
                postCreateAction(arr[i]);
            }
            return arr;
        }
    }
}
