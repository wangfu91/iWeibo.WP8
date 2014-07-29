using Hammock;
using Hammock.Authentication;
using Hammock.Silverlight.Compat;
using Hammock.Web;
using SinaBase;
using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using Shared;

namespace WeiboSdk
{
    /// <summary>
    /// SDK中的数据请求类
    /// Author:linan4
    /// </summary>
    public class SdkNetEngine
    {
        #region 成员变量
        private RestClient m_Client = new RestClient();
        public delegate void SdkCallBack(SdkRequestType type, SdkResponse response);
        public delegate void RequestBack(SdkResponse response);
        private SdkCallBack m_RequestCallBack = null;
        #endregion

        //public void StopRequest()
        //{
        //    if (null != m_Client)
        //        m_Client.StopRequest();
        //}

        /// <summary>
        /// 普通数据请求
        /// </summary>
        /// <param name="type"请求数据类型></param>
        /// <param name="data">参数包(SdkCmdBase的子类)</param>
        /// <param name="callBack">请求的回调函数</param>
        /// <param name="dataType">要求返回数据类型</param>
        public void RequestCmd (SdkRequestType type, SdkCmdBase data, SdkCallBack callBack)
        {
            m_RequestCallBack = callBack;

            RestRequest request = new RestRequest();

            bool retValue = ConfigParams(request, type, data);

            if (false == retValue)
                return;
            SendRequest(request,data, (e1) =>
            {
                if (null != m_RequestCallBack)
                    m_RequestCallBack(type, e1);

            });

        }

        /// <summary>
        /// 发送普通网络请求，传入组好参数包的RestRequest
        /// </summary>
        /// <param name="request">要发送的组好参的request</param>
        /// <param name="data">如需鉴权传入OAuth1.0的accessToken、accessTokenSecret或者OAuth2.0的accessToken</param>
        /// <param name="callBack">回调</param>
        /// <param name="dataType">需要xml/json</param>
        public void SendRequest(RestRequest request, SdkCmdBase data,RequestBack callBack)
        {
            Action<string> errAction = (e1) =>
            {
                if (null != callBack)
                {
                    SdkErrCode sdkErr = SdkErrCode.XPARAM_ERR;
                    callBack(new SdkResponse
                    {
                        //requestID = null != data ? data.requestId : "",
                        errCode = sdkErr,
                        specificCode = "",
                        content = e1,
                        stream = null
                    });
                }
            };

            if (null == request)
            {
                errAction("request should`t be null.");
                return;
            }

            //string strOauth = "";
            //if (SdkData.AuthOption == EumAuth.OAUTH2_0)
            //    strOauth = "https" + ConstDefine.publicApiUrl;
            //else
            //    strOauth = "http" + ConstDefine.publicApiUrl;
            m_Client.Authority = ConstDefine.publicApiUrl;
            m_Client.HasElevatedPermissions = true;

            //添加鉴权
            request.DecompressionMethods = DecompressionMethods.GZip;
            request.Encoding = Encoding.UTF8;
            //设置 User-Agent
            request.UserAgent = SdkData.UserAgent;

            IWebCredentials credentials = null;

            if (null != data && !string.IsNullOrEmpty(data.acessToken))
            {
                //OAuth 认证
                //if (SdkData.AuthOption == EumAuth.OAUTH2_0)
                //{
                    request.AddHeader("Authorization", string.Format("OAuth2 {0}", data.acessToken));
                //}
                //else
                //{
                //    credentials = new OAuthCredentials
                //    {
                //        Type = OAuthType.ProtectedResource,
                //        SignatureMethod = OAuthSignatureMethod.HmacSha1,
                //        ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                //        ConsumerKey = SdkData.AppKey,
                //        ConsumerSecret = SdkData.AppSecret,
                //        Token = data.acessToken,
                //        TokenSecret = data.acessTokenSecret,
                //        Version = "1.0",
                //    };
                //}
            }
            else
            {
                request.AddParameter("source", SdkData.AppKey);
            }

            request.Credentials = credentials;

            m_Client.BeginRequest(request, (e1, e2, e3) => AsyncCallback(e1, e2, callBack));

        }


        private bool ConfigParams(RestRequest request, SdkRequestType type, SdkCmdBase data)
        {
            Action<string> errAction = (e1) =>
            {
                if (null != m_RequestCallBack)
                {
                    SdkErrCode sdkErr = SdkErrCode.XPARAM_ERR;
                    m_RequestCallBack(type, new SdkResponse
                    {
                        //requestID = null != data ? data.requestId : "",
                        errCode = sdkErr,
                        specificCode = "",
                        content = e1,
                        stream = null
                    });
                }
            };

            if (null == request)
            {
                errAction("发生内部错误");
                return false;
            }

            //向RestResuest添加参数
            var t = data.GetType();
            if ((data is CmdUploadMsgWithPic) != true)//上传带图片微博的情况特殊，不再向 request 添加参数，而是在case中单独处理
            {
                foreach (FieldInfo fi in t.GetFields())
                {
                    string value = fi.GetValue(data).ToString();
                    if (value.Length > 0)
                    {
                        request.AddParameter(fi.Name, value);
                    }
                }
            }

            switch (type)
            {
                #region UPLOAD_MESSAGE
                case SdkRequestType.UPLOAD_MESSAGE:
                    {
                        request.Method = WebMethod.Post;
                        request.Path = "/statuses/update.json";
                    }
                    break;
                #endregion

                #region UPLOAD_MESSAGE_PIC
                case SdkRequestType.UPLOAD_MESSAGE_PIC:
                    {
                        request.Method = WebMethod.Post;
                        request.Path = "/statuses/upload.json";

                        var message = data as CmdUploadMsgWithPic;
                        request.AddField("status", message.status);

                        if (message.lat.Length > 0)
                            request.AddField("lat", message.lat);
                        if (message.@long.Length > 0)
                            request.AddField("long", message.@long);

                        UploadPictureHelper pic = message.pic;
                        if (string.IsNullOrWhiteSpace(pic.FullPathName))
                        {
                            errAction("picPath is null.");
                            return false;
                        }

                        if (".png" == pic.Extension)
                        {
                            request.AddFile("pic", pic.FileName, pic.FullPathName, "image/png");
                        }
                        else
                        {
                            request.AddFile("pic", pic.FileName, pic.FullPathName, "image/jpeg");
                        }

                    }
                    break;
                #endregion

                #region FRIENDS_TIMELINE
                case SdkRequestType.FRIENDS_TIMELINE:
                    {
                        request.Path = "/statuses/friends_timeline.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region FRIENDS_TIMELINE_ID
                case SdkRequestType.FRIENDS_TIMELINE_ID:
                    {
                        request.Path = "/statuses/friends_timeline/ids.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region STATUSES_SHOW
                case SdkRequestType.STATUSES_SHOW:
                    {
                        request.Path = "/statuses/show.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region STATUSES_SHOW_BATCH
                case SdkRequestType.STATUSES_SHOW_BATCH:
                    {
                        request.Path = "/statuses/show_batch.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region STATUSES_DESTROY
                case SdkRequestType.STATUSES_DESTROY:
                    {
                        request.Path = "statuses/destroy.json";
                        request.Method = WebMethod.Post;
                    }
                    break;
                #endregion

                #region STATUSES_REPOST
                case SdkRequestType.STATUSES_REPOST:
                    {
                        request.Path = "statuses/repost.json";
                        request.Method = WebMethod.Post;
                    }
                    break;
                #endregion

                #region COMMENTS_TIMELINE
                case SdkRequestType.COMMENTS_TIMELINE:
                    {
                        request.Path = "/comments/show.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region COMMENTS_CREATE
                case SdkRequestType.COMMENTS_CREAT:
                    {
                        request.Path = "/comments/create.json";
                        request.Method = WebMethod.Post;
                    }
                    break;
                #endregion

                #region COMMENTS_REPLY
                case SdkRequestType.COMMENTS_REPLY:
                    {
                        request.Path = "/comments/reply.json";
                        request.Method = WebMethod.Post;
                    }
                    break;
                #endregion

                #region MENTIONS_TIMELINE
                case SdkRequestType.MENTIONS_TIMELINE:
                    {
                        request.Path = "/statuses/mentions.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region USER_TIMELINE
                case SdkRequestType.USER_TIMELINE:
                    {
                        request.Path = "/statuses/user_timeline.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                    #endregion

                #region FAVORITE_TIMELINE
                case SdkRequestType.FAVORITE_TIMLINE:
                    {
                        request.Path = "/favorites.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region FAVORITES_CREATE
                case SdkRequestType.FAVORITES_CREATE:
                    {
                        request.Path = "/favorites/create.json";
                        request.Method = WebMethod.Post;
                    }
                    break;
                #endregion

                #region FAVORITES_DEATROY
                case SdkRequestType.FAVORITES_DESTROY:
                    {
                        request.Path = "/favorites/destroy.json";
                        request.Method = WebMethod.Post;
                    }
                    break;
                #endregion

                #region FRIENDSHIP_CREATE
                case SdkRequestType.FRIENDSHIP_CREATE:
                    {
                        request.Path = "/friendships/create.json";
                        request.Method = WebMethod.Post;
                    }
                    break;
                #endregion

                #region FRIENDSHIP_DESDROY
                case SdkRequestType.FRIENDSHIP_DESDROY:
                    {
                        request.Path = "/friendships/destroy.json";
                        request.Method = WebMethod.Post;
                    }
                    break;
                #endregion

                #region GET_UID
                case SdkRequestType.GET_UID:
                    {
                        request.Path = "/account/get_uid.json";
                        request.Method = WebMethod.Get;
                    }
                    break;

                #endregion

                #region USERS_SHOW
                case SdkRequestType.USERS_SHOW:
                    {
                        request.Path = "/users/show.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region FRIENDSHIP_SHOW（获取两个用户关系的详细信息）
                case SdkRequestType.FRIENDSHIP_SHOW:
                    {
                        request.Path = "/friendships/show.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region FRIENDSHIP_FRIENDS(获取用户的关注列表)
                case SdkRequestType.FRIENDSHIP_FRIENDS:
                    {
                        request.Path = "/friendships/friends.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                #region FRIENDSHIP_FOLLOWERS(获取用户的粉丝列表)
                case SdkRequestType.FRIENDSHIP_FOLLOWERS:
                    {
                        request.Path = "/friendships/followers.json";
                        request.Method = WebMethod.Get;
                    }
                    break;
                #endregion

                default:
                    {
                        errAction("此请求尚不支持.");
                        return false;
                    }
            }
            request.AddParameter("curtime", DateTime.Now.ToString());
            return true;
        }

        private void AsyncCallback(RestRequest request, RestResponse response, RequestBack callBack)
        {
            SdkResponse sdkRes = new SdkResponse();
            try
            {
                if (true == response.TimedOut)
                {
                    sdkRes.errCode = SdkErrCode.TIMEOUT;
                    sdkRes.content = "连接超时";

                }
                //未知异常(自定义)
                else if (null != response.UnKnowException)
                {
                    sdkRes.errCode = SdkErrCode.NET_UNUSUAL;
                    sdkRes.content = "网络异常";

                    if (response.UnKnowException is WebException)
                    {
                        WebException ex = response.UnKnowException as WebException;
                        if (WebExceptionStatus.RequestCanceled == ex.Status)
                        {

                            sdkRes.errCode = SdkErrCode.USER_CANCEL;
                            sdkRes.content = "Web Request is cancled.";

                        }
                    }

                }
                //网络异常(WebException)
                else if (null != response.InnerException || HttpStatusCode.OK != response.StatusCode)
                {
                    bool isUserCanceled = false;
                    if (response.InnerException is WebException)
                    {
                        WebException ex = response.InnerException as WebException;
                        if (WebExceptionStatus.RequestCanceled == ex.Status)
                        {
                            sdkRes.errCode = SdkErrCode.USER_CANCEL;
                            sdkRes.content = "Web Request is cancled.";
                            isUserCanceled = true;

                        }
                    }

                    if (!isUserCanceled)
                    {
                        try
                        {
                            ErrorRes resObject = null;
                            //if (state.dataType == DataType.XML)
                            if (request.Path.Contains(".xml") || request.Path.Contains(".XML"))
                            {
                                XElement xmlSina = XElement.Parse(response.Content);
                                if (null != xmlSina.Element("error_code"))
                                {
                                    //得到服务器标准错误信息
                                    XmlSerializer serializer = new XmlSerializer(typeof(ErrorRes));
                                    resObject = serializer.Deserialize(response.ContentStream) as ErrorRes;
                                }
                            }
                            else
                            {
                                DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(ErrorRes));
                                resObject = ser.ReadObject(response.ContentStream) as ErrorRes;

                            }

                            if (null != resObject && resObject is ErrorRes)
                            {
                                sdkRes.errCode = SdkErrCode.SERVER_ERR;
                                sdkRes.specificCode = resObject.ErrCode;
                                sdkRes.content = resObject.ErrInfo;
                            }
                            else
                                throw new Exception();
                        }
                        catch//如果没有error_code这个节点...
                        {
                            //不是xml
                            //网络异常时统一错误：NET_UNUSUAL
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                sdkRes.errCode = SdkErrCode.NET_UNUSUAL;
                                sdkRes.content = "网络状况异常";
                            }
                            else
                            {
                                sdkRes.errCode = SdkErrCode.SERVER_ERR;
                                sdkRes.specificCode = response.StatusCode.ToString();
                                sdkRes.content = response.Content;
                            }

                        }
                    }

                }
                else
                {
                    sdkRes.content = response.Content;
                    sdkRes.stream = response.ContentStream;
                }
            }
            catch (Exception e)
            {
                Logger.Warn(e.Message);
                //日志
                sdkRes.errCode = SdkErrCode.SERVER_ERR;
                sdkRes.content = "服务器返回信息异常";

            }

            if (null != callBack)
                callBack(sdkRes);
        }

    }
}
