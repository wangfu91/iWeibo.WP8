using iWeibo.Adapters;
using iWeibo.Services;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WeiboSdk.Models;
using WeiboSdk.Services;
using Coding4Fun.Toolkit.Controls;
using Shared;
using System.Net.NetworkInformation;
using iWeibo.WP8.Resources;
using System.Windows.Controls;

namespace iWeibo.WP8.ViewModels.Sina
{
    public class TimelineViewModel : ViewModel
    {
        private IMessageBox messageBox;

        //private long htPreviousCursor = 0;
        //private int htPage = 1;
        private int htTotalNumber = 0;
        //private long mtPreviousCursor = 0;
        //private int mtPage = 1;
        private int mtTotalNumber = 0;
        private int ftPage = 1;
        private int ftTotalNumber = 0;

        private IsoStorage htStorage = new IsoStorage(Constants.SinaHomeTime);
        private IsoStorage mtStorage = new IsoStorage(Constants.SinaMentionsTimeline);
        private IsoStorage ftStorage = new IsoStorage(Constants.SinaFavoritesTimeline);

        private int requestCount = 20;

        private TimelineService timelineService = new TimelineService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());


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

        private bool isFullScreen;

        public bool IsFullScreen
        {
            get
            {
                return isFullScreen;
            }
            set
            {
                if (value != isFullScreen)
                {
                    isFullScreen = value;
                    RaisePropertyChanged(() => this.IsFullScreen);
                }
            }
        }

        private bool isViewingImage;

        public bool IsViewingImage
        {
            get
            {
                return isViewingImage;
            }
            set
            {
                if (value != isViewingImage)
                {
                    isViewingImage = value;
                    RaisePropertyChanged(() => this.IsViewingImage);
                }
            }
        }


        private bool isRefreshEnd;

        public bool IsRefreshEnd
        {
            get
            {
                return isRefreshEnd;
            }
            set
            {
                if (value != isRefreshEnd)
                {
                    isRefreshEnd = value;
                    RaisePropertyChanged(() => this.IsRefreshEnd);
                }
            }
        }

        private bool isHTLoadingEnd;

        public bool IsHTLoadingEnd
        {
            get
            {
                return isHTLoadingEnd;
            }
            set
            {
                if (value != isHTLoadingEnd)
                {
                    isHTLoadingEnd = value;
                    RaisePropertyChanged(() => this.IsHTLoadingEnd);
                }
            }
        }

        private bool isMTLoadingEnd;

        public bool IsMTLoadingEnd
        {
            get
            {
                return isMTLoadingEnd;
            }
            set
            {
                if (value != isMTLoadingEnd)
                {
                    isMTLoadingEnd = value;
                    RaisePropertyChanged(() => this.IsMTLoadingEnd);
                }
            }
        }

        private bool isFTLoadingEnd;

        public bool IsFTLoadingEnd
        {
            get
            {
                return isFTLoadingEnd;
            }
            set
            {
                if (value != isFTLoadingEnd)
                {
                    isFTLoadingEnd = value;
                    RaisePropertyChanged(() => this.IsFTLoadingEnd);
                }
            }
        }



        //private WStatus selectedStatus;

        //public WStatus SelectedStatus
        //{
        //    get
        //    {
        //        return selectedStatus;
        //    }
        //    set
        //    {
        //        if (value != selectedStatus)
        //        {
        //            selectedStatus = value;
        //            RaisePropertyChanged(() => this.SelectedStatus);
        //            HandleSelectedStatusChange();
        //        }
        //    }
        //}

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
                    HandlePivotSelectedIndexChange();
                }
            }
        }

        private ImageViewModel imageViewModel;

        public ImageViewModel ImageViewModel
        {
            get
            {
                return imageViewModel;
            }
            set
            {
                if (value != imageViewModel)
                {
                    imageViewModel = value;
                    RaisePropertyChanged(() => this.ImageViewModel);
                }
            }
        }

        public ObservableCollection<WStatus> HomeTimeline { get; set; }

        public ObservableCollection<WStatus> MentionsTimeline { get; set; }

        public ObservableCollection<WStatus> FavoritesTimeline { get; set; }


        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand BackKeyPressCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand CreateNewCommand { get; set; }
        public DelegateCommand<string> HomeTimelineCommand { get; set; }
        public DelegateCommand<string> MentionsTimelineCommand { get; set; }
        public DelegateCommand FavoritesTimelineCommand { get; set; }
        public DelegateCommand<ListBox> ViewImageCommand { get; set; }
        public DelegateCommand<WStatus> StatusDetailCommand { get; set; }
        

        public TimelineViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            : base(navigationService, phoneApplicationServiceFacade, new Uri(Constants.SinaTimelineView, UriKind.Relative))
        {
            this.messageBox = messageBox;
            this.HomeTimeline = new ObservableCollection<WStatus>();
            this.MentionsTimeline = new ObservableCollection<WStatus>();
            this.FavoritesTimeline = new ObservableCollection<WStatus>();

            this.PageLoadedCommand = new DelegateCommand(Loaded, () => !this.IsSyncing);
            this.BackKeyPressCommand = new DelegateCommand(OnBackKeyPress, () => true);
            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);
            this.CreateNewCommand = new DelegateCommand(() => this.NavigationService.Navigate(new Uri(Constants.CreateNewView, UriKind.Relative)));
            this.HomeTimelineCommand = new DelegateCommand<string>(p =>
                {
                    if (p == "Next")
                    {
                        var lastCursor = HomeTimeline.Count > 0 ? HomeTimeline.Last().Id : 0;
                        GetHomeTimeline(0, lastCursor);
                    }
                    else
                    {
                        Refresh();
                    }
                }, p => !this.IsSyncing);

            this.MentionsTimelineCommand = new DelegateCommand<string>(p =>
                {
                    if (p == "Next")
                    {
                        var lastCursor = MentionsTimeline.Count > 0 ? MentionsTimeline.Last().Id : 0;
                        GetMentionsTimeline(0, lastCursor);
                    }
                    else
                    {
                        Refresh();
                    }
                }, p => !this.IsSyncing);

            this.FavoritesTimelineCommand = new DelegateCommand(() => GetFavoritesTimeline(ftPage), () => !this.IsSyncing);

            this.ViewImageCommand = new DelegateCommand<ListBox>(p => ViewImage(p));

            this.StatusDetailCommand = new DelegateCommand<WStatus>(ViewStatusDetail);
        }


        #region Methods

        private void ViewImage(ListBox listBox)
        {
            if (ImageViewModel == null)
                ImageViewModel = new ImageViewModel();

            ImageViewModel.Initialize(listBox.ItemsSource, listBox.SelectedIndex, ServiceProvider.SinaWeibo);

            this.IsViewingImage = true;

            //this.PhoneApplicationServiceFacade.Save("PicUrls", listBox.ItemsSource);
            //this.NavigationService.Navigate(new Uri(Constants.PictureView + "?index=" + listBox.SelectedIndex + "&from=sina", UriKind.Relative));
        }


        private void ViewStatusDetail(WStatus status)
        {
            var id = status.Id;
            new IsoStorage(Constants.SinaSelectedStatus).SaveData(status);
            this.NavigationService.Navigate(new Uri(Constants.SinaStatusDetailView + "?id=" + id, UriKind.Relative));

        }

        private async void Loaded()
        {
            WStatusCollection collection;
            if (HomeTimeline.Count <= 0)
            {
                var source = new TaskCompletionSource<bool>();
                source.SetResult(htStorage.TryLoadData<WStatusCollection>(out collection));

                if (await source.Task)
                {
                    this.htTotalNumber = collection.TotalNumber;
                    collection.Statuses.ForEach(a => HomeTimeline.Add(a));
                }
                else
                {
                    Refresh();
                }
            }
        }

        private void HandlePivotSelectedIndexChange()
        {
            switch (SelectedPivotIndex)
            {
                case 0:
                    Loaded();
                    break;
                case 1:
                    WStatusCollection mtCollection;
                    if (MentionsTimeline.Count <= 0)
                    {
                        if (mtStorage.TryLoadData<WStatusCollection>(out mtCollection))
                        {
                            this.mtTotalNumber = mtCollection.TotalNumber;
                            mtCollection.Statuses.ForEach(a => MentionsTimeline.Add(a));
                        }
                        else
                        {
                            Refresh();
                        }
                    }

                    break;
                case 2:
                    WFavoriteCollection fCollection;
                    if (FavoritesTimeline.Count <= 0)
                    {
                        if (ftStorage.TryLoadData<WFavoriteCollection>(out fCollection))
                        {
                            this.ftTotalNumber = fCollection.TotalNumber;
                            fCollection.Favorites.ForEach(a => FavoritesTimeline.Add(a));
                        }
                        else
                        {
                            Refresh();
                        }
                    }
                    break;
            }
        }

        //private void HandleSelectedStatusChange()
        //{
        //    if (!this.IsViewingImage && this.SelectedStatus != null)
        //    {
        //        var id = this.SelectedStatus.Id;
        //        new IsoStorage(Constants.SinaSelectedStatus).SaveData(this.SelectedStatus);
        //        this.NavigationService.Navigate(new Uri(Constants.SinaStatusDetailView + "?id=" + id, UriKind.Relative));

        //        this.SelectedStatus = null;
        //    }
        //}

        private void Refresh()
        {
            switch (SelectedPivotIndex)
            {
                case 0:
                    var htFirstCursor = HomeTimeline.Count > 0 ? HomeTimeline.First().Id : 0;
                    GetHomeTimeline(htFirstCursor, 0);
                    break;
                case 1:
                    var mtFirstCursor = MentionsTimeline.Count > 0 ? MentionsTimeline.First().Id : 0;
                    GetMentionsTimeline(mtFirstCursor, 0);
                    break;
                case 2:
                    GetFavoritesTimeline();
                    break;
            }
        }

        private void OnBackKeyPress()
        {
            this.NavigationService.Navigate(new Uri(Constants.MainPageView, UriKind.Relative));
        }

        private void ShowNotification(bool succeed, int count = 0, string msg = "")
        {
            if (succeed)
            {
                var toast = new ToastPrompt()
                {
                    Message = string.IsNullOrEmpty(msg) ? string.Format(AppResources.ReceivedWeiboText, count) : msg,
                    MillisecondsUntilHidden = 3000
                };
                toast.Show();
            }
            else
            {
                this.messageBox.Show(msg);
            }
        }

        private void ChangeRefreshState()
        {
            if (!IsRefreshEnd)
                this.IsRefreshEnd = true;
            if (IsSyncing)
                this.IsSyncing = false;

        }

        private async void GetHomeTimeline(long sinceId, long maxId)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                this.messageBox.Show(AppResources.NoNetworkText);
                return;
            }

            //if (HomeTimeline.Count > 0 && HomeTimeline.Count >= htTotalNumber)
            //{
            //    ChangeRefreshState();
            //    return;
            //}


            if (maxId == 0)
                this.IsSyncing = true;
            if (IsRefreshEnd)
                this.IsRefreshEnd = false;

            var source = new TaskCompletionSource<Callback<WStatusCollection>>();
            this.timelineService.GetFriendsTimeline(
                requestCount, maxId, sinceId,
                callback => source.SetResult(callback));

            var result = await source.Task;

            if (result.Succeed)
            {
                htTotalNumber = result.Data.TotalNumber;

                if (maxId == 0)
                {
                    if (result.Data.Statuses.Count > 0)
                    {
                        //var inOrder = from s in result.Data.Statuses
                        //              orderby s.CreateDateTime ascending
                        //              select s;

                        //foreach (var item in inOrder)
                        //{
                        //    HomeTimeline.Insert(0, item);
                        //}
                        if (result.Data.Statuses.Count >= 20 && HomeTimeline.Count > 0)
                        {
                            HomeTimeline.Clear();
                        }

                        for (int i = result.Data.Statuses.Count - 1; i >= 0; i--)
                        {
                            HomeTimeline.Insert(0, result.Data.Statuses[i]);
                        }

                        if (sinceId != 0)
                            ShowNotification(true, count: result.Data.Statuses.Count);

                        var collection = new WStatusCollection()
                        {
                            Statuses = HomeTimeline.Take(20).ToList(),
                            TotalNumber = result.Data.TotalNumber
                        };
                        htStorage.SaveData(collection);

                    }
                    else
                    {
                        ShowNotification(true, msg: AppResources.NoNewText);
                    }
                }
                else
                {
                    if (result.Data.Statuses.Count > 1)
                    {
                        result.Data.Statuses.RemoveAt(0);
                        result.Data.Statuses.ForEach(a => HomeTimeline.Add(a));
                    }
                    else
                        if (!this.IsHTLoadingEnd)
                            this.IsHTLoadingEnd = true;
                }
            }
            else
            {
                ShowNotification(false, msg: result.ErrorMsg);
            }

            ChangeRefreshState();
        }

        private async void GetMentionsTimeline(long sinceId, long maxId)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                this.messageBox.Show(AppResources.NoNetworkText);
                return;
            }

            //if (MentionsTimeline.Count > 0 && MentionsTimeline.Count >= mtTotalNumber)
            //{
            //    ChangeRefreshState();
            //    return;
            //}


            if (maxId == 0)
                this.IsSyncing = true;
            if (IsRefreshEnd)
                this.IsRefreshEnd = false;

            var source = new TaskCompletionSource<Callback<WStatusCollection>>();


            this.timelineService.GetMentionsTimeline(
                requestCount, maxId, sinceId,
                callback => source.SetResult(callback));

            var result = await source.Task;

            if (result.Succeed)
            {
                mtTotalNumber = result.Data.TotalNumber;
                if (maxId == 0)
                {
                    if (result.Data.Statuses.Count > 0)
                    {
                        if (result.Data.Statuses.Count >= 20 && MentionsTimeline.Count > 0)
                        {
                            MentionsTimeline.Clear();
                        }

                        for (int i = result.Data.Statuses.Count - 1; i >= 0; i--)
                        {
                            MentionsTimeline.Insert(0, result.Data.Statuses[i]);
                        }
                        if (sinceId != 0)
                            ShowNotification(true, count: result.Data.Statuses.Count);

                        var collection = new WStatusCollection()
                        {
                            Statuses = MentionsTimeline.Take(20).ToList(),
                            TotalNumber = result.Data.TotalNumber
                        };
                        mtStorage.SaveData(collection);

                    }
                    else
                    {
                        ShowNotification(true, msg: AppResources.NoNewText);
                    }
                }
                else
                {
                    if (result.Data.Statuses.Count > 1)
                    {
                        result.Data.Statuses.RemoveAt(0);
                        result.Data.Statuses.ForEach(a => MentionsTimeline.Add(a));
                    }
                    else
                        if (!this.IsMTLoadingEnd)
                            this.IsMTLoadingEnd = true;
                }
            }
            else
            {
                ShowNotification(false, msg: result.ErrorMsg);
            }

            ChangeRefreshState();
        }

        private async void GetFavoritesTimeline(int page = 1)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                this.messageBox.Show(AppResources.NoNetworkText);
                return;
            }

            //if (FavoritesTimeline.Count > 0 && FavoritesTimeline.Count >= ftTotalNumber)
            //{
            //    ChangeRefreshState();
            //    return;
            //}

            if (page == 1)
                this.IsSyncing = true;
            if (IsRefreshEnd)
                this.IsRefreshEnd = false;

            var source = new TaskCompletionSource<Callback<WFavoriteCollection>>();
            this.timelineService.GetFavoritesTimeline(
                requestCount, page,
                callback => source.SetResult(callback));

            var result = await source.Task;

            if (result.Succeed)
            {
                ftTotalNumber = result.Data.TotalNumber;

                if (page == 1)
                {
                    if (result.Data.Favorites.Count > 0)
                    {
                        this.ftPage++;
                        FavoritesTimeline.Clear();
                        this.IsFTLoadingEnd = false;
                        for (int i = result.Data.Favorites.Count - 1; i >= 0; i--)
                        {
                            FavoritesTimeline.Insert(0, result.Data.Favorites[i]);
                        }
                        //ShowNotification(true, count: result.Data.Favorites.Count);
                        var collection = new WFavoriteCollection()
                        {
                            Favorites = FavoritesTimeline.ToList(),
                            TotalNumber = result.Data.TotalNumber
                        };
                        ftStorage.SaveData(collection);

                    }
                    else
                    {
                        ShowNotification(true, msg: AppResources.NoNewText);
                    }
                }
                else
                {
                    if (result.Data.Favorites.Count > 0)
                    {
                        this.ftPage++;
                        result.Data.Favorites.ForEach(a => FavoritesTimeline.Add(a));
                    }
                    else
                        if (!this.IsFTLoadingEnd)
                            this.IsFTLoadingEnd = true;

                }
            }
            else
            {
                ShowNotification(false, msg: result.ErrorMsg);
            }
            ChangeRefreshState();
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
