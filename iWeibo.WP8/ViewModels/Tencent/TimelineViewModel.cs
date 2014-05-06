using iWeibo.Adapters;
using iWeibo.Services;
using iWeibo.WP8.Models;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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


        private long ht_lastTimeStamp = 0;
        private long mt_lastTimeStamp = 0;
        private long ft_lastTimeStamp = 0;


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
                }
            }
        }

        public ObservableCollection<Status> HomeTimeline { get; set; }
        public ObservableCollection<Status> MentionsTimeline { get; set; }
        public ObservableCollection<Status> FavoritesTimeline { get; set; }

        public DelegateCommand PageLoadCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand<string> HomeTimelineCommand { get; set; }
        public DelegateCommand<string> MentionsTimelineCommand { get; set; }
        public DelegateCommand<string> FavoritesTimelineCommand { get; set; }
        public DelegateCommand BackKeyPressCommand { get; set; }
        public DelegateCommand<string> ViewPictureCommand { get; set; }

        public DelegateCommand CreateNewCommand { get; set; }

        

        #endregion

        public TimelineViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            :base(navigationService,phoneApplicationServiceFacade,new Uri(Constants.TencentTimelineView,UriKind.Relative))
        {
            this.messageBox = messageBox;
            this.statusesService = new StatusesService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());
            this.requestNumber = 20;

            this.HomeTimeline = new ObservableCollection<Status>();
            this.MentionsTimeline = new ObservableCollection<Status>();
            this.FavoritesTimeline = new ObservableCollection<Status>();

            this.PageLoadCommand = new DelegateCommand(LoadDataFromCache, () => !this.IsSyncing);
            this.RefreshCommand = new DelegateCommand(Refresh,()=>!this.IsSyncing);

            this.HomeTimelineCommand = new DelegateCommand<string>(p =>
            {
                if (p == "Next")
                    GetHomeTimelineAsync(1, ht_lastTimeStamp);
                else
                    GetHomeTimelineAsync();
            }, p => !this.IsSyncing);

            this.MentionsTimelineCommand = new DelegateCommand<string>(p =>
            {
                if (p == "Next")
                    GetMentionsTimelineAsync(1, mt_lastTimeStamp);
                else
                    GetMentionsTimelineAsync();
            }, p => !this.IsSyncing);

            this.FavoritesTimelineCommand = new DelegateCommand<string>(p =>
            {
                if (p == "Next")
                    GetFavoritesTimelineAsync(1, ft_lastTimeStamp);
                else
                    GetFavoritesTimelineAsync();
            }, p => !this.IsSyncing);

            //this.BackKeyPressCommand = new DelegateCommand(null);
            //this.ViewPictureCommand = new DelegateCommand<string>(null);

            this.CreateNewCommand = new DelegateCommand(() => this.NavigationService.Navigate(new Uri(Constants.CreateNewView, UriKind.Relative)));


        }

        private void LoadDataFromCache()
        {
            //throw new NotImplementedException();
        }

        private void Refresh()
        {
            switch(this.SelectedPivotIndex)
            {
                case 0:
                    GetHomeTimelineAsync();
                    break;
                case 1:
                    GetMentionsTimelineAsync();
                    break;
                case 2:
                    GetFavoritesTimelineAsync();
                    break;                    
            }
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


        public async void GetHomeTimelineAsync(int pageFlag = 0, long pageTime = 0)
        {
            this.IsSyncing = true;

            var result = await GetStatusesAsync(TimelineType.HomeTimeline, pageFlag, pageTime);

            if (result.Succeed)
            {
                if (pageFlag == 0)
                {
                    HomeTimeline.Clear();
                    htStorage.SaveData(result.Data);
                }
                ht_lastTimeStamp = result.Data.LastTimeStamp;
                result.Data.ForEach(a => HomeTimeline.Add(a));
            }
            else
            {
                this.messageBox.Show(result.ExceptionMsg);
            }

            this.IsSyncing = false;
        }


        private Task<Callback<StatusCollection>> GetStatusesAsync(TimelineType type,int pageFlag=0,long pageTime=0)
        {
            var source = new TaskCompletionSource<Callback<StatusCollection>>();
            switch(type)
            {
                case TimelineType.HomeTimeline:
                    this.statusesService.HomeTimeline(
                        new ServiceArgument() { Reqnum = this.requestNumber, PageFlag = pageFlag, PageTime = pageTime },
                        callback => source.TrySetResult(callback));
                    return source.Task;
                case TimelineType.MentionsTimeline:
                    this.statusesService.MentionsTimeline(
                        new ServiceArgument() { Reqnum = this.requestNumber, PageFlag = pageFlag, PageTime = pageTime },
                        callback => source.SetResult(callback));
                    return source.Task;
                case TimelineType.FavoritesTimeline:
                    this.statusesService.FavoritesTimeline(
                        new ServiceArgument() { Reqnum = this.requestNumber, PageFlag = pageFlag, PageTime = pageTime },
                        callback => source.SetResult(callback));
                    return source.Task;
                default:
                    return source.Task;
            }

        }


        public async void GetMentionsTimelineAsync(int pageFlag=0,long pageTime=0)
        {
            this.IsSyncing = true;
            var result = await GetStatusesAsync(TimelineType.MentionsTimeline, pageFlag, pageTime);
            if(result.Succeed)
            {
                if(pageFlag==0)
                {
                    HomeTimeline.Clear();
                    mtStorage.SaveData(result.Data);
                }
                mt_lastTimeStamp = result.Data.LastTimeStamp;
                result.Data.ForEach(a => MentionsTimeline.Add(a));
            }
            else
            {
                this.messageBox.Show(result.ExceptionMsg);
            }

            this.IsSyncing = false;
        }


        public async void GetFavoritesTimelineAsync(int pageFlag=0,long pageTime=0)
        {
            this.IsSyncing = true;

            var result = await GetStatusesAsync(TimelineType.FavoritesTimeline, pageFlag, pageTime);
            if (result.Succeed)
            {
                if (pageFlag == 0)
                {
                    FavoritesTimeline.Clear();
                    ftStorage.SaveData(result.Data);
                }
                ft_lastTimeStamp = result.Data.LastTimeStamp;
                result.Data.ForEach(a => FavoritesTimeline.Add(a));
            }
            else
            {
                this.messageBox.Show(result.ExceptionMsg);
            }

            this.IsSyncing = false;
        }



        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }


}
