using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 微博的 Model, 用于表示微博的对象.
    /// </summary>
    [DataContract]
    public class Status : BaseModel
    {
        private int count = 0;
        private int likecount = 0;
        private int mcount = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Status()
        { }

        /// <summary>
        /// 城市代码
        /// </summary>
        [DataMember(Name="city_code")]
        public string City_Code { get; set; }

        /// <summary>
        /// 微博被转次数
        /// </summary>
        [DataMember(Name="count")]
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (value != count)
                {
                    count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }

        /// <summary>
        /// 国家代码
        /// </summary>
        [DataMember(Name="country_code")]
        public string Country_Code { get; set; }


        /// <summary>
        /// 心情类型
        /// </summary>
        [DataMember(Name="emotiontype")]
        public string EmotionType { get; set; }

        /// <summary>
        /// 心情图片url
        /// </summary>
        [DataMember(Name="emotionurl")]
        public string EmotionUrl { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [DataMember(Name="from")]
        public string From { get; set; }


        /// <summary>
        /// 来源url
        /// </summary>
        [DataMember(Name = "fromurl")]
        public string FromUrl { get; set; }


        /// <summary>
        /// 发表者地理信息
        /// </summary>
        [DataMember(Name = "geo")]
        public object Geo { get; set; }


        /// <summary>
        /// 发表者头像url
        /// </summary>
        [DataMember(Name = "head")]
        public string Head { get; set; }


        /// <summary>
        /// Https_Head
        /// </summary>
        [DataMember(Name = "https_head")]
        public string Https_Head { get; set; }


        /// <summary>
        /// 微博唯一id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// 图片url列表
        /// </summary>
        [DataMember(Name = "image")]
        public List<string> Image { get; set; }


        /// <summary>
        /// 是否实名认证，0-老用户，1-已实名认证，2-未实名认证,
        /// </summary>
        [DataMember(Name = "isrealname")]
        public int IsRealName { get; set; }


        /// <summary>
        /// 是否微博认证用户，0-不是，1-是
        /// </summary>
        [DataMember(Name = "isvip")]
        public int IsVIP { get; set; }

        
        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }


        /// <summary>
        /// 经度
        /// </summary>
        [DataMember(Name = "longitude")]
        public string Longitude { get; set; }

        /// <summary>
        /// 微博赞数量
        /// </summary>
        [DataMember(Name="likecount")]
        public int LikeCount
        {
            get
            {
                return likecount;
            }
            set
            {
                if (value != likecount)
                {
                    likecount = value;
                    NotifyPropertyChanged("LikeCount");
                }
            }
        }

        /// <summary>
        /// 发表者所在地
        /// </summary>
        [DataMember(Name = "location")]
        public string Location { get; set; }


        /// <summary>
        /// 点评次数
        /// </summary>
        [DataMember(Name="mcount")]
        public int MCount
        {
            get
            {
                return mcount;
            }
            set
            {
                if (value != mcount)
                {
                    mcount = value;
                    NotifyPropertyChanged("MCount");
                }
            }
        }


        /// <summary>
        /// 发表人帐户名
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }


        /// <summary>
        /// 发表人昵称
        /// </summary>
        [DataMember(Name = "nick")]
        public string Nick { get; set; }


        /// <summary>
        /// 用户唯一id，与name相对应
        /// </summary>
        [DataMember(Name = "openid")]
        public string OpenId { get; set; }


        /// <summary>
        /// 原始内容
        /// </summary>
        [DataMember(Name = "origtext")]
        public string OrigText { get; set; }


        /// <summary>
        /// 图片信息列表
        /// </summary>
        [DataMember(Name = "pic")]
        public Pictures Pic { get; set; }


        /// <summary>
        /// 省份代码
        /// </summary>
        [DataMember(Name = "province_code")]
        public string Province_Code { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "readcount")]
        public int ReadCount { get; set; }


        /// <summary>
        /// 是否自已发的的微博,0-不是，1-是
        /// </summary>
        [DataMember(Name = "self")]
        public int Self { get; set; }


        /// <summary>
        /// 当type=2时，source即为源tweet
        /// </summary>
        [DataMember(Name = "source")]
        public Status Source { get; set; }


        /// <summary>
        /// 微博状态，0-正常，1-系统删除，2-审核中，3-用户删除，4-根删除
        /// </summary>
        [DataMember(Name="status")]
        public int State { get; set; }

        /// <summary>
        /// 添加到收藏的时间
        /// </summary>
        [DataMember(Name="storetime")]
        public string StoreTime { get; set; }


        /// <summary>
        /// 微博内容
        /// </summary>      
        [DataMember(Name = "text")]
        public string Text { get; set; }


        /// <summary>
        /// 发表时间戳
        /// </summary>
        [DataMember(Name = "timestamp")]
        public long TimeStamp { get; set; }


        /// <summary>
        /// 微博类型，1-原创发表，2-转载，3-私信，4-回复，5-空回，6-提及，7-评论
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }


        /// <summary>
        /// （自定义只读属性）此微博是否已添加到收藏
        /// </summary>
        public bool IsFavorite
        {
            get
            {
                return string.IsNullOrEmpty(StoreTime) ? false : true;
            }
        }

        /// <summary>
        /// （自定义只读属性）是否是自己发的微博
        /// </summary>
        public bool IsSelf
        {
            get
            {
                return Self == 1 ? true : false;
            }
        }

        /// <summary>
        /// （自定义只读属性）此微博是否包含图片
        /// </summary>
        public bool HasPic
        {
            get
            {
                return Image == null ? false : true;
            }
        }

        /// <summary>
        /// （自定义只读属性）此微博图片的地址
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return Image == null ? "" : Image[0];
            }
        }

        /// <summary>
        /// （自定义只读属性）此微博是否是转发的微博
        /// </summary>
        public bool IsRetweetedStatus
        {
            get
            {
                return Source == null ? false : true;
            }
        }
    }
}
