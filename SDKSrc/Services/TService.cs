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
using TencentWeiboSDK.Services.Util;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Hammock;
using TencentWeiboSDK.Deserializer;
using Newtonsoft.Json.Linq;

namespace TencentWeiboSDK.Services
{
    /// <summary>
    /// TService 包含了微博相关 API.
    /// </summary>
    public sealed class TService : BaseService
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        public TService()
            : this(null)
        { }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="accessToken">
        /// 指示该 Service 所需要的 AccessToken，优先级高于 OAuthConfigruation.AccessToken, 若该值为 null,
        /// SDK 将默认使用 OAuthConfigruation.AccessToken.
        /// </param>
        public TService(TencentAccessToken accessToken)
            : base(accessToken)
        { }

        /// <summary>
        /// 获取一条微博
        /// </summary>
        /// <param name="argment">参数列表，Id为必填参数</param>
        /// <param name="callback"></param>
        public void Show(ServiceArgument argment, Action<Callback<Status>> callback)
        {
            this.Get("t/show", argment, (request, response, userState) =>
            {
                lock (this)
                {
                    Status status = null;
                    var serializer = DeserializerManager.Instance.BuildStatusDeserializer();
                    status = serializer.Read(response.Content) as Status;
                    if (null != callback)
                    {
                        callback(new Callback<Status>(status));
                    }
                }
            });
        }

        /// <summary>
        /// 删除一条微博
        /// </summary>
        /// <param name="argment">参数列表，id为必填参数</param>
        /// <param name="callback">无返回数据</param>
        public void Delete(ServiceArgument argment, Action<Callback<bool>> callback)
        {
            this.Post("t/del", argment, (request, response, userState) =>
                {
                    if (null != callback)
                    {
                        InternalCallback(response, callback);
                    }
                });
        }

        /// <summary>
        /// 转发一条微博.
        /// </summary>
        /// <param name="argment">参数列表, Content为必填参数</param>
        /// <param name="callback">无返回数据</param>
        public void Repost(ServiceArgument argment, Action<Callback<bool>> callback)
        {
            this.Post("t/re_add", argment, (request, response, userState) =>
            {
                if (null != callback)
                {
                    InternalCallback(response, callback);
                }
            });
        }

        /// <summary>
        /// 评论一条微博.
        /// </summary>
        /// <param name="argment">参数列表, Content为必填参数</param>
        /// <param name="callback">无返回数据</param>
        public void Comment(ServiceArgument argment, Action<Callback<bool>> callback)
        {
            this.Post("t/comment", argment, (request, response, userState) =>
            {

                if (null != callback)
                {
                    InternalCallback(response, callback);
                }
            });
        }

        /// <summary>
        /// 收藏一条微博
        /// </summary>
        /// <param name="argment">参数列表，Id为必填参数</param>
        /// <param name="callback">无返回数据</param>
        public void AddFavorite(ServiceArgument argment, Action<Callback<bool>> callback)
        {
            this.Post("fav/addt", argment, (request, response, userState) =>
                {
                    if (null != callback)
                    {
                        InternalCallback(response, callback);
                    }
                });
        }

        /// <summary>
        /// 取消收藏一条微博
        /// </summary>
        /// <param name="argment">参数列表，Id为必填参数</param>
        /// <param name="callback">无返回数据</param>
        public void DelFavorite(ServiceArgument argment, Action<Callback<bool>> callback)
        {
            this.Post("fav/delt", argment, (request, response, userState) =>
                {
                    if (null != callback)
                    {
                        InternalCallback(response, callback);
                    }
                });
        }
		
        /// <summary>
        /// 发表一条微博.
        /// </summary>
        /// <param name="argment">参数列表, Content为必填参数</param>
        /// <param name="callback">无返回数据</param>
        public void Add(ServiceArgument argment, Action<Callback<bool>> callback)
        {
            this.Post("t/add", argment, (request, response, userState) =>
            {
                if (null != callback)
                {
                    InternalCallback(response, callback);
                }
            });
        }

        /// <summary>
        /// 发表一条带图片的微博.
        /// </summary>
        /// <param name="argment">参数列表，Content和UploadPic为必填参数.</param>
        /// <param name="callback">无返回数据.</param>
        public void AddPic(ServiceArgument argment, Action<Callback<bool>> callback)
        {
            this.Post("t/add_pic", argment, (request, response, userState) =>
            {
                if (null != callback)
                {
                    InternalCallback(response, callback);
                }
            });
        }

        //private void InternalCallback(RestResponse response, Action<Callback<Result>> callback)
        //{
        //    lock (this)
        //    {
        //        if (null != callback)
        //        {
        //            if (response.InnerException != null)
        //            {
        //                Result list = null;
        //                var serializer = DeserializerManager.Instance.BuildResultDeserializer();
        //                list = serializer.Read(response.Content) as Result;

        //                callback(new Callback<Result>(list, response.Content));
        //            }
        //            else
        //            {
        //                callback(new Callback<Result>(response.InnerException));
        //            }
        //        }
        //    }
        //}

        private void InternalCallback(RestResponse response, Action<Callback<bool>> action)
        {
            lock (this)
            {
                if (null != action)
                {
                    if (string.IsNullOrEmpty(response.Content))
                        action(new Callback<bool>(""));
                    else
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
        }
    }
}
