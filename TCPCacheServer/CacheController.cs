using System;
using System.Collections.Specialized;
using System.Text;

namespace TCPCacheServer
{
    public class CacheController
    {
        private OrderedDictionary dictionary;
        private const long cacheLimit = 128 * 1024 * 1024;
        private long currentCacheSize;        
          
        public CacheController()
        {
            dictionary = new OrderedDictionary();
        }

        public void Add(string key, string value)
        {
            var valueSize = Encoding.UTF8.GetByteCount(value);

            var isExist = Get(key) != null;

            if (isExist)
            {
                currentCacheSize -= Encoding.UTF8.GetByteCount(Get(key));
                dictionary.Remove(key);
            }
            if (valueSize + currentCacheSize <= cacheLimit)
            {
                currentCacheSize += valueSize;

                dictionary.Add(key, value);
            }
            else if (valueSize <= cacheLimit)
            {
                for (int i = 0; i < dictionary.Count; i++)
                {
                    currentCacheSize -= Encoding.UTF8.GetByteCount((string)dictionary[i]);

                    dictionary.RemoveAt(i);

                    if (valueSize + currentCacheSize <= cacheLimit)
                    {
                        currentCacheSize += valueSize;

                        dictionary.Add(key, value);

                        break;
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
