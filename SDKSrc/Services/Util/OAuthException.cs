using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TencentWeiboSDK.Services.Util
{
    /// <summary>
    /// 当 OAuth 授权时发生任何错误时，则会抛出该异常.
    /// </summary>
    public class OAuthException : Exception
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="message">错误信息.</param>
        public OAuthException(string message)
            : base(message)
        { 
            
        }
    }

    /// <summary>
    /// 在 OAuth 授权时，若发现未设置 AppKey 或 AppSecret 则抛出该异常.
    /// </summary>
    public class MissingAppKeyOrAppSecretException : OAuthException
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        public MissingAppKeyOrAppSecretException()
            : this("未配置 AppKey 或 AppSecret")
        { }

        /// <summary>.
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息.</param>
        public MissingAppKeyOrAppSecretException(string message)
            : base(message)
        { }
    }

    /// <summary>
    /// 在 OAuth 授权时，若发现缺少 Access Token 则抛出该异常.
    /// </summary>
    public class MissingAccessTokenExcception : OAuthException
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        public MissingAccessTokenExcception()
            : this("用户未登录或未配置 OAuthConfigruation.AccessToken!")
        { }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="message">错误信息.</param>
        public MissingAccessTokenExcception(string message)
            : base(message)
        { }
    }
}
