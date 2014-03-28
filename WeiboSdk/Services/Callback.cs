using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiboSdk.Services
{
    public class Callback<T>
    {

        public Callback(string errMsg)
            : this(false, default(T), errMsg)
        {
        }

        public Callback(T data)
            : this(true, data, "")
        {
        }

        public Callback(bool succeed, T data, string errorMsg)
        {
            this.Succeed = succeed;
            this.Data = data;
            this.ErrorMsg = errorMsg;
        }


        public bool Succeed { get; set; }

        public T Data { get; set; }


        public string ErrorMsg { get; set; }

    }
}
