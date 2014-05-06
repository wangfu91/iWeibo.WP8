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
using System.Collections.Generic;
using TencentWeiboSDK.Model;
using System.IO.IsolatedStorage;
using System.Threading;
using TencentWeiboSDK.Util;
using TencentWeiboSDK.Services.Util;

namespace TencentWeiboSDK
{
    /// <summary>
    /// OAuthConfigruation 类用于配置以及验证 OAuth 相关的设置.
    /// </summary>
    public static class OAuthConfigruation
    {
        static OAuthConfigruation()
        {
            IfSaveAccessToken = true;
            Format = DataFormat.Json;
            Authority = "http://open.t.qq.com/api/";
            
            //AccessTokens = null;
        }
        
        /// <summary>
        /// 获取或设置你的应用的 App Key.
        /// </summary>
        public static string APP_KEY {get;set;}
        //    get;
        //    set { value = "801074323"; }
        //}

        /// <summary>
        /// 获取或设置你的应用的 App Secret.
        /// </summary>
        public static string APP_SECRET {get;set;}
        //{
        //    get;
        //    set { value = "64e590bfd90725c47759155a2349d191"; }
        //}
        ///// <summary>
        /// 获取腾迅微博 Open API 的根地址.
        /// </summary>
        /// <remarks>
        /// 默认为 http://open.t.qq.com/api/
        /// </remarks>
        public static string Authority { get; private set; }

        /// <summary>
        /// 获取或设置所希望 API 返回的数据格式.
        /// </summary>
        /// <remarks>
        /// 默认为 Json
        /// </remarks>
        public static DataFormat Format { get; set; }

        /// <summary>
        /// 获取或设置当前用户的 AccessToken.
        /// </summary>
        /// <remarks>若 IfSaveAccessToken == true, 则请不要手动设置该值.</remarks>
        public static TencentAccessToken AccessToken { get; set; }

        /// <summary>
        /// 获取或设置在用户授权成功后，是否需要 SDK 帮助你储存用户的 AccessToken.
        /// </summary>
        /// <remarks>
        /// true, SDK 将管理 AccessToken, 请不要手动设置 AccessToken.
        /// false，请手动设置 AccessToken， 或者在实例化 Service 时，调用带 AccessToken 参数的构造函数.
        /// </remarks>
        public static bool IfSaveAccessToken { get; set; }

        /// <summary>
        /// 清除缓存中和正在使用的 Access Token.
        /// </summary>
        public static void ClearAccessToken()
        {
            TokenIso.Current.Clear();
            AccessToken = null;
        }

        /// <summary>
        /// 验证 OAuth 相关的配置信息.
        /// </summary>
        /// <remarks>
        /// 1. Authority, AppKey, AppSerect 不允许为空，否则会抛出异常.
        /// 2. 若 IfSaveAccesstoken == true, SDK 将尝试从用户储存空间中读取 AccessToken，
        /// 若未找到 AccessToken, 则会抛出 MissingAppKeyOrAppSecretException 异常.
        /// 3. 若 IfSaveAccessToken ==  false,  则不会验证 AccessToken.
        /// </remarks>
        public static void Validate()
        {
            if (string.IsNullOrEmpty(APP_KEY) || string.IsNullOrEmpty(APP_SECRET))
            {
                throw new MissingAppKeyOrAppSecretException();
            }

            if (string.IsNullOrEmpty(Authority))
            {
                throw new OAuthException("Authority 不能为空!");
            }

            if (IfSaveAccessToken)
            {
                TencentAccessToken token = TokenIso.Current.LoadData();

                if (null == token)
                {
                    throw new MissingAccessTokenExcception();
                }


                if (token == null || string.IsNullOrEmpty(token.AccessToken))
                {
                    throw new MissingAccessTokenExcception();
                }

                AccessToken = token;
            }
        }
    }
}
