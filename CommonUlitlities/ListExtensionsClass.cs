using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUlitlities
{
    public static class ListExtensionsClass
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }

        public static class Lists
        {
            public static List<T> Of<T>(T item)
            {
                return new List<T> { item };
            }

        }
    }
}
