using System;
using System.Runtime.Serialization;

namespace TencentWeiboSDK.Hammock.Authentication.OAuth
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public enum OAuthType
    {
#if !SILVERLIGHT && !Smartphone && !ClientProfiles && !NET20 && !MonoTouch
        [EnumMember] RequestToken,
        [EnumMember] AccessToken,
        [EnumMember] ProtectedResource,
        [EnumMember] ClientAuthentication
#else
        RequestToken,
        AccessToken,
        ProtectedResource,
        ClientAuthentication
#endif
    }
}