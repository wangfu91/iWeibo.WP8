using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiboSdk.Models;

namespace WeiboSdk.Services
{
    public class TimelineService:BaseService
    {
        public TimelineService(SinaAccessToken accessToken)
            :base(accessToken)
        {

        }


        public void GetFriendsTimeline(
            int count,
            long maxId,
            long sinceId,
            Action<Callback<WStatusCollection>> action)
        {
            SdkCmdBase cmdArg = new CmdStatusTimeline
            {
                acessToken = this.Token,
                count = count.ToString(),
                max_id = maxId.ToString(),
                since_id = sinceId.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.FRIENDS_TIMELINE, cmdArg, (requestType, response) =>
                {
                    if (action != null)
                    {
                        if (response.errCode == SdkErrCode.SUCCESS)
                        {
                            WStatusCollection collection = null;
                            collection = JsonConvert.DeserializeObject<WStatusCollection>(response.content);

                            action(new Callback<WStatusCollection>(collection));
                        }
                        else
                        {
                            action(new Callback<WStatusCollection>(ErrCodeToMsg.GetMsg(response.errCode)));
                        }
                    }
                });
        }


        public void GetMentionsTimeline(
            int count,
            long maxId,
            long sinceId,
            Action<Callback<WStatusCollection>> action)
        {
            SdkCmdBase cmdArg = new CmdStatusTimeline
            {
                acessToken = this.Token,
                count = count.ToString(),
                max_id = maxId.ToString(),
                since_id = sinceId.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.MENTIONS_TIMELINE, cmdArg, (requestType, response) =>
                {
                    if (action != null)
                    {
                        if (response.errCode == SdkErrCode.SUCCESS)
                        {
                            WStatusCollection collection;
                            collection = JsonConvert.DeserializeObject<WStatusCollection>(response.content);

                            action(new Callback<WStatusCollection>(collection));
                        }
                        else
                        {
                            action(new Callback<WStatusCollection>(ErrCodeToMsg.GetMsg(response.errCode)));
                        }
                    }
                });
        }

        public void GetCommentsTimleine(
            string id,
            int count,
            long maxId,
            long sinceId,
            Action<Callback<WCommentCollection>> action)
        {
            SdkCmdBase cmdArg = new CmdCommentsTimeline
            {
                acessToken = this.Token,
                id=id.ToString(),
                count = count.ToString(),
                max_id = maxId.ToString(),
                since_id = sinceId.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.COMMENTS_TIMELINE, cmdArg, (requestType, response) =>
            {
                if (action != null)
                {
                    if (response.errCode == SdkErrCode.SUCCESS)
                    {
                        WCommentCollection collection;
                        collection = JsonConvert.DeserializeObject<WCommentCollection>(response.content);

                        action(new Callback<WCommentCollection>(collection));
                    }
                    else
                    {
                        action(new Callback<WCommentCollection>(ErrCodeToMsg.GetMsg(response.errCode)));
                    }
                }
            });
        }

        public void GetFavoritesTimeline(
            int count,
            long maxId,
            long sinceId,
            Action<Callback<WFavoriteCollection>> action)
        {
            SdkCmdBase cmdArg = new CmdStatusTimeline
            {
                acessToken = this.Token,
                count = count.ToString(),
                max_id = maxId.ToString(),
                since_id = sinceId.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.FAVORITE_TIMLINE, cmdArg, (requestType, response) =>
                {
                    if (action != null)
                    {
                        if (response.errCode == SdkErrCode.SUCCESS)
                        {

                            var collection = new WFavoriteCollection();
                            var jo = JObject.Parse(response.content);
                            var ja = jo["favorites"];
                            if (jo["next_cursor"] != null)
                                collection.NextCursor = (long)jo["next_cursor"];
                            collection.TotalNumber = (int)jo["total_number"];
                            collection.Favorites = new List<WStatus>();
                            foreach (var j in ja.Children())
                            {
                                var status = j["status"].ToObject<WStatus>();
                                collection.Favorites.Add(status);
                            }

                            action(new Callback<WFavoriteCollection>(collection));
                        }
                        else
                        {
                            action(new Callback<WFavoriteCollection>(ErrCodeToMsg.GetMsg(response.errCode)));
                        }

                    }
                });
        }
    }
}
