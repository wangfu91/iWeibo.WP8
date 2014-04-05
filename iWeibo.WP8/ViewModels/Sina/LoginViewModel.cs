using iWeibo.Adapters;
using iWeibo.Services;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiboSdk;
using WeiboSdk.Models;
using WeiboSdk.Services;

namespace iWeibo.WP8.ViewModels.Sina
{
    public class LoginViewModel:ViewModel
    {

        private IMessageBox messageBox;

        private bool isSyncing;

        public bool IsSyncing
        {
            get
            {
                return isSyncing;
            }
            set
            {
                if (value != isSyncing)
                {
                    isSyncing = value;
                    RaisePropertyChanged(() => this.IsSyncing);
                }
            }
        }


        public DelegateCommand OAuthBrowserNavigatingCommand { get; set; }
        public DelegateCommand OAuthBrowserNavigatedCommand { get; set; }

        public DelegateCommand OAuthBrowserCanceledCommand { get; set; }

        public DelegateCommand<OAuthControl> OAuthVerifyCompletedCommand { get; set; }



        public LoginViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            :base(navigationService,phoneApplicationServiceFacade ,new Uri(Constants.SinaLoginView,UriKind.Relative))
        {
            this.messageBox = messageBox;

            this.OAuthBrowserNavigatingCommand = new DelegateCommand(OAuthBrowserNavigating);

            this.OAuthBrowserNavigatedCommand = new DelegateCommand(OAuthBrowserNavigated);

            this.OAuthBrowserCanceledCommand = new DelegateCommand(OAuthBrowserCanceled);

            this.OAuthVerifyCompletedCommand = new DelegateCommand<OAuthControl>(OAuthVerifyCompleted);
        }

        private void OAuthBrowserNavigating()
        {
            this.IsSyncing = true;
        }

        private void OAuthBrowserNavigated()
        {
            this.IsSyncing = false;
        }

        private void OAuthBrowserCanceled()
        {
            if(this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void OAuthVerifyCompleted(OAuthControl control)
        {
            control.OAuth2VerifyCompleted += ((isSuccess, err, response) =>
                {
                    if (err.errCode == SdkErrCode.SUCCESS)
                    {
                        if (null != response)
                        {
                            SinaAccessToken token = new SinaAccessToken
                            {
                                Token = response.accesssToken,
                                TokenSecret = response.refleshToken
                            };
                            //accessToken = token;
                            //保存Token
                            TokenIsoStorage.SinaTokenStorage.SaveData(token);
                            //GetMyInfoAndNavigate();

                            //await GetUserInfo(token);
                        } 

                    }
                    else if (err.errCode == SdkErrCode.NET_UNUSUAL)
                    {
                        this.messageBox.Show("网络异常，请检查网络后重试...");
                    }
                    else if (err.errCode == SdkErrCode.SERVER_ERR)
                    {
                        this.messageBox.Show("服务器返回错误，错误代码：" + err.specificCode);
                    }
                    else
                    {
                        this.messageBox.Show("未知错误，请稍后重试...");
                    }
                });

        }

        private void GetUserInfo(SinaAccessToken accessToken)
        {
            var userService = new WUserService(accessToken);
            userService.GetMyUserInfo(callback =>
                {
                    if (callback.Succeed)
                    {
                        new IsoStorage("").SaveData(callback.Data);
                    }
                    else
                    {
                        this.messageBox.Show(callback.ErrorMsg);
                    }
                });
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
