using System;

namespace TencentWeiboSDK.Hammock.Caching
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CacheOptions
    {
        public virtual CacheMode Mode { get; set; }
        public virtual TimeSpan Duration { get; set; }
    }
}
