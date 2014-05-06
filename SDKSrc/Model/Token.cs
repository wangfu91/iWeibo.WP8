using System.Runtime.Serialization;
using TencentWeiboSDK.Util;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// Access Toekn 类，用于表示服务器所返回的 Access Token 对象.
    /// </summary>
    [DataContract]
    public class TencentAccessToken
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="content">回调的字符串.</param>
        internal TencentAccessToken(string content)
        {

            var result = OAuthHelper.GetQueryParameters(content);

            // 通过content, 初始化 Key 和 Secret
            if (result.ContainsKey("access_token")) this.AccessToken = result["access_token"];
            if (result.ContainsKey("expire_in")) this.ExpiresIn = result["expire_in"];
            if (result.ContainsKey("refresh_token")) this.RefreshToken = result["refresh_token"];
            if (result.ContainsKey("openid")) this.OpenId = result["openid"];
            if (result.ContainsKey("name")) this.Name = result["name"];
            if (result.ContainsKey("nick")) this.Nick = result["nick"];

        }

        /// <summary>
        /// 构造函数，用于反序列化.
        /// </summary>
        public TencentAccessToken()
        { }

        /// <summary>
        /// 访问第三方资源的凭证 
        /// </summary>
        [DataMember(Name = "access_token", IsRequired = true)]
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// accesstoken过期时间，以返回的时间的准，单位为秒，注意过期时提醒用户重新授权 
        /// </summary>
        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }

        /// <summary>
        /// 刷新token 
        /// </summary>
        [DataMember(Name = "refresh_token")]
        public virtual string RefreshToken { get; set; }

        /// <summary>
        /// 用户统一标识，可以唯一标识一个用户
        /// </summary>
        [DataMember(Name = "openid", IsRequired = false)]
        public string OpenId { get; set; }

        /// <summary>
        /// 授权用户的用户名
        /// </summary>
        [DataMember(Name = "name", IsRequired = false)]
        public string Name { get; set; }

        /// <summary>
        /// 授权用户的昵称
        /// </summary>
        [DataMember(Name = "nick", IsRequired = false)]
        public string Nick { get; set; }

    }

    /// <summary>
    /// Request Toekn 类，用于表示服务器所返回的 Request Token 对象.
    /// </summary>
    public class AuthorizationCode
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="content">请求 Authorization Code 后, 服务器返回的结果.</param>
        public AuthorizationCode(string content)
        {
            var result = OAuthHelper.GetQueryParameters(content);

            // 通过content, 初始化 Key 和 Secret
            if (result.ContainsKey("code")) this.Code = result["code"];
            if (result.ContainsKey("openid")) this.OpenId = result["openid"];
            if (result.ContainsKey("openkey")) this.OpenKey = result["openkey"];
        }

        /// <summary>
        /// 用来换取accesstoken的授权码，有效期为10分钟 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 用户统一标识，可以唯一标识一个用户 
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 与openid对应的用户key，是验证openid身份的验证密钥 
        /// </summary>
        public string OpenKey { get; set; }
    }

}
