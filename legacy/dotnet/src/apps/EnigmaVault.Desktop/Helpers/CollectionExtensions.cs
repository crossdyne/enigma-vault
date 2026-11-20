using System.Collections.ObjectModel;

namespace EnigmaVault.Desktop.Helpers
{
    internal static class CollectionExtensions
    {
        public static void Load<T>(this ICollection<T> source, IEnumerable<T> data,bool isClear = false)
        {
            if (isClear)
                source.Clear();

            foreach (var item in data)
                source.Add(item);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> data)
        {
            var obs = new ObservableCollection<T>();

            foreach (var item in data)
                obs.Add(item);

            return obs;
        }
    }
}