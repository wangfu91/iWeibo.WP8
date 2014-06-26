using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WeiboSdk.Models;
using WeiboSdk;
using iWeibo.Services;
using System.Threading.Tasks;
using WeiboSdk.Services;
using iWeibo.WP8.Services;
using Shared;

namespace iWeibo.WP8.Views.Sina
{
    public partial class Login : PhoneApplicationPage
    {
        private SinaAccessToken accessToken;

        public Login()
        {
            InitializeComponent();
            this.OAuthControl.OBrowserNavigating = new EventHandler(OAuthBrowserNavigating);
            this.OAuthControl.OBrowserNavigated = new EventHandler(OAuthBrowserNavigated);
            this.OAuthControl.OBrowserCancelled = new EventHandler(CancleEvent);
            this.OAuthControl.OAuth2VerifyCompleted += new OAuth2LoginBack(OAuth2CallBack);

        }


        private void OAuth2CallBack(bool isSucess, SdkAuthError err, SdkAuth2Res response)
        {
            VerifyBack(isSucess, err, response);
        }

        private void VerifyBack(bool isSuccess, SdkAuthError err, SdkAuth2Res response)
        {
            switch (err.errCode)
            {
                case SdkErrCode.SUCCESS:
                    if (null != response)
                    {
                        SinaAccessToken token = new SinaAccessToken
                        {
                            Token = response.accesssToken,
                            TokenSecret = response.refleshToken
                        };

                        accessToken = token;
                        //save Token
                        TokenIsoStorage.SinaTokenStorage.SaveData(token);

                        GetOAuthedUserInfo();

                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.NavigationService.Navigate(new Uri(Constants.SinaTimelineView, UriKind.Relative));
                        });
                    }
                    break;
                case SdkErrCode.NET_UNUSUAL:
                    MessageBox.Show("网络异常:" + err.specificCode + "，请检查网络后重试...");
                    break;
                case SdkErrCode.SERVER_ERR:
                    MessageBox.Show("服务器返回错误:" + err.specificCode);
                    break;
                case SdkErrCode.TIMEOUT:
                    MessageBox.Show("请求超时:" + err.specificCode);
                    break;
                case SdkErrCode.USER_CANCEL:
                    break;
                case SdkErrCode.XPARAM_ERR:
                    MessageBox.Show("参数错误:" + err.specificCode);
                    break;
            }
        }


        private void CancleEvent(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            });
        }

        private void OAuthBrowserNavigated(object sender, EventArgs e)
        {
            Indicator.IsVisible = false;
        }

        private void OAuthBrowserNavigating(object sender, EventArgs e)
        {
            Indicator.IsVisible = true;
        }

        private async void GetOAuthedUserInfo()
        {
            this.Dispatcher.BeginInvoke(() => this.Indicator.IsVisible = true);
            this.Dispatcher.BeginInvoke(() => this.Indicator.Text = "Retriving user info...");
            var source = new TaskCompletionSource<Callback<WUser>>();
            WUserService userService = new WUserService(accessToken);
            userService.GetMyUserInfo(callback => source.SetResult(callback));

            var result = await source.Task;
            if (result.Succeed)
            {
                if (result.Data != null)
                {
                    new SettingStore().AddOrUpdateValue(Constants.SinaUserName, result.Data.Name);
                    new IsoStorage(Constants.SinaUserInfo).SaveData(result.Data);
                }
            }

            this.Dispatcher.BeginInvoke(() => this.Indicator.IsVisible = false);
        }

    }
}