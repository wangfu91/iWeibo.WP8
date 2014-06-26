using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 微腾微博列表，包含微博对象，以及微博所提到的用户列表
    /// </summary>
    public class StatusCollection:List<Status>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusCollection()
        {
            this.Users = new List<User>();
        }

        /// <summary>
        /// 该列表中所提到的用户列表.
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// 列表中最后一条微博的时间戳
        /// </summary>
        public long LastTimeStamp
        {
            get
            {
                //return this[this.Count - 1].TimeStamp;
                return this.Count > 0 ? this[this.Count - 1].TimeStamp : 0;
            }
        }

        /// <summary>
        /// 列表中第一条微博的时间戳
        /// </summary>
        public long FirstTimeStamp
        {
            get
            {
                return this.Count > 0 ? this[0].TimeStamp : 0;
            }
        }
    }
}