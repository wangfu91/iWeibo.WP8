using Shared;
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

namespace TencentWeiboSDK.Services.Util
{
    /// <summary>
    /// Service 参数列表.
    /// </summary>
    /// <remarks>
    /// 同时请参考 http://wiki.open.t.qq.com/ 设置正确的参数，以及其取值范围.
    /// 否则可能导致签名错误，或不可意料的异常.
    /// </remarks>
    public class ServiceArgument
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        public ServiceArgument()
        {
            this.Format = DataFormat.Json;
        }
        
        /// <summary>
        /// 获取或设置分页标识.
        /// </summary>
        /// <remarks>
        /// 0：第一页，1：向下翻页，2：向上翻页.
        /// </remarks>
        public int PageFlag { get; set; }
		
		
        public long Lastid { get; set; }
        public int Flag { get; set; }
        //搜索用户分页
        public int Page { get; set; }

        /// <summary>
        /// 获取或设置本页起始时间
        /// </summary>
        /// <remarks>
        /// 第一页：填0，向上翻页：填上一次请求返回的第一条记录时间，向下翻页：
        /// 填上一次请求返回的最后一条记录时间.
        /// </remarks>
        public long PageTime { get; set; }

        /// <summary>
        /// 获取或设置每次请求记录的条数 .
        /// </summary>
        public int Reqnum { get; set; }
        public int Pagesize { get; set; }
        

        /// <summary>
        /// 获取或设置用户/微博 Id.
        /// </summary>
        public string Id { get; set; }

        public string Keyword { get; set; }
        public string Rootid { get; set; }
        /// <summary>
        /// 获取或设置拉取类型 .
        /// </summary>
        /// <remarks>
        /// 0x1 原创发表 
        /// 0x2 转载 
        /// 0x8 回复 
        /// 0x10 空回 
        /// 0x20 提及 
        /// 0x40 点评 
        /// 如需拉取多个类型请使用|，如(0x1|0x2)得到3，则type=3即可，填零表示拉取所有类型 .
        /// </remarks>
        public int Type { get; set; }

        /// <summary>
        /// 获取或设置内容过滤.
        /// </summary>
        /// <remarks>
        /// 0-表示所有类型，1-带文本，2-带链接，4-图片，8-带视频，0x10-带音频.
        /// </remarks>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// 获取或设置权限标识。1－表示只显示我发表的，默认填零.
        /// </summary>
        public int  AccessLevel { get; set; }

        /// <summary>
        /// 获取或设置你需要读取的用户的用户名（可选）. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置你需要读取的用户名列表，中间用_隔开.
        /// </summary>
        public string FOpenIds { get; set; }

        /// <summary>
        /// 获取或设置你需要读取的用户的openid（可选）.
        /// </summary>
        public string FOpenId { get; set; }

        /// <summary>
        /// 获取或设置起始位置.
        /// </summary>
        /// <remarks>
        /// 第一页填0，继续向下翻页：填【reqnum*（page-1）】.
        /// </remarks>
        public int StartIndex  { get; set; }

        /// <summary>
        /// 获取或设置微博内容.
        /// </summary>
        public string Content { get; set; }

        public string Reid { get; set; }

        /// <summary>
        /// 获取或设置需要上传的 Pic.
        /// </summary>
        public UploadPictureHelper Pic { get; set; }

        /// <summary>
        /// 获取或设置返回数据的格式.
        /// </summary>
        /// <remarks>
        /// json或xml, 但暂时只提供Json方式.
        /// </remarks>
        public DataFormat Format { get; set; }
    }
}
