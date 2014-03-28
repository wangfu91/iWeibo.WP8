using System;

namespace TencentWeiboSDK.Hammock.Web
{
    public class HttpCookieParameter : WebParameter
    {
        public virtual Uri Domain { get; set; }

        public HttpCookieParameter(string name, string value) : base(name, value)
        {

        }
    }
}