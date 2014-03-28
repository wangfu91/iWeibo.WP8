using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWeibo.Services
{
    public static class TokenIsoStorage
    {
        private static SafeIsoStorage tencentTokenIsoStorage = new SafeIsoStorage("/Tencent/TencentAccessTokenToken.dat");
        private static SafeIsoStorage sinaTokenIsoStorage = new SafeIsoStorage("Sina/SinaAccessToken.dat");

        public static SafeIsoStorage SinaTokenStorage
        {
            get
            {
                return TokenIsoStorage.sinaTokenIsoStorage;
            }
        }

        public static SafeIsoStorage TencentTokenStorage
        {
            get
            {
                return TokenIsoStorage.tencentTokenIsoStorage;
            }
        }
    }

}
