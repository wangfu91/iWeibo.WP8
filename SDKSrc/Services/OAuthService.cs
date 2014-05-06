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
        string authority = "https://open.t.qq.com/";
        
        private void GetAccessTokenRequest(string callbackUrl, AuthorizationCode authCode, RestCallback handler)
        {
            RestClient client = new RestClient()
            {
                Authority = authority,
                HasElevatedPermissions = true,
                //Credentials = OAuthCredentials.ForTencentAccessToken(OAuthConfigruation.APP_KEY, OAuthConfigruation.APP_SECRET, requestToken, requestTokenSecret, verifier)
            };

            RestRequest request = new RestRequest()
            {
                Path = @"cgi-bin/oauth2/access_token",
                Method = WebMethod.Get
            };
            request.AddParameter("client_id", OAuthConfigruation.APP_KEY);
            request.AddParameter("client_secret", OAuthConfigruation.APP_SECRET);
            request.AddParameter("redirect_uri", callbackUrl);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", authCode.Code);
            request.AddParameter("state", OAuthTools.GetNonce());

            client.BeginRequest(request, new RestCallback(handler));
        }

        /// <summary>
        /// 获取AccessToken.
        /// </summary>
        /// <param name="authCode">填入在 GetRequestTokenRequest 里取得的Authorization Code</param>
        /// <param name="callback">回调返回AccessToken.</param>
        public void GetAccessTokenRequest(string callbackUrl,AuthorizationCode authCode, Action<Callback<TencentAccessToken>> callback)
        {
            this.GetAccessTokenRequest(callbackUrl, authCode, (request, response, userState) =>
            {
                if (callback != null)
                {
                    callback(new Callback<TencentAccessToken>(new TencentAccessToken(response.Content)));
                }
            });
        } 
    }
}
