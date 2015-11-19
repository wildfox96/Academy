using System;

namespace AUPPRB.Common.Utils.Caching
{
    public interface ICache
    {
        /// <summary>
        /// Puts item in cache using specified key.
        /// </summary>
        /// <param name="key">Key to store item by.</param>
        /// <param name="value">Item to store in cache. Can't be null.</param>
        void Put(string key, object value);

        /// <summary>
        /// Puts item in cache using specified key.
        /// </summary>
        /// <param name="key">Key to store item by.</param>
        /// <param name="value">Item to store in cache. Can't be null.</param>
        /// <param name="expires">Time to store item in cache for.</param>
        void Put(string key, object value, TimeSpan expires);

        /// <summary>
        /// Remove item with specified key from cache.
        /// </summary>
        /// <param name="key">Key of cached item to remove.</param>
        void Remove(string key);

        /// <summary>
        /// Remove item with specified key from cache.
        /// </summary>
        /// <param name="key">Key of cached item to remove.</param>
        void RemoveStartWith(string key);

        /// <summary>
        /// Delete all items from current cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Get item with specified key.
        /// </summary>
        /// <param name="key">Key to get cached item by.</param>
        /// <param name="result">Item stored in cache by specified key.</param>
        /// <returns>True if item with provided key exists in cache; otherwise false.</returns>
        bool TryGet<T>(string key, out T result);

        /// <summary>
        /// Tries to get item with specified key from cache; otherwise stores value provided by factory in cache and then returns it.
        /// If factory can't provide value, it should call action passed as a parameter. Null item value is forbidden.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key of item to get.</param>
        /// <param name="factory">Factory, to provide item to cache, if cache has no items with such key.</param>
        /// <returns>Item from cache, or provided by factory.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        T GetOrAdd<T>(string key, Func<T> factory);
        void Add<T>(string key, T factory);
        void Add<T>(string key, T factory, TimeSpan expires);

        /// <summary>
        /// Tries to get item with specified key from cache; otherwise stores value provided by factory in cache and then returns it.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="key">Key of item to get.</param>
        /// <param name="factory">Factory, to provide item to cache, if cache has no items with such key.</param>
        /// <param name="expires">Time for which item should be stored in cache.</param>
        /// <returns>Item from cache, or provided by factory.</returns>
        T GetOrAdd<T>(string key, Func<T> factory, TimeSpan expires);
    }
}
