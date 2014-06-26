using iWeibo.Adapters;
using iWeibo.Services;
using iWeibo.Utils;
using iWeibo.WP8.Models.Sina;
using iWeibo.WP8.Models.Tencent;
using Microsoft.Phone.Tasks;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentWeiboSDK.Model;
using WeiboSdk.Models;
using Windows.Storage;

namespace iWeibo.WP8.ViewModels
{
    public class SettingsViewModel:ViewModel
    {
        private IMessageBox messageBox;
        private string cachePath = @"\Temp";


        private IsoStorage tencentUserStorage = new IsoStorage(Constants.TencentUserInfo);

        private IsoStorage sinaUserStorage = new IsoStorage(Constants.SinaUserInfo);

        private string sinaUserName = "新浪微博";

        public string SinaUserName
        {
            get
            {
                return sinaUserName;
            }
            set
            {
                if (value != sinaUserName)
                {
                    sinaUserName = value;
                    RaisePropertyChanged(() => this.SinaUserName);
                }
            }
        }

        private string tencentUserName = "腾讯微博";

        public string TencentUserName
        {
            get
            {
                return tencentUserName;
            }
            set
            {
                if (value != tencentUserName)
                {
                    tencentUserName = value;
                    RaisePropertyChanged(() => this.TencentUserName);
                }
            }
        }

        private string sinaUserPicUrl = "/Assets/Icons/User.png";

        public string SinaUserPicUrl
        {
            get
            {
                return sinaUserPicUrl;
            }
            set
            {
                if (value != sinaUserPicUrl)
                {
                    sinaUserPicUrl = value;
                    RaisePropertyChanged(() => this.SinaUserPicUrl);
                }
            }
        }

        private string tencentUserPicUrl = "/Assets/Icons/User.png";

        public string TencentUserPicUrl
        {
            get
            {
                return tencentUserPicUrl;
            }
            set
            {
                if (value != tencentUserPicUrl)
                {
                    tencentUserPicUrl = value;
                    RaisePropertyChanged(() => this.TencentUserPicUrl);
                }
            }
        }

        private int selectedPivotIndex;

        public int SelectedPivotIndex
        {
            get
            {
                return selectedPivotIndex;
            }
            set
            {
                if (value != selectedPivotIndex)
                {
                    selectedPivotIndex = value;
                    RaisePropertyChanged(() => this.SelectedPivotIndex);
                }
            }
        }

        private string totalCacheSize="正在计算...";

        public string TotalCacheSize
        {
            get
            {
                return totalCacheSize;
            }
            set
            {
                if (value != totalCacheSize)
                {
                    totalCacheSize = value;
                    RaisePropertyChanged(() => this.TotalCacheSize);
                }
            }
        }


        public DelegateCommand PageLoadedCommand { get; set; }

        public DelegateCommand DeleteSinaUserCommand { get; set; }

        public DelegateCommand DeleteTencentUserCommand { get; set; }

        public DelegateCommand ClearCacheCommand { get; set; }
        
        public DelegateCommand EmailCommand { get; set; }

        public DelegateCommand RatingCommand { get; set; }

        public DelegateCommand MarketDetailCommand { get; set; }


        public SettingsViewModel(
            INavigationService navicationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            : base(navicationService, phoneApplicationServiceFacade, new Uri(Constants.SettingsView, UriKind.Relative))
        {
            this.messageBox = messageBox;

            this.PageLoadedCommand = new DelegateCommand(PageLoaded);
            this.DeleteSinaUserCommand = new DelegateCommand(DeleteSinaUser, () => SinaConfig.Validate());
            this.DeleteTencentUserCommand = new DelegateCommand(DeleteTencentUser, () => TencentConfig.Validate());
            this.ClearCacheCommand = new DelegateCommand(ClearCache);
            this.EmailCommand = new DelegateCommand(OpenEmail);
            this.RatingCommand = new DelegateCommand(Rating);
            this.MarketDetailCommand = new DelegateCommand(OpenMarket);

        }

        private void PageLoaded()
        {
            if (TencentConfig.Validate())
            {
                var tencentUser = tencentUserStorage.LoadData<User>();
                this.TencentUserName = tencentUser.Nick;
                this.TencentUserPicUrl = tencentUser.Head + @"/100";
            }

            if (SinaConfig.Validate())
            {
                var sinaUser = sinaUserStorage.LoadData<WUser>();
                this.SinaUserName = sinaUser.ScreenName;
                this.SinaUserPicUrl = sinaUser.AvatarLarge;
            }

            GetCacheSize();
        }

        private async void GetCacheSize()
        {
            long totalBytes;
            using(var isf=IsolatedStorageFile.GetUserStoreForApplication())
            {
                totalBytes = isf.AvailableFreeSpace;
            }

            var totalSize = ImageHelper.SizeSuffix(totalBytes);
            var cacheSize = await ImageHelper.GetImageCacheSizeAsync() + await ImageHelper.GetImageCacheSizeAsync(cachePath);

            this.TotalCacheSize = string.Format("{0}/{1}", ImageHelper.SizeSuffix(cacheSize), totalSize);
        }

        private void DeleteSinaUser()
        {
            sinaUserStorage.Clear();
            TokenIsoStorage.SinaTokenStorage.Clear();
            new IsoStorage(Constants.SinaHomeTime).Clear();
            new IsoStorage(Constants.SinaMentionsTimeline).Clear();
            new IsoStorage(Constants.SinaFavoritesTimeline).Clear();
            new IsoStorage(Constants.SinaSelectedStatus).Clear();

            this.SinaUserPicUrl = "/Assets/Icons/User.png";

            //NavigateToMainPage();
        }

        private void DeleteTencentUser()
        {
            tencentUserStorage.Clear();
            TokenIsoStorage.TencentTokenStorage.Clear();
            new IsoStorage(Constants.TencentHomeTimeline).Clear();
            new IsoStorage(Constants.TencentMentionsTimeline).Clear();
            new IsoStorage(Constants.TencentFavoritesTimeline).Clear();
            new IsoStorage(Constants.TencentSelectedStatus).Clear();

            this.TencentUserPicUrl = "/Assets/Icons/User.png";

            //NavigateToMainPage();
        }

        private async void ClearCache()
        {
            await ImageHelper.ClearImageCacheAsync(cachePath);
            await ImageHelper.ClearImageCacheAsync();
            GetCacheSize();
        }

        private void NavigateToMainPage()
        {
            this.NavigationService.Navigate(new Uri(Constants.MainPageView, UriKind.Relative));
        }

        private void OpenEmail()
        {
            EmailComposeTask ect = new EmailComposeTask();
            ect.Subject = "[iWeibo.WP8反馈]";
            ect.To = "coding4u@outlook.com";
            ect.Show();
        }

        private void Rating()
        {
            MarketplaceReviewTask mdr = new MarketplaceReviewTask();
            mdr.Show();
        }

        private void OpenMarket()
        {
            MarketplaceDetailTask mdt = new MarketplaceDetailTask();
            mdt.ContentType = MarketplaceContentType.Applications;
            mdt.Show();
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }

    }
}
