using System;
using System.Diagnostics;

namespace TencentWeiboSDK.Hammock.Web
{
#if !Smartphone
    [DebuggerDisplay("{Name}:{Value}")]
#endif
    public class WebHeader : WebPair
    {
        public WebHeader(string name, string value) : base(name, value)
        {
        }
    }
}