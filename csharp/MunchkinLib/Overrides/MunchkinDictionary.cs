using System.Collections.Generic;

namespace MunchkinLib.Overrides
{
    public class MunchkinDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> dictionary;

        public MunchkinDictionary()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        public new TValue this[TKey key]
        {
            get
            {
                if (!dictionary.ContainsKey(key))
                    dictionary.Add(key, default(TValue));

                return dictionary[key];
            }
        }
    }
}
