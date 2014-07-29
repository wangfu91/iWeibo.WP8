using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiboSdk.Models;

namespace WeiboSdk.Services
{
    public class WStatusService : BaseService
    {

        public WStatusService(SinaAccessToken accessToken)
            : base(accessToken)
        {

        }

        /// <summary>
        /// 根据Id获取一条微博信息
        /// </summary>
        /// <param name="id">微博唯一ID</param>
        /// <param name="action">回调</param>
        public void GetStatus(string id, Action<Callback<WStatus>> action)
        {
            SdkCmdBase cmdBase = new CmdStatus
            {
                acessToken = this.Token,
                id = id
            };

            this.NetEngine.RequestCmd(SdkRequestType.STATUSES_SHOW, cmdBase, (requestType, response) =>
                {
                    if (action != null)
                    {
                        if (response.errCode == SdkErrCode.SUCCESS)
                        {
                            WStatus status = null;
                            status = JsonConvert.DeserializeObject<WStatus>(response.content);

                            action(new Callback<WStatus>(status));
                        }
                        else
                        {
                            action(new Callback<WStatus>(ErrCodeToMsg.GetMsg(response.errCode)));
                        }
                    }
                });
        }

        public void GetStatusContent(long id, Action<Callback<string>> action)
        {
            SdkCmdBase cmdBase = new CmdStatus
            {
                acessToken = this.Token,
                id = id.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.STATUSES_SHOW, cmdBase, (requestType, response) =>
            {
                if (action != null)
                {
                    if (response.errCode == SdkErrCode.SUCCESS)
                    {
                        action(new Callback<string>(data:response.content));
                    }
                    else
                    {
                        action(new Callback<string>(ErrCodeToMsg.GetMsg(response.errCode)));
                    }
                }
            });

        }



        public void GetStatusBatch(List<string> idList, Action<Callback<WStatusCollection>> action)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < idList.Count; i++)
            {
                var length = idList[i].Length;
                sb.Append(idList[i].Insert(length, ","));
            }

            var cmd = new CmdStatus
            {
                acessToken = this.Token,
                ids = sb.ToString()
            };

            this.NetEngine.RequestCmd(SdkRequestType.STATUSES_SHOW_BATCH, cmd, (requestType, response) =>
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

        /// <summary>
        /// 转发一条微博
        /// </summary>
        /// <param name="id">要转发的微博ID</param>
        /// <param name="status">添加的转发文本，内容不超过140个汉字，不填则默认为“转发微博”</param>
        /// <param name="isComment">是否在转发的同时发表评论，0：否、1：评论给当前微博、2：评论给原微博、3：都评论，默认为0</param>
        /// <param name="action">回调数据</param>
        public void Repost(string id, string status, int isComment, Action<Callback<bool>> action)
        {
            SdkCmdBase cmdArg = new CmdStatusRepost
            {
                acessToken = this.Token,
                id = id,
                status = status,
                is_comment = isComment.ToString()
            };
            this.NetEngine.RequestCmd(SdkRequestType.STATUSES_REPOST, cmdArg, (requestType, response) =>
                {
                    InternalCallback(action, response);
                });
        }

        /// <summary>
        /// 对一条微博进行评论
        /// </summary>
        /// <param name="id"> 需要评论的微博ID</param>
        /// <param name="comment">评论内容</param>
        /// <param name="commentOri">当评论转发微博时，是否评论给原微博，0：否、1：是，默认为0</param>
        /// <param name="action">回调数据</param>
        public void Comment(string id, string comment, int commentOri, Action<Callback<bool>> action)
        {
            SdkCmdBase cmdArg = new CmdStatusComment
            {
                acessToken = this.Token,
                id = id,
                comment = comment,
                comment_ori = commentOri.ToString()
            };
            this.NetEngine.RequestCmd(SdkRequestType.COMMENTS_CREAT, cmdArg, (requestType, response) =>
            {
                InternalCallback(action, response);
            });
        }

        /// <summary>
        /// 添加一条微博到收藏里
        /// </summary>
        /// <param name="id">要收藏的微博ID</param>
        /// <param name="action">回调数据</param>
        public void AddFavorite(string id, Action<Callback<bool>> action)
        {
            SdkCmdBase cmdArg = new CmdStatus
            {
                acessToken = this.Token,
                id = id
            };
            this.NetEngine.RequestCmd(SdkRequestType.FAVORITES_CREATE, cmdArg, (requestType, response) =>
                {
                    InternalCallback(action, response);
                });
        }

        /// <summary>
        /// 取消收藏一条微博
        /// </summary>
        /// <param name="id">要取消收藏的微博ID</param>
        /// <param name="action">回调委托</param>
        public void DelFavorite(string id, Action<Callback<bool>> action)
        {
            SdkCmdBase cmdArg = new CmdStatus
            {
                acessToken = this.Token,
                id = id
            };
            this.NetEngine.RequestCmd(SdkRequestType.FAVORITES_DESTROY, cmdArg, (requestType, response) =>
            {
                InternalCallback(action, response);
            });
        }

        /// <summary>
        /// 发布一条心微博
        /// </summary>
        /// <param name="status">要发布的微博文本内容，不超过140个汉字</param>
        /// <param name="action">回调委托</param>
        public void Add(string status, Action<Callback<bool>> action)
        {
            SdkCmdBase cmdArg = new CmdUploadMessage
            {
                acessToken = this.Token,
                status = status
            };
            this.NetEngine.RequestCmd(SdkRequestType.UPLOAD_MESSAGE, cmdArg, (requestType, response) =>
                {
                    InternalCallback(action, response);
                });
        }

        /// <summary>
        /// 上传图片并发表一条新微博
        /// </summary>
        /// <param name="status">要发布的微博文本内容，不超过140个汉字</param>
        /// <param name="pic">包含要上传的图片的封装类，仅支持JPEG、GIF、PNG格式，图片大小小于5M</param>
        /// <param name="action">回调委托</param>
        public void AddPic(string status, UploadPictureHelper pic, Action<Callback<bool>> action)
        {
            SdkCmdBase cmdArg = new CmdUploadMsgWithPic
            {
                acessToken = this.Token,
                pic = pic,
                status = status
            };
            this.NetEngine.RequestCmd(SdkRequestType.UPLOAD_MESSAGE_PIC, cmdArg, (requestType, response) =>
                {
                    InternalCallback(action, response);
                });
        }


        public void DestroyStatus(string id, Action<Callback<bool>> action)
        {
            SdkCmdBase cmdBase = new CmdStatus
            {
                acessToken = this.Token,
                id = id
            };

            this.NetEngine.RequestCmd(SdkRequestType.STATUSES_DESTROY, cmdBase, (requestType, response) =>
                {
                    InternalCallback(action, response);
                });
        }

        private void InternalCallback(Action<Callback<bool>> action, SdkResponse response)
        {
            if (action != null)
            {
                if (response.errCode == SdkErrCode.SUCCESS)
                {
                    action(new Callback<bool>(true));
                }
                else
                {
                    action(new Callback<bool>(ErrCodeToMsg.GetMsg(response.errCode)));
                }
            }
        }
    }
}
