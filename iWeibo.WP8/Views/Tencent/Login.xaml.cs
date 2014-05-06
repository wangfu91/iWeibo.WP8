using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TencentWeiboSDK;
using iWeibo.Services;
using TencentWeiboSDK.Services;
using System.Threading.Tasks;
using TencentWeiboSDK.Services.Util;
using TencentWeiboSDK.Model;
using iWeibo.WP8.Services;
using System.Threading;

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

        private void Verify()
        {
            Indicator.IsVisible = true;
            //开始进行OAuth授权。
            TencentOauthControl.OAuthLogin((callback) =>
            {
                // 若已获得 AccessToken 则跳转到 TimelineView 页面
                // 注意： 若 OAuthConfigruation.IfSaveAccessToken 属性为 False，则需要在此处保存用户的 AccessToken(callback.Data) 以便下次使用.
                if (callback.Succeed)
                {
                    OAuthConfigruation.AccessToken = callback.Data;
                    TokenIsoStorage.TencentTokenStorage.SaveData(callback.Data);
                    //GetOAuthedUserInfo();
                    //Deployment.Current.Dispatcher.BeginInvoke(() => this.NavigationService.Navigate(new Uri(Constants.TencentTimelineView, UriKind.Relative)));

                    GetMyInfoAndNavigate();
                }
            });

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

        private void GetMyInfoAndNavigate()
        {
            this.Indicator.IsVisible = true;
            new Thread(() =>
            {
                new UserService().UserInfo(callback =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.Indicator.IsVisible = false;
                        if (callback.Succeed)
                        {
                            new IsoStorage(Constants.TencentUserInfo).SaveData(callback.Data);
                        }
                        else
                        {
                            MessageBox.Show(callback.ExceptionMsg);
                        }

                        NavigationService.Navigate(new Uri(Constants.TencentTimelineView, UriKind.RelativeOrAbsolute));

                    });

                });
            }).Start();
        }


    }
}