using iWeibo.Adapters;
using iWeibo.Services;
using iWeibo.WP8.Models.SinaModels;
using iWeibo.WP8.Models.TencentModels;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentWeiboSDK.Model;

namespace iWeibo.WP8.ViewModels
{
    public class MainPageViewModel:ViewModel
    {
        public DelegateCommand EnterSinaCommand { get; set; }
        public DelegateCommand EnterTencentCommand { get; set; }
        public DelegateCommand EnterPostNewCommand { get; set; }
        public DelegateCommand EnterSettingsCommand { get; set; }

        public MainPageViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationFacade)
            : base(navigationService, phoneApplicationFacade, new Uri(Constants.MainPageView, UriKind.Relative))
        {
            EnterSinaCommand = new DelegateCommand(EnterSina);
            EnterTencentCommand = new DelegateCommand(EnterTencent);
            EnterPostNewCommand = new DelegateCommand(EnterPostNew);
        }

        private void EnterSina()
        {
            if (SinaConfig.Validate())
            {
                this.NavigationService.Navigate(new Uri(Constants.SinaTimelineView, UriKind.Relative));
            }
            else
            {
                this.NavigationService.Navigate(new Uri(Constants.SinaLoginView, UriKind.Relative));
            }
        }

        private void EnterTencent()
        {
            if (TencentConfig.Validate())
            {
                TencentWeiboSDK.OAuthConfigruation.AccessToken = TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>();
                this.NavigationService.Navigate(new Uri(Constants.TencentTimelineView, UriKind.Relative));
            }
            else
            {
                this.NavigationService.Navigate(new Uri(Constants.TencentLoginView, UriKind.Relative));
            }
        }

        private void EnterPostNew()
        {
            this.NavigationService.Navigate(new Uri(Constants.CreateNewView, UriKind.Relative));
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
