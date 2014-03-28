
namespace TencentWeiboSDK.Model
{
    public class Result:BaseModel
    {
        private int ret = 0;
        private string msg = string.Empty;

        public Result()
        { }

        /// <summary>
        /// 返回是否成功
        /// </summary>
        public int Ret
        {
            get
            {
                return ret;
            }
            set
            {
                if (value != ret)
                {
                    ret = value;
                    NotifyPropertyChanged("Ret");
                }
            }
        }
        /// <summary>
        /// 返回消息内容
        /// </summary>
        public string Msg
        {
            get
            {
                return msg;
            }
            set
            {
                if (value != msg)
                {
                    msg = value;
                    NotifyPropertyChanged("Ret");
                }
            }
        }
    }
}
