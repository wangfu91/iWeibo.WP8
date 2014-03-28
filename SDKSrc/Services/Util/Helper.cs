using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TencentWeiboSDK.Util
{
    /// <summary>
    /// OAuthHelper 类帮助 Service 解析服务器返回的结果.
    /// </summary>
    internal static class OAuthHelper
    {
        /// <summary>
        /// 将服务器返回的结果转换为参数列表.
        /// </summary>
        /// <param name="response">服务器返回的结果./param>
        /// <returns>返回参数列表.</returns>
        internal static Dictionary<string, string> GetQueryParameters(string response)
        {
            Dictionary<string, string> nameValueCollection = new Dictionary<string, string>();
            string[] items = response.Split('&');

            foreach (string item in items)
            {
                if (item.Contains("="))
                {
                    string[] nameValue = item.Split('=');
                    if (nameValue[0].Contains("?"))
                        nameValue[0] = nameValue[0].Replace("?", "");
                    nameValueCollection.Add(nameValue[0], System.Net.HttpUtility.UrlDecode(nameValue[1]));
                }
            }
            return nameValueCollection;
        }
    }
}
