using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TencentWeiboSDK.Model;

namespace TencentWeiboSDK.Deserializer.Json
{
    /// <summary>
    /// Json 格式的微博(Status)对象的反序列化器.
    /// </summary>
    public class StatusDeserializer : BaseDeserializer<Status>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusDeserializer()
        { }

        /// <summary>
        /// 将微博(Status)的 Json 字符串反序列化成 Status 对象.
        /// </summary>
        /// <param name="content">需要反符列化的Json字符串</param>
        /// <returns>返回 Status 对象</returns>
        public override Status Read(string content)
        {
            if (content == "")
                return new Status();

            var jo = JObject.Parse(content)["data"];

            Status status = jo.ToObject<Status>();
            return status;
        }

        /// <summary>
        /// 将微博(Status)的 Json 字符串反序列成 Status 对象列表.
        /// </summary>
        /// <param name="content">需要读取的Json字符串</param>
        /// <returns>返回 StatusCollection 对象</returns>
        public override List<Status> ReadList(string content)
        {
            //if (content == "")
            //    return new StatusCollection();

            var jo = JObject.Parse(content);

            if (jo["data"].ToString() == "")
                return new StatusCollection();

            var jInfo = jo["data"]["info"];
            StatusCollection list = new StatusCollection();
            if (jInfo != null)
            {
                foreach (var j in jInfo.Children())
                {

                    list.Add(j.ToObject<Status>());
                }
            }            

            var jUser = jo["data"]["user"];
            if (null != jUser)
            {
                foreach (JProperty u in jUser.Children())
                {
                    list.Users.Add(new User() { Name = u.Name, Nick = u.Value.ToString() });
                }
            }

            return list;
        }
    }
}
