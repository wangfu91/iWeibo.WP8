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
using System.Collections.Generic;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Deserializer;
using System.Collections;
using TencentWeiboSDK.Hammock;
using Shared;

namespace TencentWeiboSDK.Services
{
    /// <summary>
    /// StatusesService 包含了时间线相关 API.
    /// </summary>
    public sealed class StatusesService : BaseService
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        public StatusesService()
            : this(null)
        { }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="accessToken">
        /// 指示该 Service 所需要的 AccessToken，优先级高于 OAuthConfigruation.AccessToken, 若该值为 null,
        /// SDK 将默认使用 OAuthConfigruation.AccessToken.
        /// </param>
        public StatusesService(TencentAccessToken accessToken)
            : base(accessToken)
        { }

        /// <summary>
        /// 我发表时间线.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="callback">回调返回我发表的时间线数据</param>
        public void BroadcastTimeline(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("statuses/broadcast_timeline", argment, (request, response, userState) =>
            {
                InternalCallback(response, callback);
            });
        }
        
        /// <summary>
        /// 主页时间线.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="callback">回调返回主页时间线数据.</param>
        public void HomeTimeline(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("statuses/home_timeline", argment, (request, response, userState) =>
                {
                    InternalCallback(response, callback);
                });
        }

        /// <summary>
        /// 我发表时间线索引.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="callback">回调返回我发表时间线索引数据.</param>
        public void BroadcastTimelineIds(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("statuses/broadcast_timeline_ids", argment, (request, response, userState) =>
            {
                InternalCallback(response, callback);
            });
        }

        /// <summary>
        /// 主页时间线索引.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="callback">回调返回主页时间线索引数据.</param>
        public void HomeTimelineIds(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("statuses/home_timeline_ids", argment, (request, response, userState) =>
            {
                InternalCallback(response, callback);
            });
        }

        /// <summary>
        /// 用户提及时间线.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="callback">回调返回用户提及时间线数据.</param>
        public void MentionsTimeline(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("statuses/mentions_timeline", argment, (request, response, userState) =>
            {
                InternalCallback(response, callback);
            });
        }

        /// <summary>
        /// 获取某用户的最新微博
        /// </summary>
        /// <param name="argment"></param>
        /// <param name="callback"></param>
        public void UserTimeline(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("statuses/user_timeline", argment, (request, response, userState) =>
            {
                InternalCallback(response, callback);
            });
        }

        /// <summary>
        /// 获取广播大厅的最新微博
        /// </summary>
        /// <param name="argment"></param>
        /// <param name="callback"></param>
        public void PublicTimeline(ServiceArgument argment,Action<Callback<StatusCollection>> callback)
        {
            this.Get("statuses/public_timeline", argment, (request, response, userState) =>
                {
                    InternalCallback(response, callback);
                });
        }

        /// <summary>
        /// 用户提及时间线索引.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="callback">返回用户提及时间线索引数据.</param>
        public void MentionsTimelineIds(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("statuses/mentions_timeline_ids", argment, (request, response, userState) =>
            {
                InternalCallback(response, callback);
            });
        }

        /// <summary>
        /// 用户收藏时间线.
        /// </summary>
        /// <param name="argment">参数列表, 可以为 null.</param>
        /// <param name="callback">返回用户收藏时间线数据.</param>
        public void FavoritesTimeline(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("fav/list_t", argment, (request, response, userState) =>
            {
                InternalCallback(response, callback);
            });
        }

        /// <summary>
        /// 微博相关/获取单条微博的转发或点评列表
        /// </summary>
        /// <param name="argment"></param>
        /// <param name="callback"></param>
        public void SingleCommentsTimeline(ServiceArgument argment, Action<Callback<StatusCollection>> callback)
        {
            this.Get("t/re_list", argment, (request, response, userState) =>
            {
                InternalCallback(response, callback);
            });
        }

        private void InternalCallback(RestResponse response, Action<Callback<StatusCollection>> callback)
        {
            lock (this)
            {
                if (null != callback)
                {
                    if (response.InnerException == null)
                    {
                        StatusCollection list = null;
                        var serializer = DeserializerManager.Instance.BuildStatusDeserializer();
                        list = serializer.ReadList(response.Content) as StatusCollection;

                        callback(new Callback<StatusCollection>(list));
                    }
                    else
                    {
                        callback(new Callback<StatusCollection>(response.InnerException.Message));
                    }
                }
            }
        }
    }
}
