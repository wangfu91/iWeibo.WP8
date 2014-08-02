using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WeiboSdk.Models
{
    /// <summary>
    /// 新浪微博用户信息
    /// </summary>
    [DataContract]
    [Table]
    public class WUser : BaseModel
    {

        private long id;
        /// <summary>
        /// 用户UID
        /// </summary>
        [DataMember(Name = "id")]
        [Column(IsPrimaryKey = true)]
        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }


        private string idStr;
        /// <summary>
        /// 字符串型的用户UID
        /// </summary>
        [DataMember(Name = "idstr")]
        [Column]
        public string IdStr
        {
            get
            {
                return idStr;
            }
            set
            {
                if (value != idStr)
                {
                    idStr = value;
                    NotifyPropertyChanged("IdStr");
                }
            }
        }


        private string screenName;
        /// <summary>
        /// 用户昵称
        /// </summary>
        [DataMember(Name = "screen_name")]
        [Column]
        public string ScreenName
        {
            get
            {
                return screenName;
            }
            set
            {
                if (value != screenName)
                {
                    screenName = value;
                    NotifyPropertyChanged("ScreenName");
                }
            }
        }


        private string name;
        /// <summary>
        /// 友好显示名称
        /// </summary>
        [DataMember(Name = "name")]
        [Column]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }


        private int province;
        /// <summary>
        /// 用户所在省级ID
        /// </summary>
        [DataMember(Name = "province")]
        [Column]
        public int Province
        {
            get
            {
                return province;
            }
            set
            {
                if (value != province)
                {
                    province = value;
                    NotifyPropertyChanged("Province");
                }
            }
        }


        private int city;
        /// <summary>
        /// 用户所在城市ID
        /// </summary>
        [DataMember(Name = "city")]
        [Column]
        public int City
        {
            get
            {
                return city;
            }
            set
            {
                if (value != city)
                {
                    city = value;
                    NotifyPropertyChanged("City");
                }
            }
        }


        private string location;
        /// <summary>
        /// 用户所在地
        /// </summary>
        [DataMember(Name = "location")]
        [Column]
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                if (value != location)
                {
                    location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }


        private string description;
        /// <summary>
        /// 用户个人描述
        /// </summary>
        [DataMember(Name = "description")]
        [Column]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }


        private string url;
        /// <summary>
        /// 用户博客地址
        /// </summary>
        [DataMember(Name = "url")]
        [Column]
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                if (value != url)
                {
                    url = value;
                    NotifyPropertyChanged("Url");
                }
            }
        }


        private string profileImageUrl;
        /// <summary>
        /// 用户头像地址
        /// </summary>
        [DataMember(Name = "profile_image_url")]
        [Column]
        public string ProfileImageUrl
        {
            get
            {
                return profileImageUrl;
            }
            set
            {
                if (value != profileImageUrl)
                {
                    profileImageUrl = value;
                    NotifyPropertyChanged("ProfileImageUrl");
                }
            }
        }



        private string profileUrl;
        /// <summary>
        /// 用户的微博统一URL地址
        /// </summary>
        [DataMember(Name = "profile_url")]
        [Column]
        public string ProfileUrl
        {
            get
            {
                return profileUrl;
            }
            set
            {
                if (value != profileUrl)
                {
                    profileUrl = value;
                    NotifyPropertyChanged("ProfileUrl");
                }
            }
        }

        private string domain;
        /// <summary>
        /// 用户的个性化域名
        /// </summary>
        [DataMember(Name = "domain")]
        [Column]
        public string Domain
        {
            get
            {
                return domain;
            }
            set
            {
                if (value != domain)
                {
                    domain = value;
                    NotifyPropertyChanged("Domain");
                }
            }
        }


        private string weiHao;
        /// <summary>
        /// 用户的微号
        /// </summary>
        [DataMember(Name = "weihao")]
        [Column]
        public string WeiHao
        {
            get
            {
                return weiHao;
            }
            set
            {
                if (value != weiHao)
                {
                    weiHao = value;
                    NotifyPropertyChanged("WeiHao");
                }
            }
        }


        private string gender;
        /// <summary>
        /// 性别，m:男、f:女、n:未知
        /// </summary>
        [DataMember(Name = "gender")]
        [Column]
        public string Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
                NotifyPropertyChanged("Gender");
            }
        }


        private int followersCount;
        /// <summary>
        /// 粉丝数
        /// </summary>
        [DataMember(Name = "followers_count")]
        [Column]
        public int FollowersCount
        {
            get
            {
                return followersCount;
            }
            set
            {
                if (value != followersCount)
                {
                    followersCount = value;
                    NotifyPropertyChanged("FollowersCount");
                }
            }
        }


        private int friendsCount;
        /// <summary>
        /// 关注数
        /// </summary>
        [DataMember(Name = "friends_count")]
        [Column]
        public int FriendsCount
        {
            get
            {
                return friendsCount;
            }
            set
            {
                if (value != friendsCount)
                {
                    friendsCount = value;
                    NotifyPropertyChanged("FriendsCount");
                }
            }
        }


        private int statusesCount;
        /// <summary>
        /// 微博数
        /// </summary>
        [DataMember(Name = "statuses_count")]
        [Column]
        public int StatusesCount
        {
            get
            {
                return statusesCount;
            }
            set
            {
                if (value != statusesCount)
                {
                    statusesCount = value;
                    NotifyPropertyChanged("StatusCount");
                }
            }
        }


        private int favoritesCount;
        /// <summary>
        /// 收藏数
        /// </summary>
        [DataMember(Name = "favourites_count")]
        [Column]
        public int FavoritesCount
        {
            get
            {
                return favoritesCount;
            }
            set
            {
                if (value != favoritesCount)
                {
                    favoritesCount = value;
                    NotifyPropertyChanged("FavoritesCount");
                }
            }
        }


        private string createdAt;
        /// <summary>
        /// 用户注册时间
        /// </summary>
        [DataMember(Name = "created_at")]
        [Column]
        public string CreatedAt
        {
            get
            {
                return createdAt;
            }
            set
            {
                if (value != createdAt)
                {
                    createdAt = value;
                    NotifyPropertyChanged("CreatedAt");
                }
            }
        }


        private bool allowAllActMsg;
        /// <summary>
        /// 是否允许所有人给我发私信
        /// </summary>
        [DataMember(Name = "allow_all_act_msg")]
        [Column]
        public bool AllowAllActMsg
        {
            get
            {
                return allowAllActMsg;
            }
            set
            {
                if (value != allowAllActMsg)
                {
                    allowAllActMsg = value;
                    NotifyPropertyChanged("AllowAllActMsg");
                }
            }
        }


        private bool geoEnable;
        /// <summary>
        /// 是否允许标识用户的地理位置
        /// </summary>
        [DataMember(Name = "geo_enabled")]
        [Column]
        public bool GeoEnabled
        {
            get
            {
                return geoEnable;
            }
            set
            {
                if (value != geoEnable)
                {
                    geoEnable = value;
                    NotifyPropertyChanged("GeoEnable");
                }
            }
        }


        private bool verified;
        /// <summary>
        /// 是否是微博认证用户，即加V用户
        /// </summary>
        [DataMember(Name = "verified")]
        [Column]
        public bool Verified
        {
            get
            {
                return verified;
            }
            set
            {
                if (value != verified)
                {
                    verified = value;
                    NotifyPropertyChanged("Verified");
                }
            }
        }

        private string remark;
        /// <summary>
        /// 用户备注信息
        /// </summary>
        [DataMember(Name = "remark")]
        [Column]
        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                if (value != remark)
                {
                    remark = value;
                    NotifyPropertyChanged("Remark");
                }
            }
        }


        ///// <summary>
        ///// 用户的最近一条微博信息字段
        ///// </summary>
        //[DataMember(Name = "status")]
        //public WStatus Status { get; set; }


        private bool allowAllComment;
        /// <summary>
        /// 是否允许所有人对我的微博进行评论
        /// </summary>
        [DataMember(Name = "allow_all_comment")]
        [Column]
        public bool AllowAllComment
        {
            get
            {
                return allowAllComment;
            }
            set
            {
                if (value != allowAllComment)
                {
                    allowAllComment = value;
                    NotifyPropertyChanged("AllowAllComment");
                }
            }
        }


        private string avatarLarge;
        /// <summary>
        /// 用户头像地址（大图），180×180像素 
        /// </summary>
        [DataMember(Name = "avatar_large")]
        [Column]
        public string AvatarLarge
        {
            get
            {
                return avatarLarge;
            }
            set
            {
                if (value != avatarLarge)
                {
                    avatarLarge = value;
                    NotifyPropertyChanged("AvatarLarge");
                }
            }
        }


        private string avatarHd;
        /// <summary>
        /// 用户头像地址（高清），高清头像原图 
        /// </summary>
        [DataMember(Name = "avatar_hd")]
        [Column]
        public string AvatarHD
        {
            get
            {
                return avatarHd;
            }
            set
            {
                if (value != avatarHd)
                {
                    avatarHd = value;
                    NotifyPropertyChanged("AvatarHD");
                }
            }
        }


        private string verifiedReason;
        /// <summary>
        /// 认证原因 
        /// </summary>
        [DataMember(Name = "verified_reason")]
        [Column]
        public string VerifiedReason
        {
            get
            {
                return verifiedReason;
            }
            set
            {
                if (value != verifiedReason)
                {
                    verifiedReason = value;
                    NotifyPropertyChanged("VerifiedReason");
                }
            }
        }


        private bool followMe;
        /// <summary>
        /// 该用户是否关注当前登录用户
        /// </summary>
        [DataMember(Name = "follow_me")]
        [Column]
        public bool FollowMe
        {
            get
            {
                return followMe;
            }
            set
            {
                if (value != followMe)
                {
                    followMe = value;
                    NotifyPropertyChanged("FollowMe");
                }
            }
        }


        private int onlineStatus;
        /// <summary>
        /// 用户的在线状态，0：不在线、1：在线
        /// </summary>
        [DataMember(Name = "online_status")]
        [Column]
        public int OnLineStatus
        {
            get
            {
                return onlineStatus;
            }
            set
            {
                if (value != onlineStatus)
                {
                    onlineStatus = value;
                    NotifyPropertyChanged("OnLineStatus");
                }
            }
        }


        private int biFollowersCount;
        /// <summary>
        /// 用户的互粉数 
        /// </summary>
        [DataMember(Name = "bi_followers_count")]
        [Column]
        public int BiFollowersCount
        {
            get
            {
                return biFollowersCount;
            }
            set
            {
                if (value != biFollowersCount)
                {
                    biFollowersCount = value;
                    NotifyPropertyChanged("BiFollowersCount");
                }
            }
        }


        private string lang;
        /// <summary>
        /// 用户当前的语言版本，zh-cn：简体中文，zh-tw：繁体中文，en：英语 
        /// </summary>
        [DataMember(Name = "lang")]
        [Column]
        public string Lang
        {
            get
            {
                return lang;
            }
            set
            {
                lang = value;
                NotifyPropertyChanged("Lang");
            }
        }

    }
}
