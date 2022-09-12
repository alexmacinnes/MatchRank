using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchRank
{
    internal static class ExtensionMethods
    {
        public static V CreateOrAdd<K,V>(this Dictionary<K,V> dictionary, K key, Func<V> valueFunc)
            where K : notnull
            where V : notnull
        {
            V? value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = valueFunc();
                dictionary[key] = value;
            }

            return value;
        }
    }
}
