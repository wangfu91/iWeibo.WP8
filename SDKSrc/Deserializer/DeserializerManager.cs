using System;
using System.Net;
using System.Reflection;
using TencentWeiboSDK.Deserializer.Json;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services.Util;

namespace TencentWeiboSDK.Deserializer
{
    /// <summary>
    /// 反序列化器管理器，根据数据类型返回用户所需要的反序列化器.
    /// </summary>
    public class DeserializerManager
    {
        private static DeserializerManager instance = new DeserializerManager();


        private DeserializerManager()
        { }

        /// <summary>
        /// 获取反序列化器管理器实例.
        /// </summary>
        public static DeserializerManager Instance
        {
            get {
                return instance;
            }
        }

        /// <summary>
        /// 构造微博(Status)对象的反序列化器.
        /// </summary>
        /// <returns>返回 StatusDeserializer 对象</returns>
        public StatusDeserializer BuildStatusDeserializer()
        {
            return new StatusDeserializer();
        }

        /// <summary>
        /// 构造用户(User)对象的反序列化器
        /// </summary>
        /// <returns>返回 UserDeserializer 对象</returns>
        public UserDeserializer BuildUserDeserializer()
        {
            return new UserDeserializer();
        }

        /// <summary>
        /// 构造返回结果(Result)对象的反序列化器
        /// </summary>
        /// <returns>返回 UserDeserializer 对象</returns>
        public ResultDeserializer BuildResultDeserializer()
        {
            return new ResultDeserializer();
        }

    }
}
