using System;

namespace TencentWeiboSDK.Hammock.Caching
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public static class CacheFactory
    {
        public static ICache InMemoryCache
        {
            get { return new SimpleCache(); }
        }

#if SILVERLIGHT
        public static ICache IsolatedStorageCache
        {
            get { throw new NotImplementedException(); }
        }
#endif
    }
}