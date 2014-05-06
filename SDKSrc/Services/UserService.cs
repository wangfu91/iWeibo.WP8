using System;
using System.Collections.Generic;
using TencentWeiboSDK.Deserializer;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services.Util;

namespace TencentWeiboSDK.Services
{
    /// <summary>
    /// UserService 包含了帐户相关 API.
    /// </summary>
    public class UserService : BaseService
    {

        /// <summary>
        /// 构造函数.
        /// </summary>
        public UserService()
            : this(null)
        { }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="accessToken">
        /// 指示该 Service 所需要的 AccessToken，优先级高于 OAuthConfigruation.AccessToken, 若该值为 null,
        /// SDK 将默认使用 OAuthConfigruation.AccessToken.
        /// </param>
        public UserService(TencentAccessToken accessToken)
            : base(accessToken)
        { }


        /// <summary>
        /// 获取已授权用户的详细资料.
        /// </summary>
        /// <param name="callback">回调返回 User 对象.</param>
        public void UserInfo(Action<Callback<User>> callback)
        {
            this.Get("user/info", new ServiceArgument(), (request, response, userState) =>
            {
                if (null != callback)
                {
                    User user = null;
                    var serializer = DeserializerManager.Instance.BuildUserDeserializer();
                    user = serializer.Read(response.Content) as User;

                    callback(new Callback<User>(user));
                }
                else
                {
                    callback(new Callback<User>(response.InnerException.Message));
                }
            });
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="name">用户的openId</param>
        /// <param name="callback">回调返回User对象</param>
        public void OtherUserInfo(string name,Action<Callback<User>> callback)
        {
            this.Get("user/other_info", new ServiceArgument { Name = name }, (request, response, userState) =>
            {
                if (null != callback)
                {
                    if (response.InnerException == null)
                    {
                        User user = null;
                        var serializer = DeserializerManager.Instance.BuildUserDeserializer();
                        user = serializer.Read(response.Content) as User;

                        callback(new Callback<User>(user));
                    }
                    else
                    {
                        callback(new Callback<User>(response.InnerException.Message));
                    }
                }
            });
        }
		
        /// <summary>
        /// 搜索相关/搜索用户
        /// </summary>
        /// <param name="callback"></param>
        public void SearchUserInfo(ServiceArgument argment, Action<Callback<List<User>>> callback)
        {
            this.Get("search/user", argment, (request, response, userState) =>
            {
                if (null != callback)
                {
                    if (response.InnerException == null)
                    {
                        List<User> user = null;
                        var serializer = DeserializerManager.Instance.BuildUserDeserializer();
                        user = serializer.ReadList(response.Content) as List<User>;

                        callback(new Callback<List<User>>(user));
                    }
                    else
                    {
                        callback(new Callback<List<User>>(response.InnerException.Message));
                    }
                }
            });
        }

    }
}
