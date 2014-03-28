using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WeiboSdk.Models;

namespace WeiboSdk.Services
{
    public class WUserService:BaseService
    {

        public WUserService(SinaAccessToken accessToken)
            :base(accessToken)
        {

        }


        private void GetMyUid(Action<string> action)
        {
            SdkCmdBase cmdBase = new SdkCmdBase
            {
                acessToken = this.Token
            };

            this.NetEngine.RequestCmd(SdkRequestType.GET_UID, cmdBase, (requestType, response) =>
                {
                    if (response.errCode == SdkErrCode.SUCCESS)
                    {
                        if (action != null)
                        {
                            var j = JObject.Parse(response.content)["uid"];
                            action(j.ToString());
                        }
                    }
                });
        }


        public void GetMyUserInfo(Action<Callback<WUser>> action)
        {
            GetMyUid(uid=>
                {
                    SdkCmdBase cmdBase = new CmdUserInfo
                    {
                        acessToken = this.Token,
                        uid = uid
                    };

                    this.NetEngine.RequestCmd(SdkRequestType.USERS_SHOW, cmdBase, (requestType, response) =>
                        {
                            if (action != null)
                            {
                                if (response.errCode == SdkErrCode.SUCCESS)
                                {
                                    WUser user;
                                    user = JsonConvert.DeserializeObject<WUser>(response.content);

                                    action(new Callback<WUser>(user));
                                }
                                else
                                {
                                    action(new Callback<WUser>(ErrCodeToMsg.GetMsg(response.errCode)));
                                }
                            }
                        });
                });
        }

        public void GetUserInfo(Action<Callback<WUser>> action,string name)
        {
            SdkCmdBase cmdBase = new CmdUserInfo
            {
                acessToken = this.Token,
                screen_name = name
            };

            this.NetEngine.RequestCmd(SdkRequestType.USERS_SHOW, cmdBase, (requestType, response) =>
                {
                    if (action != null)
                    {
                        if (response.errCode == SdkErrCode.SUCCESS)
                        {
                            WUser user = null;
                            user = JsonConvert.DeserializeObject<WUser>(response.content);

                            action(new Callback<WUser>(user));
                        }
                        else
                        {
                            action(new Callback<WUser>(ErrCodeToMsg.GetMsg(response.errCode)));
                        }
                    }
                });
        }


    }
}
