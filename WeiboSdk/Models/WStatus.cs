using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WeiboSdk.Models
{
    /// <summary>
    /// 新浪微博Model类
    /// </summary>
    [DataContract]
    [Table]
    public class WStatus : BaseModel
    {
        private string createdAt;

        /// <summary>
        /// 微博创建时间
        /// </summary>
        [DataMember(Name = "created_at")]
        [Column(UpdateCheck = UpdateCheck.Never)]
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
                    NotifyPropertyChanged("createdAt");
                }
            }
        }


        private long id;

        /// <summary>
        /// 微博Id
        /// </summary>
        [DataMember(Name = "id")]
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.Default, CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
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

        private long mid;

        /// <summary>
        /// 微博MId
        /// </summary>
        [DataMember(Name = "mid")]
        [Column(UpdateCheck = UpdateCheck.Never)]
        public long MId
        {
            get
            {
                return mid;
            }
            set
            {
                if (value != mid)
                {
                    mid = value;
                    NotifyPropertyChanged("mid");
                }
            }
        }

        private string idStr;

        /// <summary>
        /// 字符串型的微博ID
        /// </summary>
        [DataMember(Name = "idstr")]
        [Column(UpdateCheck = UpdateCheck.Never)]
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

        private string text;
        /// <summary>
        /// 微博信息内容
        /// </summary>
        [DataMember(Name = "text")]
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value != text)
                {
                    text = value;
                    NotifyPropertyChanged("Text");
                }
            }
        }


        private string source;
        /// <summary>
        /// 微博来源
        /// </summary>
        [DataMember(Name = "source")]
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                if (value != source)
                {
                    source = value;
                    NotifyPropertyChanged("Source");
                }
            }
        }

        private bool favorited;
        /// <summary>
        /// 是否已收藏
        /// </summary>
        [DataMember(Name = "favorited")]
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool Favorited
        {
            get
            {
                return favorited;
            }
            set
            {
                if (value != favorited)
                {
                    favorited = value;
                    NotifyPropertyChanged("Favorited");
                }
            }
        }


        private bool truncated;

        /// <summary>
        /// 是否被截断
        /// </summary>
        [DataMember(Name = "truncated")]
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool Truncated
        {
            get
            {
                return truncated;
            }
            set
            {
                if (value != truncated)
                {
                    truncated = value;
                    NotifyPropertyChanged("Truncated");
                }
            }
        }

        private List<PicUrl> picUrls;

        [DataMember(Name = "pic_urls")]
        public List<PicUrl> PicUrls
        {
            get
            {
                return picUrls;
            }
            set
            {
                if (value != picUrls)
                {
                    picUrls = value;
                    NotifyPropertyChanged("PicUrls");
                }
            }
        }


        private string picsStr;
        [Column(CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public string PicsStr
        {
            get
            {
                return picsStr;
            }
            set
            {
                if (value != picsStr)
                {
                    picsStr = value;
                    NotifyPropertyChanged("PicsId");
                }
            }
        }



        private string thumbnailPic;
        /// <summary>
        /// 缩略图片地址
        /// </summary>
        [DataMember(Name = "thumbnail_pic")]
        [Column(CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public string ThumbnailPic
        {
            get
            {
                return thumbnailPic;
            }
            set
            {
                if (value != thumbnailPic)
                {
                    thumbnailPic = value;
                    NotifyPropertyChanged("ThumbnailPic");
                }
            }
        }


        private string bmiddlePic;

        /// <summary>
        /// 中等尺寸图片地址
        /// </summary>
        [DataMember(Name = "bmiddle_pic")]
        [Column(CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public string BMiddlePic
        {
            get
            {
                return bmiddlePic;
            }
            set
            {
                if (value != bmiddlePic)
                {
                    bmiddlePic = value;
                    NotifyPropertyChanged("BMiddlePic");
                }
            }
        }


        private string originalPic;

        /// <summary>
        /// 原始图片地址
        /// </summary>
        [DataMember(Name = "original_pic")]
        [Column(CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public string OriginalPic
        {
            get
            {
                return originalPic;
            }
            set
            {
                if (value != originalPic)
                {
                    originalPic = value;
                    NotifyPropertyChanged("OriginalPic");
                }
            }
        }

        ///// <summary>
        ///// 地理信息
        ///// </summary>
        //[DataMember(Name = "geo")]
        //public object Geo { get; set; }

        private WUser user;
        /// <summary>
        /// 微博作者的用户信息
        /// </summary>
        [DataMember(Name = "user")]
        //[Column(DbType = "sql_variant")]
        public WUser User
        {
            get
            {
                return user;
            }
            set
            {
                if (value != user)
                {
                    user = value;
                    NotifyPropertyChanged("User");
                }
            }
        }

        private long? userId;
        [Column(DbType = "BIGINT", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public long? UserId
        {
            get
            {
                return userId;
            }
            set
            {
                if (value != userId)
                {
                    userId = value;
                    NotifyPropertyChanged("UserId");
                }
            }
        }

        private WStatus retweetedStatus;
        /// <summary>
        /// 被转发的原微博信息
        /// </summary>
        [DataMember(Name = "retweeted_status")]
        public WStatus RetweetedStatus
        {
            get
            {
                return retweetedStatus;
            }
            set
            {
                if (value != retweetedStatus)
                {
                    retweetedStatus = value;
                    NotifyPropertyChanged("RetweetedStatus");
                }
            }
        }


        private long? retweetedStatusId;
        [Column(DbType = "BIGINT", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public long? RetweetedStatusId
        {
            get
            {
                return retweetedStatusId;
            }
            set
            {
                if (value != retweetedStatusId)
                {
                    retweetedStatusId = value;
                    NotifyPropertyChanged("RetweetedStatusId");
                }
            }
        }



        private int repostsCount;
        /// <summary>
        /// 转发数
        /// </summary>
        [DataMember(Name = "reposts_count")]
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int RepostsCount
        {
            get
            {
                return repostsCount;
            }
            set
            {
                if (value != repostsCount)
                {
                    repostsCount = value;
                    NotifyPropertyChanged("RepostsCount");
                }
            }
        }


        private int commentsCount;
        /// <summary>
        /// 评论数
        /// </summary>
        [DataMember(Name = "comments_count")]
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int CommentsCount
        {
            get
            {
                return commentsCount;
            }
            set
            {
                if (value != commentsCount)
                {
                    commentsCount = value;
                    NotifyPropertyChanged("CommentsCount");
                }
            }
        }


        private int attitudesCount;
        /// <summary>
        /// 赞数
        /// </summary>
        [DataMember(Name = "attitudes_count")]
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int AttitudesCount
        {
            get
            {
                return attitudesCount;
            }
            set
            {
                if (value != attitudesCount)
                {
                    attitudesCount = value;
                    NotifyPropertyChanged("AttitudesCount");
                }
            }
        }


        /// <summary>
        /// 是否是转发的微博
        /// </summary>
        public bool IsRetweetedStatus
        {
            get
            {
                return RetweetedStatus == null ? false : true;
            }
        }

        /// <summary>
        /// 微博是否带图片
        /// </summary>
        public bool HasPic
        {
            get
            {
                return string.IsNullOrEmpty(ThumbnailPic) ? false : true;
            }
        }


        public DateTime CreateDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(this.CreatedAt))
                    return DateTime.MinValue;
                string[] array = this.CreatedAt.Split(' ');
                array[4] = array[4].Substring(0, 3) + ":" + array[4].Substring(3, 2);
                string text = string.Join(" ", array);
                return DateTime.ParseExact(text, "ddd MMM dd HH:mm:ff zzz yyyy", new System.Globalization.CultureInfo("en-US"));
            }
        }


    }

    [DataContract]
    public class PicUrl
    {
        [DataMember(Name = "thumbnail_pic")]
        public string ThumbnailPic { get; set; }

        public string BMiddlePic
        {
            get
            {
                return this.ThumbnailPic.Replace("thumbnail", "bmiddle");
            }
        }

        public bool IsGif
        {
            get
            {
                return this.ThumbnailPic.EndsWith(".gif") ? true : false;
            }
        }

    }
}
