using System;
using TencentWeiboSDK.Hammock.Web;

namespace TencentWeiboSDK.Hammock.Retries
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public abstract class RetryResultCondition : IRetryCondition<WebQueryResult>
    {
        public virtual Predicate<WebQueryResult> RetryIf
        {
            get { return r => false; }
        }
    }
}