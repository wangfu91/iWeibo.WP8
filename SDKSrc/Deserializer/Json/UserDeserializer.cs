using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TencentWeiboSDK.Model;

namespace TencentWeiboSDK.Deserializer.Json
{
    /// <summary>
    /// Json 格式的用户(User)对象的反序列化器. 
    /// </summary>
    public class UserDeserializer : BaseDeserializer<User>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserDeserializer()
        { }
        
        /// <summary>
        /// 将用户(User)的 Json 字符串反序列化成 User 对象.
        /// </summary>
        /// <param name="content">需要反符列化的Json字符串</param>
        /// <returns>返回 User 对象</returns>
        public override User Read(string content)
        {
            if(content == "")
                return new User();

            var je = JObject.Parse(content)["data"];
            User user = je.ToObject<User>();

            if (user == null)
                return new User();

            return user;
        }

        /// <summary>
        /// 将用户(User)的 Json 字符串反序列成 User 对象列表.
        /// </summary>
        /// <param name="content">需要读取的Json字符串.</param>
        /// <returns>返回 User 的列表对象.</returns>
        public override List<User> ReadList(string content)
        {
            var jo = JObject.Parse(content);
            if (content == "")
                return new List<User>();
            if (jo["data"].ToString() == "")
                return new List<User>();
            var jInfo = jo["data"]["info"];
            if(jo.ToString()=="")
                return new List<User>();

            UserColloection list=new UserColloection();
            if(jInfo!=null)
            {
                foreach (var j in jInfo.Children())
                {
                    User user = j.ToObject<User>();
                    list.Add(user);
                }
            }

            var jNextStartPos = jo["data"]["nextstartpos"];
            if(jNextStartPos!=null)
            {
                list.NextStartPos = jNextStartPos.Value<int>();
            }

            return list;
        }
    }
}
