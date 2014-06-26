using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiboSdk.Services
{
    public static class ErrCodeToMsg
    {
        public static string GetMsg(SdkErrCode errCode)
        {
            switch (errCode)
            {
                case SdkErrCode.NET_UNUSUAL:
                    return "网络不可用，请检查网络设置...";
                case SdkErrCode.SERVER_ERR:
                    return "服务器返回异常，请稍后重试...";
                case SdkErrCode.TIMEOUT:
                    return "请求超时，请稍后重试...";
                case SdkErrCode.USER_CANCEL:
                    return "用户请求被取消...";
                case SdkErrCode.XPARAM_ERR:
                    return "参数错误...";
                default:
                    return "未知错误...";
            }
        }
    }
}
