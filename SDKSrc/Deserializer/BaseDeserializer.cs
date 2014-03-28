using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TencentWeiboSDK.Model;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TencentWeiboSDK.Deserializer
{
    /// <summary>
    /// 反序列化器基类，提供各种数据格式都可能用到的接口.
    /// </summary>
    public abstract class BaseDeserializer<T> where T:BaseModel
    {
        /// <summary>
        /// 反序列化单个对象.
        /// </summary>
        /// <param name="content">需要反序列化成单个对象的字符串.</param>
        /// <returns>返回对象.</returns>
        public abstract T Read(string content);

        /// <summary>
        /// 反序列化对象列表.
        /// </summary>
        /// <param name="content">需要反序列化成列表的字符串.</param>
        /// <returns>返回对象列表</returns>
        public abstract List<T> ReadList(string content);
    }
}
