using TencentWeiboSDK.Util;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// Access Token 和 Request Token 的基类.
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="content">回调的字符串.</param>
        internal Token(string content)
        {

            var result = OAuthHelper.GetQueryParameters(content);

            // 通过content, 初始化 Key 和 Secret
            if (result.ContainsKey("oauth_token") && result.ContainsKey("oauth_token_secret"))
            {
                this.TokenKey = result["oauth_token"];
                this.TokenSecret = result["oauth_token_secret"];
            }
        }

        /// <summary>
        /// 构造函数，用于反序列化.
        /// </summary>
        public Token()
        { }

        /// <summary>
        /// 获取或设置 Toekn 的 Key.
        /// </summary>
        public virtual string TokenKey { get; set; }

        /// <summary>
        /// 获取或设置 Toekn 的 Secret.
        /// </summary>
        public virtual string TokenSecret { get; set; }

    }

    /// <summary>
    /// Request Toekn 类，用于表示服务器所返回的 Request Token 对象.
    /// </summary>
    public class RequestToken : Token
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="content">请求 Request Token 后, 服务器返回的结果.</param>
        public RequestToken(string content)
            : base(content)
        {
            this.AccessUrl = BuildAccessUri(TokenKey);
        }

        private string BuildAccessUri(string key)
        {
            return string.Format(@"https://open.t.qq.com/cgi-bin/authorize?oauth_token={0}", key);
        }

        /// <summary>
        /// 获取或设置验证码.
        /// </summary>
        public string Verifier { get; set; }

        /// <summary>
        /// 获取或设置请求用户授址地址.
        /// </summary>
        public string AccessUrl { get; set; }
    }

    /// <summary>
    /// Access Toekn 类，用于表示服务器所返回的 Access Token 对象.
    /// </summary>
    public class TencentAccessToken : Token
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        public TencentAccessToken()
        { }
        
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="content">请求 Access Token 后, 服务器返回的结果.</param>
        public TencentAccessToken(string content)
            : base(content)
        {
            var result = OAuthHelper.GetQueryParameters(content);
            this.Name = result["name"];
            this.Open = true;
        }

        /// <summary>
        /// 获取或设置用户头像.
        /// </summary>
        public string Head { get; set; }

        /// <summary>
        /// 获取或设置 Toekn 用户名.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当前登录用户标识
        /// </summary>
        public bool Open { get; set; }
    }
}
