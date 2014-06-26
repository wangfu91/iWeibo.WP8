using iWeibo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiboSdk.Models;

namespace iWeibo.WP8.Models.Sina
{
    public class SinaConfig
    {
        private static string appKey = "2604917733";

        public static string AppKey
        {
            get { return appKey; }
        }

        private static string appSecret = "cdf9d1f2d2f56fe8d16648daa271d927";

        public static string AppSecret
        {
            get { return appSecret; }
        }

        private static string redirectUri = "http://weibo.com";

        public static string ReDirectUri
        {
            get { return redirectUri; }
        }

        public static bool Validate()
        {
            SinaAccessToken accessToken;
            if (TokenIsoStorage.SinaTokenStorage.TryLoadData<SinaAccessToken>(out accessToken))
            {
                return !string.IsNullOrWhiteSpace(accessToken.Token);
            }

            return false;
        }
    }
}
