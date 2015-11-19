using System;
using System.Linq;
using System.Runtime.Caching;


namespace AUPPRB.Common.Utils.Caching
{
    public class Cache : ICache
    {
        private readonly MemoryCache _cache;

        /// <summary>
        /// Creates new instance of <see cref="Cache"/> using provided parameters.
        /// </summary>
        /// <param name="cache">Common <see cref="Cache"/> for items storing.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Cache(MemoryCache cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }
            _cache = cache;
        }

        /// <summary>
        /// Get item with specified key.
        /// </summary>
        /// <param name="key">Key to get cached item by.</param>
        /// <param name="value">Item stored in cache by specified key.</param>
        /// <returns>True if item with provided key exists in cache; otherwise false.</returns>
        private bool TryGet(string key, out object value)
        {
            value = _cache.Get(CreateKey(key));
            var result = value != null;

            return result;
        }

        /// <summary>
        /// Puts item in cache using specified key.
        /// </summary>
        /// <param name="key">Key to store item by.</param>
        /// <param name="value">Item to store in cache. Can't be null.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Put(string key, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var cacheItem = new CacheItem(CreateKey(key), value);
            var cachePolicy = new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable };

            _cache.Add(cacheItem, cachePolicy);
        }

        /// <summary>
        /// Puts item in cache using specified key.
        /// </summary>
        /// <param name="key">Key to store item by.</param>
        /// <param name="value">Item to store in cache. Can't be null.</param>
        /// <param name="expires">Time to store item in cache for.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Put(string key, object value, TimeSpan expires)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var cacheItem = new CacheItem(CreateKey(key), value);
            var cachePolicy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + expires };

            _cache.Add(cacheItem, cachePolicy);
        }

        /// <summary>
        /// Remove item with specified key from cache.
        /// </summary>
        /// <param name="key">Key of cached item to remove.</param>
        public void Remove(string key)
        {
            _cache.Remove(CreateKey(key));
        }

        /// <summary>
        /// Remove item start with specified key
        /// </summary>
        /// <param name="key"></param>
        public void RemoveStartWith(string key)
        {
            foreach (var cachedItem in _cache.Where(cachedItem => cachedItem.Key.StartsWith(key)))
            {
                _cache.Remove(cachedItem.Key);
            }
        }

        /// <summary>
        /// Delete all items from current cache.
        /// </summary>
        public void Clear()
        {
            foreach (var cachedItem in _cache)
            {
                _cache.Remove(cachedItem.Key);
            }
        }

        /// <summary>
        /// Create bucket-specific key to store items in common <see cref="Cache"/>
        /// </summary>
        /// <param name="key">Per bucket key.</param>
        /// <returns>Key for common <see cref="Cache"/></returns>
        private string CreateKey(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key");
            }

            var result = key;
            return result;
        }

        public bool TryGet<T>(string key, out T result)
        {
            object value;
            if (TryGet(key, out value))
            {
                result = (T)value;
                return true;
            }

            result = default(T);

            return false;
        }

        /// <summary>
        /// Tries to get item with specified key from cache; otherwise stores value provided by factory in cache and then returns it.
        /// If factory can't provide value, it should call action passed as a parameter. Null item value is forbidden.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key of item to get.</param>
        /// <param name="factory">Factory, to provide item to cache, if cache has no items with such key.</param>
        /// <returns>Item from cache, or provided by factory.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public T GetOrAdd<T>(string key, Func<T> factory)
        {
            T result;
            if (!TryGet(key, out result))
            {
                result = factory();
                if (!ReferenceEquals(result, null))
                {
                    Put(key, result);
                }
            }

            return result;
        }

        public void Add<T>(string key, T result)
        {
            Put(key, result);
        }

        public void Add<T>(string key, T result, TimeSpan expires)
        {
            Put(key, result, expires);
        }

        /// <summary>
        /// Tries to get item with specified key from cache; otherwise stores value provided by factory in cache and then returns it.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key of item to get.</param>
        /// <param name="factory">Factory, to provide item to cache, if cache has no items with such key.</param>
        /// <param name="expires">Time for which item should be stored in cache.</param>
        /// <returns>Item from cache, or provided by factory.</returns>
        public T GetOrAdd<T>(string key, Func<T> factory, TimeSpan expires)
        {
            T result;
            if (!TryGet(key, out result))
            {
                result = factory();
                if (!ReferenceEquals(result, null))
                {
                    Put(key, result, expires);
                }
            }
            return result;
        }
    }
}
