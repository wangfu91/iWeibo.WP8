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
    public class WStatus:BaseModel
    {
        /// <summary>
        /// 微博创建时间
        /// </summary>
        [DataMember(Name="created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// 微博ID
        /// </summary>
        [DataMember(Name="id")]
        public long Id { get; set; }

        /// <summary>
        /// 微博MID
        /// </summary>
        [DataMember(Name="mid")]
        [Column]
        public long MId { get; set; }

        /// <summary>
        /// 字符串型的微博ID
        /// </summary>
        [DataMember(Name="idstr")]
        [Column(IsPrimaryKey = true, IsDbGenerated = false, DbType = "NVarChar(16) NOT NULL", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string IdStr { get; set; }

        [DataMember(Name="text")]
        private string text;
        /// <summary>
        /// 微博信息内容
        /// </summary>
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

        /// <summary>
        /// 微博来源
        /// </summary>
        [DataMember(Name="source")]
        public string Source { get; set; }

        [DataMember(Name="favorited")]
        private bool favorited;
        /// <summary>
        /// 是否已收藏
        /// </summary>
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

        /// <summary>
        /// 是否被截断
        /// </summary>
        [DataMember(Name="truncated")]
        public bool Truncated { get; set; }

        [DataMember(Name = "pic_urls")]
        public List<PicUrl> PicUrls { get; set; }

        /// <summary>
        /// 缩略图片地址
        /// </summary>
        [DataMember(Name = "thumbnail_pic")]
        public string ThumbnailPic { get; set; }

        /// <summary>
        /// 中等尺寸图片地址
        /// </summary>
        [DataMember(Name = "bmiddle_pic")]
        public string BMiddlePic { get; set; }

        /// <summary>
        /// 原始图片地址
        /// </summary>
        [DataMember(Name = "original_pic")]
        public string OriginalPic { get; set; }

        /// <summary>
        /// 地理信息
        /// </summary>
        [DataMember(Name = "geo")]
        public object Geo { get; set; }

        /// <summary>
        /// 微博作者的用户信息
        /// </summary>
        [DataMember(Name="user")]
        public WUser User { get; set; }

        /// <summary>
        /// 被转发的原微博信息
        /// </summary>
        [DataMember(Name ="retweeted_status")]
        public WStatus RetweetedStatus { get; set; }

        /// <summary>
        /// 转发数
        /// </summary>
        [DataMember(Name="reposts_count")]
        public int RepostsCount { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        [DataMember(Name="comments_count")]
        public int CommentsCount { get; set; }

        /// <summary>
        /// 赞数
        /// </summary>
        [DataMember(Name = "attitudes_count")]
        public int AttitudesCount { get; set; }

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

        /// <summary>
        /// 图片是否包含gif格式 
        /// </summary>
        //public bool IsGIF
        //{
        //    get
        //    {
        //        if (HasPic && PicUrls.Any(a => a.ToString().EndsWith(".gif")))
        //            return true;
        //        else
        //            return false;
        //    }
        //}


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
