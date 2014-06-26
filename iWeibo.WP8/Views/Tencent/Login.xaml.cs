using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using TencentWeiboSDK;
using iWeibo.Services;
using TencentWeiboSDK.Services.Util;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services;
using iWeibo.WP8.Services;
using Shared;

namespace iWeibo.WP8.Views.Tencent
{
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.TencentOauthControl.OBrowserNavigating = new EventHandler(OAuthBrowserNavigating);
            this.TencentOauthControl.OBrowserNavigated = new EventHandler(OAuthBrowserNavigated);
            this.TencentOauthControl.OBrowserCancelled = new EventHandler(OAuthBrowserCanceled);

            Verify();
        }

        private void OAuthBrowserCanceled(object sender, EventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void OAuthBrowserNavigated(object sender, EventArgs e)
        {
            this.Indicator.IsVisible = false;
        }

        private void OAuthBrowserNavigating(object sender, EventArgs e)
        {
            this.Indicator.IsVisible = true;
        }

        private async void Verify()
        {
            Indicator.IsVisible = true;

            var source = new TaskCompletionSource<Callback<TencentAccessToken>>();


            //开始进行OAuth授权。
            TencentOauthControl.OAuthLogin(callback => source.SetResult(callback));
            var token = await source.Task;

            // 若已获得 AccessToken 则跳转到 TimelineView 页面
            // 注意： 若 OAuthConfigruation.IfSaveAccessToken 属性为 False，则需要在此处保存用户的 AccessToken(callback.Data) 以便下次使用.
            if (token.Succeed)
            {
                OAuthConfigruation.AccessToken = token.Data;
                TokenIsoStorage.TencentTokenStorage.SaveData(token.Data);
                GetOAuthedUerInfoAsync();
                this.NavigationService.Navigate(new Uri(Constants.TencentTimelineView, UriKind.Relative));
            }

        }


        //private void GetOAuthedUserInfo()
        //{
        //    this.Indicator.IsVisible = true;

        //    var source = new TaskCompletionSource<Callback<User>>();
        //    var userService = new UserService(OAuthConfigruation.AccessToken);
        //    userService.UserInfo(callback => source.SetResult(callback));
        //    source.Task.Wait();

        //    var result = source.Task.Result;
        //    if (result.Succeed)
        //    {
        //        new IsoStorage(Constants.TencentUserInfo).SaveData(result.Data);
        //        new SettingStore().AddOrUpdateValue(Constants.TencentUserName, result.Data.Name);
        //    }
        //    this.Indicator.IsVisible = false;

        //}


        //private void GetMyInfoAndNavigate()
        //{
        //    this.Indicator.IsVisible = true;
        //    new Thread(() =>
        //    {
        //        new UserService().UserInfo(callback =>
        //        {
        //            Deployment.Current.Dispatcher.BeginInvoke(() =>
        //            {
        //                this.Indicator.IsVisible = false;
        //                if (callback.Succeed)
        //                {
        //                    new IsoStorage(Constants.TencentUserInfo).SaveData(callback.Data);
        //                }
        //                else
        //                {
        //                    MessageBox.Show(callback.ExceptionMsg);
        //                }

        //                NavigationService.Navigate(new Uri(Constants.TencentTimelineView, UriKind.RelativeOrAbsolute));

        //            });

        //        });
        //    }).Start();
        //}

        private async void GetOAuthedUerInfoAsync()
        {
            var source = new TaskCompletionSource<Callback<User>>();
            var userSerice = new UserService();
            userSerice.UserInfo(callback => source.SetResult(callback));

            var result = await source.Task;
            if (result.Succeed)
            {
                new SettingStore().AddOrUpdateValue(Constants.TencentUserName, result.Data.Name);
                new IsoStorage(Constants.TencentUserInfo).SaveData(result.Data);
            }
            else
            {
                this.Dispatcher.BeginInvoke(() => MessageBox.Show(result.ErrorMsg));
            }
        }


    }
}