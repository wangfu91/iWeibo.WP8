using System;
using System.Runtime.Serialization;

namespace TencentWeiboSDK.Hammock.Tasks
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public enum RateLimitType
    {
#if !SILVERLIGHT && !Smartphone && !ClientProfiles && !NET20 && !MonoTouch
        [EnumMember] ByPercent,
        [EnumMember] ByPredicate
#else
        ByPercent,
        ByPredicate
#endif
    }
}