using System;
using System.Net;
using TencentWeiboSDK.Hammock.Authentication.OAuth;
using TencentWeiboSDK.Hammock.Web;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services.Util;
using TencentWeiboSDK.Deserializer;
using TencentWeiboSDK.Hammock;
using System.Collections.Generic;

namespace TencentWeiboSDK.Services
{
    /// <summary>
    /// OAuthService 包含了 OAuth 相关 API.
    /// </summary>
    public sealed class OAuthService
    {
        string authority = "http://open.t.qq.com/";
        
        /// <summary>
        /// 获取RequestToken.
        /// </summary>
        /// <param name="callbackUrl">需要回调的Url(用于判断是否验证成功).</param>
        /// <param name="callback">回调返回 RequestToken 对象.</param>
        public void GetRequestTokenRequest(string callbackUrl,  Action<Callback<RequestToken>> callback)
        {
            this.GetRequestTokenRequest(callbackUrl, (request, response, userState) => {

                if (callback != null)
                {
                    callback(new Callback<RequestToken>(new RequestToken(response.Content)));
                }
            });
        }
        
        private void GetRequestTokenRequest(string callback, RestCallback handler)
        {
            RestClient client = new RestClient()
            {
                Authority = authority,
                HasElevatedPermissions = true,
                Credentials = OAuthCredentials.ForTencentRequestToken(OAuthConfigruation.APP_KEY, OAuthConfigruation.APP_SECRET, callback)
            };

            RestRequest request = new RestRequest()
            {
                Path = @"cgi-bin/request_token",
                Method = WebMethod.Get
            };

            client.BeginRequest(request, new RestCallback(handler));
        }

        private void GetAccessTokenRequest(string requestToken, string requestTokenSecret, string verifier, RestCallback handler)
        {
            RestClient client = new RestClient()
            {
                Authority = authority,
                HasElevatedPermissions = true,
                Credentials = OAuthCredentials.ForTencentAccessToken(OAuthConfigruation.APP_KEY, OAuthConfigruation.APP_SECRET, requestToken, requestTokenSecret, verifier)
            };

            RestRequest request = new RestRequest()
            {
                Path = @"cgi-bin/access_token",
                Method = WebMethod.Get
            };

            client.BeginRequest(request, new RestCallback(handler));
        }

        /// <summary>
        /// 获取AccessToken.
        /// </summary>
        /// <param name="requestToken">填入在 GetRequestTokenRequest 里取得的RequestToken.</param>
        /// <param name="callback">回调返回AccessToken.</param>
        public void GetAccessTokenRequest(RequestToken requestToken, Action<Callback<TencentAccessToken>> callback)
        {
            this.GetAccessTokenRequest(requestToken.TokenKey, requestToken.TokenSecret, requestToken.Verifier, (request, response, userState) =>
            {
                if (callback != null)
                {
                    callback(new Callback<TencentAccessToken>(new TencentAccessToken(response.Content)));
                }
            });
        } 
    }
}
