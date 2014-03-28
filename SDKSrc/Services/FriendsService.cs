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
using TencentWeiboSDK.Services.Util;
using TencentWeiboSDK.Deserializer;
using TencentWeiboSDK.Hammock;
using Newtonsoft.Json.Linq;

namespace TencentWeiboSDK.Services
{
    /// <summary>
    /// FriendsService 包含了关系链相关的 Open API.
    /// </summary>
    public sealed class FriendsService : BaseService
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        public FriendsService():this(null)
        { }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="accessToken">
        /// 指示该 Service 所需要的 AccessToken，优先级高于 OAuthConfigruation.AccessToken, 若该值为 null,
        /// SDK 将默认使用 OAuthConfigruation.AccessToken.
        /// </param>
        public FriendsService(TencentAccessToken accessToken)
            : base(accessToken)
        { }

        /// <summary>
        /// 收听某个用户.
        /// </summary>
        /// <param name="argment">参数列表, 其中Name或FOpenIds必填一个.</param>
        /// <param name="action">回调Result返回空.</param>
        public void Add(ServiceArgument argment, Action<Callback<bool>> action)
        {
            this.Post("friends/add", argment, (request, response, userState) => 
            {
                InternalCallback(response, action);
            });
        }

        /// <summary>
        /// 取消收听某个用户.
        /// </summary>
        /// <param name="argment">参数列表, 其中Name或FOpenIds必填一个.</param>
        /// <param name="action">回调Result返回空.</param>
        public void Del(ServiceArgument argment, Action<Callback<bool>> action)
        {
            this.Post("friends/del", argment, (request, response, userState) =>
            {
                InternalCallback(response, action);
            });
        }

        /// <summary>
        /// 我的听众列表.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="action">回调返回我的听众的集合.</param>
        public void Fanlist(ServiceArgument argment, Action<Callback<UserColloection>> action)
        {
            this.Get("friends/fanslist", argment, (request, response, userState) =>
            {
                InternalCallback(response, action);
            });
        }

        /// <summary>
        /// 我收听的人列表.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="action">回调返回我收听的人集合.</param>
        public void Idolist(ServiceArgument argment, Action<Callback<UserColloection>> action)
        {
            this.Get("friends/idollist", argment, (request, response, userState) =>
            {
                InternalCallback(response, action);
            });
        }

        /// <summary>
        /// 其他帐户听众列表.
        /// </summary>
        /// <param name="argment">参数列表，Name和FOpenId至少选一个，若同时存在则以name值为主.</param>
        /// <param name="action"></param>
        public void UserFanslist(ServiceArgument argment, Action<Callback<UserColloection>> action)
        {
            this.Get("friends/user_fanslist", argment, (request, response, userState) =>
            {
                InternalCallback(response, action);
            });
        }

        /// <summary>
        /// 其他帐户收听的人列表
        /// </summary>
        /// <param name="argment">参数列表，Name和FOpenId至少选一个，若同时存在则以name值为主</param>
        /// <param name="action"></param>
        public void UserIdollist(ServiceArgument argment, Action<Callback<UserColloection>> action)
        {
            this.Get("friends/user_idollist", argment, (request, response, userState) =>
            {
                InternalCallback(response, action);
            });
        }


        private void InternalCallback(RestResponse response, Action<Callback<bool>> action)
        {
            lock (this)
            {
                if (null != action)
                {
                    var jo = JObject.Parse(response.Content);
                    int ret = jo["ret"].ToObject<int>();
                    string msg = jo["msg"].ToObject<string>();
                    if (ret == 0)
                    {
                        action(new Callback<bool>(true));
                    }
                    else
                    {
                        action(new Callback<bool>(msg));
                    }
                }
            }
        }

        private void InternalCallback(RestResponse response, Action<Callback<UserColloection>> action)
        {
            lock (this)
            {
                if (null != action)
                {
                    if (response.InnerException == null)
                    {
                        UserColloection list = null;

                        var serializer = DeserializerManager.Instance.BuildUserDeserializer();
                        list = serializer.ReadList(response.Content) as UserColloection;

                        action(new Callback<UserColloection>(list));
                    }
                    else
                    {
                        action(new Callback<UserColloection>(response.InnerException.Message));
                    }
                }
            }
        }
    }
}
