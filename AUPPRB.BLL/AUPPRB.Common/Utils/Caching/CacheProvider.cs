using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Common.Utils.Caching;


namespace AUPPRB.Common.Utils.Caching
{
    public sealed class CacheProvider : ICacheProvider
    {
        private static readonly MemoryCache MemoryCache = MemoryCache.Default;

        static CacheProvider()
        {
            Current = new CacheProvider();
        }

        private CacheProvider() { }

        public static ICacheProvider Current { get; private set; }

        public ICache GetCache()
        {
            var cache = new Cache(MemoryCache);
            var result = cache as ICache;

            return result;
        }
    }
}
