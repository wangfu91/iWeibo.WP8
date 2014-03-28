using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 用户 Model，用来表示微博用户的对象.
    /// </summary>
    [DataContract]
    public class User : BaseModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public User()
        { }

        /// <summary>
        /// 出生天
        /// </summary>
        [DataMember(Name = "birth_day")]
        public string Birth_Day { get; set; }

        /// <summary>
        /// 出生月
        /// </summary>
        [DataMember(Name = "birth_month")]
        public string Birth_Month { get; set; }

        /// <summary>
        /// 出生年
        /// </summary>
        [DataMember(Name = "birth_year")]
        public string Birth_Year { get; set; }


        /// <summary>
        /// 城市Id
        /// </summary>
        [DataMember(Name = "city_code")]
        public string City_Code { get; set; }


        /// <summary>
        /// 公司信息列表
        /// </summary>
        [DataMember(Name = "comp")]
        public List<Company> Comp { get; set; }


        /// <summary>
        /// 国家Id
        /// </summary>
        [DataMember(Name = "country_code")]
        public string Country_Code { get; set; }
 

        /// <summary>
        /// 教育信息列表
        /// </summary>
        [DataMember(Name = "edu")]
        public List<Education> Edu { get; set; }


        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }


        /// <summary>
        /// 经验值
        /// </summary>
        [DataMember(Name = "exp")]
        public int Exp { get; set; }

                
        /// <summary>
        /// 听众数
        /// </summary>
        [DataMember(Name = "fansnum")]
        public int FansNum { get; set; }


        /// <summary>
        /// 收藏数
        /// </summary>
        [DataMember(Name = "favnum")]
        public int FavNum { get; set; }


        /// <summary>
        /// 头像Url
        /// </summary>
        [DataMember(Name = "head")]
        public string Head { get; set; }


        /// <summary>
        /// 家乡所在城市Id
        /// </summary>
        [DataMember(Name = "homecity_code")]
        public string HomeCity_Code { get; set; }


        /// <summary>
        /// 家乡所在国家Id
        /// </summary>
        [DataMember(Name = "homecountry_code")]
        public string HomeCountry_Code { get; set; }


        /// <summary>
        /// 个人主页
        /// </summary>
        [DataMember(Name = "homepage")]
        public string HomePage { get; set; }


        /// <summary>
        /// 家乡所在省Id
        /// </summary>
        [DataMember(Name = "homeprovince_code")]
        public string HomeProvince_Code { get; set; }


        /// <summary>
        /// 家乡所在城镇Id
        /// </summary>
        [DataMember(Name = "hometown_code")]
        public string HomeTown_Code { get; set; }


        /// <summary>
        /// 收听的人数
        /// </summary>
        [DataMember(Name = "idolnum")]
        public int IdolNum { get; set; }


        /// <summary>
        /// 行业Id
        /// </summary>
        [DataMember(Name = "industry_code")]
        public string Industry_Code { get; set; }


        /// <summary>
        /// 个人介绍
        /// </summary>
        [DataMember(Name = "introduction")]
        public string Introduction { get; set; }


        /// <summary>
        /// 是否企业机构
        /// </summary>
        [DataMember(Name = "isent")]
        public int IsEnt { get; set; }


        /// <summary>
        /// 是否在我的黑名单中
        /// </summary>
        [DataMember(Name = "ismyblack")]
        public int IsMyBlack { get; set; }


        /// <summary>
        /// 是否收听我
        /// </summary>
        [DataMember(Name = "ismyfans")]
        public int IsMyFans { get; set; }


        /// <summary>
        /// 是否是我收听的人
        /// </summary>
        [DataMember(Name = "ismyidol")]
        public int IsMyIdol { get; set; }


        /// <summary>
        /// 是否实名认证
        /// </summary>
        [DataMember(Name = "isrealname")]
        public int IsRealName { get; set; }


        /// <summary>
        /// 是否是认证用户
        /// </summary>
        [DataMember(Name = "isvip")]
        public int IsVIP { get; set; }


        /// <summary>
        /// 等级
        /// </summary>
        [DataMember(Name = "level")]
        public int Level { get; set; }


        /// <summary>
        /// 所在地.
        /// </summary>
        [DataMember(Name = "location")]
        public string Location { get; set; }


        /// <summary>
        /// 互听好友数
        /// </summary>
        [DataMember(Name = "mutual_fans_num")]
        public int Mutual_Fans_Num { get; set; }


        /// <summary>
        /// 帐户名.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }


        /// <summary>
        /// 昵称.
        /// </summary>
        [DataMember(Name = "nick")]
        public string Nick { get; set; }


        /// <summary>
        /// 唯一id，与name相对应.
        /// </summary>
        [DataMember(Name = "openid")]
        public string OpenId { get; set; }


        /// <summary>
        /// 地区Id
        /// </summary>
        [DataMember(Name = "province_code")]
        public string Province_Code { get; set; }


        /// <summary>
        /// 注册时间 
        /// </summary>
        [DataMember(Name = "regtime")]
        public string RegTime { get; set; }


        /// <summary>
        /// 是否允许所有人给当前用户发私信
        /// </summary>
        /// <remarks>0-仅有偶像，1-名人+听众，2-所有人</remarks>
        [DataMember(Name = "send_private_flag")]
        public int Send_Private_Flag { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        /// <remarks>1-男，2-女，0-未填写</remarks>
        [DataMember(Name = "sex")]
        public int Sex { get; set; }


        /// <summary>
        /// 标签列表
        /// </summary>
        [DataMember(Name = "tag")]
        public List<Tag> Tag { get; set; }


        /// <summary>
        /// 最近的一条原创微博信息
        /// </summary>
        [DataMember(Name = "tweetinfo")]
        public List<Status> TweetInfo { get; set; }


        /// <summary>
        /// 发表的微博数量
        /// </summary>
        [DataMember(Name = "tweetnum")]
        public int TweetNum { get; set; }


        /// <summary>
        /// 认证信息
        /// </summary>
        [DataMember(Name = "verifyinfo")]
        public string VerifyInfo { get; set; }

    }
}
