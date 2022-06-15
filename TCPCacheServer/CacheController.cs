using System;
using System.Collections.Specialized;
using System.Text;

namespace TCPCacheServer
{
    public class CacheController
    {
        private OrderedDictionary dictionary;                   //using OrderedDictionary for implementation “First-In, First-Out”
        private const long cacheLimit = 128 * 1024 * 1024;      //method on evicting entries from the cache
        private long currentCacheSize;        
          
        public CacheController()
        {
            dictionary = new OrderedDictionary();
        }

        public void Add(string key, string value)
        {
            var valueSize = Encoding.UTF8.GetByteCount(value);

            var isExist = Get(key) != null;

            if (isExist)                                        //remove entry for replacing by new one
            {
                currentCacheSize -= Encoding.UTF8.GetByteCount(Get(key));
                dictionary.Remove(key);
            }

            if (valueSize + currentCacheSize <= cacheLimit)     //when cache has enough space
            {
                currentCacheSize += valueSize;
                dictionary.Add(key, value);
            }
            else if (valueSize <= cacheLimit)                   //when not enough space, just check that entry can fit into the cache
            {
                for (int i = 0; i < dictionary.Count; i++)      //release of space in the cache by deleting one by one entry
                {
                    currentCacheSize -= Encoding.UTF8.GetByteCount((string)dictionary[i]);
                    dictionary.RemoveAt(i);

                    if (valueSize + currentCacheSize <= cacheLimit)  
                    {
                        currentCacheSize += valueSize;
                        dictionary.Add(key, value);

                        break;                                  //now new entry in cache, stop releasing space
                    }
                }
            }
            else
            {
                throw new ArgumentException($"You trying to add element which bigger than cache limit ({cacheLimit} bytes)");
            }

            Console.WriteLine($"Current cache size is {currentCacheSize}");

        }

#nullable enable
        public string? Get(string key)
        {
            if (dictionary[key] == null) return null;

            return dictionary[key] as string;            
        }
#nullable disable

    }
}
