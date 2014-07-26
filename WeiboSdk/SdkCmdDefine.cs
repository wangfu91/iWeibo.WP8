using Shared;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WeiboSdk
{

    //自定义的ErrCode
    public enum SdkErrCode
    {
        //参数错误
        XPARAM_ERR = -1,
        //成功
        SUCCESS = 0,
        //网络不可用
        NET_UNUSUAL,
        //服务器返回异常
        SERVER_ERR,
        //访问超时
        TIMEOUT,
        //用户请求被取消
        USER_CANCEL

    }

    //public enum DataType
    //{
    //    XML = 0,
    //    JSON
    //}

    public class SdkResponse
    {
        public SdkErrCode errCode;
        public string specificCode;

        //public string requestID = "";
        public string content = "";
        public Stream stream = null;
    }

    public class SdkAuthError
    {
        public SdkErrCode errCode;
        public string specificCode = "";
        public string errMessage = "";
    }

    public class SdkAuthRes
    {
        public string userId = "";
        public string acessToken = "";
        public string acessTokenSecret = "";

        //refleshToken
    }

    [DataContract]
    public class SdkAuth2Res
    {
        [DataMember(Name = "access_token")]
        public string accesssToken { get; set; }

        [DataMember(Name = "refresh_token")]
        public string refleshToken { get; set; }

        [DataMember(Name = "expires_in")]
        public string expriesIn { get; set; }
    }

    [DataContract]
    public class OAuthErrRes
    {
        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "error_code")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "error_description")]
        public string errDes { get; set; }
    }

    public delegate void OAuth1LoginBack(bool isSucess, SdkAuthError err, SdkAuthRes response);
    public delegate void OAuth2LoginBack(bool isSucess, SdkAuthError err, SdkAuth2Res response);

    /// <summary>
    /// 失败时返回的对象(外部接口)
    /// </summary>
    [XmlRoot("hash")]
    [DataContract]
    public class ErrorRes
    {
        [XmlElement("request")]
        [DataMember(Name = "request")]
        public string Request { get; set; }

        [XmlElement("error_code")]
        [DataMember(Name = "error_code")]
        public string ErrCode { get; set; }

        [XmlElement("error")]
        [DataMember(Name = "error")]
        public string ErrInfo { get; set; }

        //public string InnErrcode
        //{
        //    get
        //    {
        //        string err = "";
        //        if (!string.IsNullOrEmpty(ErrInfo))
        //        {
        //            int pos = ErrInfo.IndexOf(":");
        //            if (-1 != pos)
        //            {
        //                err = ErrInfo.Substring(0, pos);
        //            }
        //        }
        //        return err;
        //    }
        //}
    }

    public enum SdkRequestType
    {
        NULL_TYPE = -1,
        FRIENDS_TIMELINE = 0,        //获取主页时间线(cmdNormalMessages)
        FRIENDS_TIMELINE_ID,
        UPLOAD_MESSAGE,             //发送微博(cmdUploadMessage)
        UPLOAD_MESSAGE_PIC,         //发送带图片微博(cmdUploadPic)
        FRIENDSHIP_CREATE,          //关注某用户(cmdFriendShip)
        FRIENDSHIP_DESDROY,         //取消关注(cmdFriendShip)
        FRIENDSHIP_SHOW,            //获取两个用户关系的详细情况(cmdFriendShip)
        FRIENDSHIP_FRIENDS,         //获取用户的关注列表
        FRIENDSHIP_FOLLOWERS,        //获取用户的粉丝列表
        AT_USERS,                   //@用户时的联想建议 (cmdAtUsers)
        USER_TIMELINE,              //获取用户发布的微博消息列表(cdmUserTimeline)
        MENTIONS_TIMELINE,           //获取最新的提到登录用户的微博列表，即@我的微博 
        COMMENTS_TIMELINE,           //获取指定微博的评论列表
        FAVORITE_TIMLINE,            //获取当前登录用户的收藏列表
        USERS_SHOW,                  //根据用户ID获取用户信息
        GET_UID,                      //OAuth授权之后，获取授权用户的UID 
        STATUSES_SHOW,                //根据Id获取单条微博的详细信息
        STATUSES_REPOST,              //转发一条微博
        STATUSES_DESTROY,             //删除一条微博
        COMMENTS_CREAT,               //对一条微博进行评论 
        COMMENTS_REPLY,               //回复一条评论
        FAVORITES_CREATE,             //添加一条微博到收藏
        FAVORITES_DESTROY             //取消收藏一条微博
    }

    //鉴权方式
    public enum EumAuth
    {
        OAUTH1_0 = 0,
        OAUTH2_0
    }


    public class SdkCmdBase
    {
        public string acessToken = "";
        //public string acessTokenSecret = "";

        //public string requestId = "";
    }

    /// <summary>
    /// TIMELINE
    /// </summary>
    public abstract class CmdTimeline : SdkCmdBase
    {
        public string since_id = "";
        public string max_id = "";
        public string count = "";
        public string page = "";
    }


    public class CmdStatusTimeline:CmdTimeline
    {
        public string feature = "";
        public string baseApp = "";
        public string trim_user = "";
    }

    public class CmdCommentsTimeline : CmdTimeline
    {
        public string id = "";
        public string filter_by_author = "";
        public string filter_by_source = "";
    }

    /// <summary>
    /// USER_TIMELINE
    /// </summary>
    public class CmdUserTimeline :CmdTimeline
    {
        public string uid = "";
        public string screen_name = "";
        public string baseApp = "";
        public string feature = "";
        public string trim_user = "";
    }

    public class CmdStatus : SdkCmdBase
    {
        public string id = "";
    }

    public class CmdStatusRepost : CmdStatus
    {
        public string status = "";
        public string is_comment = "";
        public string rip = "";
    }

    public class CmdStatusComment : CmdStatus
    {
        public string comment = "";
        public string comment_ori = "";
        public string rip = "";
        public string cid = "";
        public string without_mention = "";
    }

    public abstract class CmdUpload : SdkCmdBase
    {
        public string visible = "";
        public string list_id = "";
        public string lat = "";
        public string @long = "";
        public string annotations = "";
        public string rip = "";
    }

    public class CmdUploadMessage : CmdUpload
    {
        public string status = "";
    }

    public class CmdUploadMsgWithPic : CmdUpload
    {
        public string status = "";
        public UploadPictureHelper pic = null;
    }


    public class CmdFriendShip : CmdUserInfo
    {
        //单页返回的记录条数，默认为50，最大不超过200
        public string count = "";
        public string cursor = "";
        public string trim_status = "";
        public string rip = "";
    }

    public class CmdUserInfo : SdkCmdBase
    {
        //参数uid与screen_name二者必选其一，且只能选其一 
        public string uid = "";
        public string screen_name = "";
    }

}
