using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Callback 类用于封装异步回调的结果.
    /// </summary>
    /// <typeparam name="T">回调数据的数据类型.</typeparam>
    public class Callback<T>
    {

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="data">回调数据实例</param>
        public Callback(T data)
            : this(true, data, string.Empty)
        {
        }


        public Callback(string errMsg)
            : this(false, default(T), errMsg)
        {
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="succeed">指示 Service 是否成功取得数据，默认为 true.</param>
        /// <param name="data">回调数据实例, 若无数据返回，该值可以为 null.</param>
        /// <param name="innerException">若有网络或数据异常发生，可以使用此参数.</param>
        public Callback(bool succeed, T data, string errMsg)
        {
            this.Succeed = succeed;
            this.Data = data;
            this.ErrorMsg = errMsg;
        }

        /// <summary>
        /// 获取或设置回调过程中发生的异常.
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 获取或设置回调数据的实例.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 获取或设置是否成功取得数据.
        /// </summary>
        /// <remarks>
        /// 默认为 true.
        /// </remarks>
        public bool Succeed { get; set; }

    }
}
