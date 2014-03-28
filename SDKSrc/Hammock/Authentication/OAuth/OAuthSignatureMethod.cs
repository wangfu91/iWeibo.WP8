using System;
using System.Runtime.Serialization;

namespace TencentWeiboSDK.Hammock.Authentication.OAuth
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public enum OAuthSignatureMethod
    {
#if !SILVERLIGHT && !Smartphone && !ClientProfiles && !NET20 && !MonoTouch
        [EnumMember] HmacSha1,
        [EnumMember] PlainText,
        [EnumMember] RsaSha1
#else
        HmacSha1,
        PlainText,
        RsaSha1
#endif
    }
}