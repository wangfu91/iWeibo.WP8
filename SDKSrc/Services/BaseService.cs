using System.Collections.Generic;
using System.Linq;
using System.Text;
using TencentWeiboSDK.Hammock;
using TencentWeiboSDK.Hammock.Authentication.OAuth;
using TencentWeiboSDK.Hammock.Web;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services.Util;

namespace TencentWeiboSDK.Services
{
    /// <summary>
    /// 所有 Service(除了 OAuthService) 的基类，用于和底层网络接口 Hammock 进行通信.
    /// </summary>
    public abstract class BaseService
    {
        private TencentAccessToken accessToken;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessToken">
        /// 指示该 Service 所需要的 AccessToken，优先级高于 OAuthConfigruation.AccessToken, 若该值为 null,
        /// SDK 将默认使用 OAuthConfigruation.AccessToken.
        /// </param>
        public BaseService(TencentAccessToken accessToken)
        {
            this.accessToken = accessToken;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseService()
            : this(null)
        { }
        
        private Dictionary<string, object> parseArg(ServiceArgument arg)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(arg.Id))
            {
                args.Add("id", arg.Id);
            }

            if (!string.IsNullOrEmpty(arg.Keyword))
            {
                args.Add("keyword", arg.Keyword);
            }

            if (!string.IsNullOrEmpty(arg.Rootid))
            {
                args.Add("rootid", arg.Rootid);
            }

            if (arg.Page != 0)
            {
                args.Add("page", arg.Page);
            }

            if (arg.Flag !=0)
            {
                args.Add("flag", arg.Flag);
            }

            if (!string.IsNullOrEmpty(arg.Reid))
            {
                args.Add("reid", arg.Reid);
            }

            if (!string.IsNullOrEmpty(arg.Name))
            {
                args.Add("name", arg.Name);
            }

            if (!string.IsNullOrEmpty(arg.FOpenIds))
            {
                args.Add("fopenids", arg.FOpenIds);
            }

            if (arg.PageFlag != 0)
            {
                args.Add("pageflag", arg.PageFlag);
            }

            if (arg.PageTime != 0)
            {
                args.Add("pagetime", arg.PageTime);
            }

            if (arg.Pagesize != 0)
            {
                args.Add("pagesize", arg.Pagesize);
            }

            if (arg.Reqnum != 0)
            {
                args.Add("reqnum", arg.Reqnum);
            }

            if (arg.Type != 0)
            {
                args.Add("type", arg.Type);
            }

            if (arg.ContentType != 0)
            {
                args.Add("contenttype", arg.ContentType);
            }

            if (arg.StartIndex != 0)
            {
                args.Add("startindex", arg.StartIndex);
            }
			
            if (arg.Lastid != 0)
            {
                args.Add("lastid", arg.Lastid);
            }
			
            if (!string.IsNullOrEmpty(arg.Content))
            {
                args.Add("content", arg.Content);
            }

            if (!string.IsNullOrEmpty(arg.FOpenId))
            {
                args.Add("fopenid", arg.FOpenId);
            }

            if (null != arg.Pic)
            {
                args.Add("pic", arg.Pic);
            }

            switch (arg.Format)
            {
                case DataFormat.Json:
                    args.Add("format", "json");
                    break;
                case DataFormat.Xml:
                    args.Add("format", "xml");
                    break;
                default:
                    break;
            }

            return args;
        }

        /// <summary>
        /// 通过Get方法异步调用API.
        /// </summary>
        /// <param name="path">API的相对地址.</param>
        /// <param name="arg">参数列表.</param>
        /// <param name="handler">需要回调的委托.</param>
        protected void Get(string path, ServiceArgument arg , RestCallback handler)
        {
            if (null == arg)
            {
                arg = new ServiceArgument();
            }
            
            Get(path, parseArg(arg), handler);
        }
        
        private void Get(string path, Dictionary<string, object> args, RestCallback handler)
        {
            TencentAccessToken token = (this.accessToken == null) ? OAuthConfigruation.AccessToken : this.accessToken;

            if (null == token)
            {
                throw new MissingAccessTokenExcception();
            }


            RestClient client = new RestClient()
            {
                Authority = OAuthConfigruation.Authority,
                HasElevatedPermissions = true,
                Credentials = OAuthCredentials.ForTencentProtectedResource(OAuthConfigruation.APP_KEY, OAuthConfigruation.APP_SECRET, token.TokenKey, token.TokenSecret),
                Encoding = Encoding.UTF8
            };

            RestRequest request = new RestRequest()
            {
                Path = path,
                Method = WebMethod.Get,
                Encoding = Encoding.UTF8
            };

            foreach (var i in args)
            {
                request.AddParameter(i.Key, i.Value.ToString());
            }

            client.BeginRequest(request, new RestCallback(handler));
        }


        /// <summary>
        /// 用 Post 方法异步上传数据
        /// </summary>
        /// <param name="path">API的要对路径</param>
        /// <param name="arg">参数列表</param>
        /// <param name="handler">回调委托</param>
        protected void Post(string path, ServiceArgument arg, RestCallback handler)
        {
            Post(path, parseArg(arg), handler);
        }

        private void Post(string path, Dictionary<string, object> args, RestCallback handler)
        {
            TencentAccessToken token = (this.accessToken == null) ? OAuthConfigruation.AccessToken : this.accessToken;

            if (null == token)
            {
                throw new MissingAccessTokenExcception();
            }

            var client = new RestClient
            {
                Authority = OAuthConfigruation.Authority,
                HasElevatedPermissions = true,
                Encoding = Encoding.UTF8
            };

            var request = new RestRequest
            {
                Method = WebMethod.Post,
                Path = path,
                CopyFieldToParameters = true,
                Encoding = Encoding.UTF8,
                Credentials = new OAuthCredentials
                {
                    Type = OAuthType.ProtectedResource,
                    ParameterHandling = OAuthParameterHandling.UrlOrPostParameters,
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    SignatureTreatment = OAuthSignatureTreatment.Escaped,
                    ConsumerKey = OAuthConfigruation.APP_KEY,
                    ConsumerSecret = OAuthConfigruation.APP_SECRET,
                    Token = token.TokenKey,
                    TokenSecret = token.TokenSecret,
                }
            };

            bool needsPost = (args.Values.FirstOrDefault(a => (a is UploadPic)) == null) ? false : true;

            foreach (var i in args)
            {
                if (i.Value is UploadPic)
                {
                    UploadPic pic = i.Value as UploadPic;
                    request.AddFile(i.Key, pic.FileName, pic.FullPathName, ((pic.Extention == ".jpg") ? "image/jpeg" : "image/png"));
                }
                else
                {
                    if (needsPost)
                    {
                        request.AddField(i.Key, i.Value.ToString());
                    }
                    else
                        request.AddParameter(i.Key, i.Value.ToString());
                }
            }

            client.BeginRequest(request, handler);
        }
    }

   
}
