using System;
using TencentWeiboSDK.Hammock.Web;

#if SILVERLIGHT
using TencentWeiboSDK.Hammock.SILVERLIGHT.Compat;
#endif

namespace TencentWeiboSDK.Hammock.Authentication.OAuth
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class OAuthCredentials : IWebCredentials
    {
        public virtual string ConsumerKey { get; set; }
        public virtual string ConsumerSecret { get; set; }
        public virtual OAuthParameterHandling ParameterHandling { get; set; }
        public virtual OAuthSignatureMethod SignatureMethod { get; set; }
        public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }
        public virtual OAuthType Type { get; set; }

        public virtual string Token { get; set; }
        public virtual string TokenSecret { get; set; }
        public virtual string Verifier { get; set; }
        public virtual string ClientUsername { get; set; }
        public virtual string ClientPassword { get; set; }
        public virtual string CallbackUrl { get; set; }
        public virtual string Version { get; set; }
        public virtual string SessionHandle { get; set; }

        public static RestRequest DelegateWith(RestClient client, RestRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException("request");
            }

            if(!request.Method.HasValue)
            {
                throw new ArgumentException("Request must specify a web method.");
            }

            var method = request.Method.Value;
            var credentials = (OAuthCredentials)request.Credentials;
            var url = request.BuildEndpoint(client).ToString();
            var workflow = new OAuthWorkflow(credentials);
            var uri = new Uri(client.Authority);
            var realm = uri.Host;

            var info = workflow.BuildProtectedResourceInfo(method, request.GetAllHeaders(), url);
            var query = credentials.GetQueryFor(url, request, info, method);
            ((OAuthWebQuery) query).Realm = realm;
            var auth = query.GetAuthorizationContent();

            var echo = new RestRequest();
            echo.AddHeader("X-Auth-Service-Provider", url);
            echo.AddHeader("X-Verify-Credentials-Authorization", auth);
            return echo;
        }

        public static OAuthCredentials ForRequestToken(string consumerKey, string consumerSecret)
        {
            var credentials = new OAuthCredentials
                                  {
                                      Type = OAuthType.RequestToken,
                                      ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                                      SignatureMethod = OAuthSignatureMethod.HmacSha1,
                                      SignatureTreatment = OAuthSignatureTreatment.Escaped,
                                      ConsumerKey = consumerKey,
                                      ConsumerSecret = consumerSecret, 
                                  };
            return credentials;
        }

        public static OAuthCredentials ForTencentRequestToken(string consumerKey, string consumerSecret, string callbackUrl)
        {
            var credentials = ForRequestToken(consumerKey, consumerSecret);
            credentials.ParameterHandling = OAuthParameterHandling.UrlOrPostParameters;
            credentials.CallbackUrl = callbackUrl;
            return credentials;
        }

        public static OAuthCredentials ForAccessToken(string consumerKey, string consumerSecret, string requestToken, string requestTokenSecret)
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.AccessToken,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Token = requestToken,
                TokenSecret = requestTokenSecret,
            };
            return credentials;
        }

        public static OAuthCredentials ForTencentAccessToken(string consumerKey, string consumerSecret, string requestToken, string requestTokenSecret, string verifier)
        {
            var credentials = ForAccessToken(consumerKey, consumerSecret, requestToken, requestTokenSecret);
            credentials.ParameterHandling = OAuthParameterHandling.UrlOrPostParameters;
            credentials.Verifier = verifier;
            return credentials;
        }

        public static OAuthCredentials ForAccessTokenRefresh(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, string sessionHandle)
        {
            var credentials = ForAccessToken(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            credentials.SessionHandle = sessionHandle;
            return credentials;
        }

        public static OAuthCredentials ForAccessTokenRefresh(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, string sessionHandle, string verifier)
        {
            var credentials = ForAccessToken(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            credentials.SessionHandle = sessionHandle;
            credentials.Verifier = verifier;
            return credentials;
        }

        public static OAuthCredentials ForClientAuthentication(string consumerKey, string consumerSecret, string username, string password)
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.ClientAuthentication,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                ClientUsername = username,
                ClientPassword = password
            };

            return credentials;
        }

		public static OAuthCredentials ForProtectedResource(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.ProtectedResource,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                SignatureTreatment = OAuthSignatureTreatment.Escaped,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Token = accessToken,
                TokenSecret = accessTokenSecret
            };
            return credentials;
        }

        public static OAuthCredentials ForTencentProtectedResource(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            var credentials = ForProtectedResource(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            credentials.ParameterHandling = OAuthParameterHandling.UrlOrPostParameters;
            return credentials;
        }
       
        public virtual WebQuery GetQueryFor(string url, 
                                            WebParameterCollection parameters, 
                                            IWebQueryInfo info, 
                                            WebMethod method)
        {
            OAuthWebQueryInfo oauth;

            var workflow = new OAuthWorkflow
            {
                ConsumerKey = ConsumerKey,
                ConsumerSecret = ConsumerSecret,
                ParameterHandling = ParameterHandling,
                SignatureMethod = SignatureMethod,
                SignatureTreatment = SignatureTreatment,
                CallbackUrl = CallbackUrl,
                ClientPassword = ClientPassword,
                ClientUsername = ClientUsername,
                Verifier = Verifier,
                Token = Token,
                TokenSecret = TokenSecret,
                Version = Version ?? "1.0",
                SessionHandle = SessionHandle
            };

            switch (Type)
            {
                case OAuthType.RequestToken:
                    workflow.RequestTokenUrl = url;
                    oauth = workflow.BuildRequestTokenInfo(method, parameters);
                    break;
                case OAuthType.AccessToken:
                    workflow.AccessTokenUrl = url;
                    oauth = workflow.BuildAccessTokenInfo(method, parameters);
                    break;
                case OAuthType.ClientAuthentication:
                    method = WebMethod.Post;
                    workflow.AccessTokenUrl = url;
                    oauth = workflow.BuildClientAuthAccessTokenInfo(method, parameters);
                    break;
                case OAuthType.ProtectedResource:
                    oauth = workflow.BuildProtectedResourceInfo(method, parameters, url);
                    oauth.FirstUse = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new OAuthWebQuery(oauth);
        }

        public virtual WebQuery GetQueryFor(string url, RestBase request, IWebQueryInfo info, WebMethod method)
        {
            WebParameterCollection ps = new WebParameterCollection();

            foreach (var p in request.Parameters)
            {
                ps.Add(p);
            }

            if (request.CopyFieldToParameters)
            {
                foreach (var p in request.PostParameters)
                {
                    if (p.Type == HttpPostParameterType.Field)
                    {
                        //if (HttpUtility.UrlDecode(p.Value) != HttpUtility.HtmlEncode(p.Value))
                        if (p.Value != string.Empty)
                            ps.Add(p);
                    }
                }
            }
            var query = GetQueryFor(url, ps, info, method);
            request.Method = method;
            return query;
			
            //var query = GetQueryFor(url, request.Parameters, info, method);
            //request.Method = method;
            //return query;
        }
    }
}


