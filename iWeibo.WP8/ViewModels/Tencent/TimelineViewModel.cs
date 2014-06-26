using iWeibo.Adapters;
using iWeibo.Services;
using iWeibo.WP8.Common;
using iWeibo.WP8.Models;
using iWeibo.WP8.Resources;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Services.Util;

namespace iWeibo.WP8.ViewModels.Tencent
{
    public class TimelineViewModel : ViewModel
    {
        #region Fields
        private IMessageBox messageBox;

        private StatusesService statusesService;

        private int requestNumber;

        private IsoStorage htStorage = new IsoStorage(Constants.TencentHomeTimeline);
        private IsoStorage mtStorage = new IsoStorage(Constants.TencentMentionsTimeline);
        private IsoStorage ftStorage = new IsoStorage(Constants.TencentFavoritesTimeline);

        //private long ht_firstTimeStamp = 0;
        //private long ht_lastTimeStamp = 0;
        //private long mt_firstTimeStamp = 0;
        //private long mt_lastTimeStamp = 0;
        //private long ft_firstTimeStamp = 0;
        //private long ft_lastTimeStamp = 0;


        #endregion

        #region Proprities

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
                    //HandleCommandCanExecuteChange();
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

        private Status selectedStatus;

        public Status SelectedStatus
        {
            get
            {
                return selectedStatus;
            }
            set
            {
                if (value != selectedStatus)
                {
                    selectedStatus = value;
                    RaisePropertyChanged(() => this.SelectedStatus);
                    HandleSelectedStatusChange();
                }
            }
        }

        public ObservableCollection<Status> HomeTimeline { get; set; }
        public ObservableCollection<Status> MentionsTimeline { get; set; }
        public ObservableCollection<Status> FavoritesTimeline { get; set; }

        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand<string> HomeTimelineCommand { get; set; }
        public DelegateCommand<string> MentionsTimelineCommand { get; set; }
        public DelegateCommand<string> FavoritesTimelineCommand { get; set; }
        public DelegateCommand BackKeyPressCommand { get; set; }
        public DelegateCommand<ListBox> ViewImageCommand { get; set; }

        public DelegateCommand CreateNewCommand { get; set; }


        #endregion

        public TimelineViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            : base(navigationService, phoneApplicationServiceFacade, new Uri(Constants.TencentTimelineView, UriKind.Relative))
        {

            this.messageBox = messageBox;
            this.statusesService = new StatusesService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());
            this.requestNumber = 20;

            this.HomeTimeline = new ObservableCollection<Status>();
            this.MentionsTimeline = new ObservableCollection<Status>();
            this.FavoritesTimeline = new ObservableCollection<Status>();

            this.PageLoadedCommand = new DelegateCommand(Loaded, () => !this.IsSyncing);
            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);

            this.HomeTimelineCommand = new DelegateCommand<string>(p =>
            {
                if (p == "Next")
                {
                    var lastTimeStamp = HomeTimeline.Count > 0 ? HomeTimeline.Last().TimeStamp : 0;
                    GetHomeTimelineAsync(1, lastTimeStamp);
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
                    var lastTimeStamp = MentionsTimeline.Count > 0 ? MentionsTimeline.Last().TimeStamp : 0;
                    GetMentionsTimelineAsync(1, lastTimeStamp);
                }
                else
                {
                    Refresh();
                }
            }, p => !this.IsSyncing);

            this.FavoritesTimelineCommand = new DelegateCommand<string>(p =>
            {
                if (p == "Next")
                {
                    var lastTimeStamp = FavoritesTimeline.Count > 0 ? FavoritesTimeline.Last().TimeStamp : 0;
                    GetFavoritesTimelineAsync(1, lastTimeStamp);
                }
                else
                {
                    Refresh();
                }
            }, p => !this.IsSyncing);

            this.BackKeyPressCommand = new DelegateCommand(OnBackKeyPress);
            this.ViewImageCommand = new DelegateCommand<ListBox>(ViewImage);

            this.CreateNewCommand = new DelegateCommand(() => this.NavigationService.Navigate(new Uri(Constants.CreateNewView, UriKind.Relative)));


        }

        private void ViewImage(ListBox listBox)
        {
            this.PhoneApplicationServiceFacade.Save("PicUrls", listBox.ItemsSource);
            this.NavigationService.Navigate(new Uri(Constants.PictureView + "?index=" + listBox.SelectedIndex+"&from=tencent", UriKind.Relative));
        }

        private void Loaded()
        {
            StatusCollection collection;
            if (HomeTimeline.Count <= 0)
            {
                if (htStorage.TryLoadData<StatusCollection>(out collection))
                {
                    collection.ForEach(a => HomeTimeline.Add(a));
                }
                else
                {
                    Refresh();
                }
            }
        }

        private void Refresh()
        {
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    var htFirstTimeStamp = HomeTimeline.Count > 0 ? HomeTimeline.First().TimeStamp : 0;
                    GetHomeTimelineAsync(2, htFirstTimeStamp);
                    break;
                case 1:
                    var mtFirstTimeStamp = MentionsTimeline.Count > 0 ? MentionsTimeline.First().TimeStamp : 0;
                    GetMentionsTimelineAsync(2, mtFirstTimeStamp);
                    break;
                case 2:
                    var ftFirstTimeStamp = FavoritesTimeline.Count > 0 ? FavoritesTimeline.First().TimeStamp : 0;
                    GetFavoritesTimelineAsync(2, ftFirstTimeStamp);
                    break;
            }
        }

        private void HandlePivotSelectedIndexChange()
        {
            StatusCollection collection;
            switch (SelectedPivotIndex)
            {
                case 0:
                    Loaded();
                    break;
                case 1:
                    if (MentionsTimeline.Count <= 0)
                    {
                        if (mtStorage.TryLoadData<StatusCollection>(out collection))
                        {
                            collection.ForEach(a => MentionsTimeline.Add(a));
                        }
                        else
                        {
                            Refresh();
                        }
                    }
                    break;
                case 2:
                    if (FavoritesTimeline.Count <= 0)
                    {
                        if (ftStorage.TryLoadData<StatusCollection>(out collection))
                        {
                            collection.ForEach(a => FavoritesTimeline.Add(a));
                        }
                        else
                        {
                            Refresh();
                        }
                    }
                    break;
            }
        }

        private void ChangeRefreshState()
        {
            if (!IsRefreshEnd)
                this.IsRefreshEnd = true;
            if (IsSyncing)
                this.IsSyncing = false;
        }


        //private void GetHomeTimeline(int pageFlag = 0, long pageTime = 0)
        //{
        //    this.IsSyncing = true;
        //    new Thread(() =>
        //    {
        //        statusesService.HomeTimeline(
        //            new ServiceArgument() { Reqnum = requestNumber, PageFlag = pageFlag, PageTime = pageTime },
        //            (callback) =>
        //            {
        //                Deployment.Current.Dispatcher.BeginInvoke(() =>
        //                {
        //                    if (callback.Succeed)
        //                    {
        //                        if (pageFlag == 0)
        //                        {
        //                            HomeTimeline.Clear();
        //                            //缓存
        //                            htStorage.SaveData(callback.Data);
        //                        }
        //                        ht_lastTimeStamp = callback.Data.LastTimeStamp;
        //                        callback.Data.ForEach(a => HomeTimeline.Add(a));
        //                    }
        //                    else
        //                    {
        //                        this.messageBox.Show(callback.ExceptionMsg);
        //                    }
        //                    this.IsSyncing = false;
        //                });
        //            });
        //    }).Start();
        //}


        public async void GetHomeTimelineAsync(int pageFlag, long pageTime)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                this.messageBox.Show(AppResources.NoNetworkText);
                return;
            }

            if (pageFlag == 2)
                this.IsSyncing = true;
            if (IsRefreshEnd)
                this.IsRefreshEnd = false;

            var source = new TaskCompletionSource<Callback<StatusCollection>>();

            this.statusesService.HomeTimeline(
                new ServiceArgument() { Reqnum = this.requestNumber, PageFlag = pageFlag, PageTime = pageTime },
                callback => source.SetResult(callback));


            var result = await source.Task;

            if (result.Succeed)
            {
                if (pageFlag == 2)
                {
                    if (result.Data.Count > 0)
                    {
                        if (result.Data.Count >= 20)
                        {
                            HomeTimeline.Clear();
                        }
                        for (int i = result.Data.Count - 1; i >= 0; i--)
                        {
                            HomeTimeline.Insert(0, result.Data[i]);
                        }
                        if (pageTime != 0)
                            ToastNotification.Show(true, count: result.Data.Count);

                        var collection = new StatusCollection();
                        collection.AddRange(HomeTimeline.Take(20).ToList());
                        htStorage.SaveData(collection);
                    }
                    else
                    {
                        ToastNotification.Show(true, msg: AppResources.NoNewText);
                    }
                }
                else
                {
                    if (result.Data.Count > 0)
                    {
                        result.Data.ForEach(a => HomeTimeline.Add(a));
                    }
                    else
                        if (!this.IsHTLoadingEnd)
                            this.IsHTLoadingEnd = true;
                }
            }
            else
            {
                ToastNotification.Show(false, msg: result.ErrorMsg);
            }

            ChangeRefreshState();
        }

        public async void GetMentionsTimelineAsync(int pageFlag, long pageTime)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                this.messageBox.Show(AppResources.NoNetworkText);
                return;
            }

            if (pageFlag == 2)
                this.IsSyncing = true;
            if (IsRefreshEnd)
                this.IsRefreshEnd = false;


            var source = new TaskCompletionSource<Callback<StatusCollection>>();

            this.statusesService.MentionsTimeline(
                new ServiceArgument() { Reqnum = this.requestNumber, PageFlag = pageFlag, PageTime = pageTime },
                callback => source.SetResult(callback));

            var result = await source.Task;

            if (result.Succeed)
            {
                if (pageFlag == 2)
                {
                    if (result.Data.Count > 0)
                    {
                        if (result.Data.Count >= 20)
                        {
                            MentionsTimeline.Clear();
                        }
                        for (int i = result.Data.Count - 1; i >= 0; i--)
                        {
                            MentionsTimeline.Insert(0, result.Data[i]);
                        }
                        if (pageTime != 0)
                            ToastNotification.Show(true, count: result.Data.Count);

                        var collection = new StatusCollection();
                        collection.AddRange(MentionsTimeline.Take(20).ToList());
                        mtStorage.SaveData(collection);
                    }
                    else
                    {
                        ToastNotification.Show(true, msg: AppResources.NoNewText);
                    }
                }
                else
                {
                    if (result.Data.Count > 0)
                    {
                        result.Data.ForEach(a => MentionsTimeline.Add(a));
                    }
                    else
                        if (!this.IsMTLoadingEnd)
                            this.IsMTLoadingEnd = true;
                }
            }
            else
            {
                ToastNotification.Show(false, msg: result.ErrorMsg);
            }

            ChangeRefreshState();
        }


        public async void GetFavoritesTimelineAsync(int pageFlag, long pageTime)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                this.messageBox.Show(AppResources.NoNetworkText);
                return;
            }

            if (pageFlag == 2)
                this.IsSyncing = true;
            if (IsRefreshEnd)
                this.IsRefreshEnd = false;

            var source = new TaskCompletionSource<Callback<StatusCollection>>();

            this.statusesService.FavoritesTimeline(
                new ServiceArgument() { Reqnum = this.requestNumber, PageFlag = pageFlag, PageTime = pageTime },
                callback => source.SetResult(callback));

            var result = await source.Task;

            if (result.Succeed)
            {
                if (pageFlag == 2)
                {
                    if (result.Data.Count > 0)
                    {
                        if (result.Data.Count >= 20)
                        {
                            FavoritesTimeline.Clear();
                        }
                        for (int i = result.Data.Count - 1; i >= 0; i--)
                        {
                            FavoritesTimeline.Insert(0, result.Data[i]);
                        }
                        if (pageTime != 0)
                            ToastNotification.Show(true, count: result.Data.Count);

                        var collection = new StatusCollection();
                        collection.AddRange(FavoritesTimeline.Take(20).ToList());
                        mtStorage.SaveData(collection);
                    }
                    else
                    {
                        ToastNotification.Show(true, msg: AppResources.NoNewText);
                    }
                }
                else
                {
                    if (result.Data.Count > 0)
                    {
                        result.Data.ForEach(a => FavoritesTimeline.Add(a));
                    }
                    else
                        if (!this.IsFTLoadingEnd)
                            this.IsFTLoadingEnd = true;
                }
            }
            else
            {
                ToastNotification.Show(false, msg: result.ErrorMsg);
            }

            ChangeRefreshState();
        }

        private void HandleSelectedStatusChange()
        {
            if (this.SelectedStatus != null)
            {
                var id = this.SelectedStatus.Id;
                new IsoStorage(Constants.TencentSelectedStatus).SaveData(this.SelectedStatus);
                this.NavigationService.Navigate(new Uri(Constants.TencentStatusDetailView + "?id=" + id, UriKind.Relative));

                this.SelectedStatus = null;
            }
        }

        private void OnBackKeyPress()
        {
            this.NavigationService.Navigate(new Uri(Constants.MainPageView, UriKind.Relative));
        }


        //private void HandleCommandCanExecuteChange()
        //{
        //    this.RefreshCommand.RaiseCanExecuteChanged();
        //    this.HomeTimelineCommand.RaiseCanExecuteChanged();
        //    this.MentionsTimelineCommand.RaiseCanExecuteChanged();
        //    this.FavoritesTimelineCommand.RaiseCanExecuteChanged();
        //}

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }


}
