using System;

namespace TencentWeiboSDK.Hammock.Tasks
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class TaskState : IRetryState
    {
        #region ITaskState Members

        public int RepeatCount
        {
            get;set;
        }

        public DateTime? LastRepeat
        {
            get; set;
        }

        #endregion
    }
}
