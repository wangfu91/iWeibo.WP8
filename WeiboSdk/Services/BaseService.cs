using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiboSdk.Models;

namespace WeiboSdk.Services
{
    public abstract class BaseService
    {
        public BaseService(SinaAccessToken accessToken)
        {
            this.token = accessToken.Token;
            this.netEngine = new SdkNetEngine();
        }

        private string token;

        public string Token
        {
            get { return token; }
        }

        private SdkNetEngine netEngine;

        public SdkNetEngine NetEngine
        {
            get { return netEngine; }
        }


    }
}
