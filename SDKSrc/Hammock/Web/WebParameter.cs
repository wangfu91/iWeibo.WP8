#if !Smartphone
using System.Diagnostics;
#endif
using System;
namespace TencentWeiboSDK.Hammock.Web
{
#if !Smartphone
    [DebuggerDisplay("{Name}:{Value}")]
#endif
#if !SILVERLIGHT
    [Serializable]
#endif
    public class WebParameter : WebPair
    {
        public WebParameter(string name, string value) : base(name, value)
        {

        }
    }
}