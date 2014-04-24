using iWeibo.Adapters;
using iWeibo.Services;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WeiboSdk.Models;
using WeiboSdk.Services;

namespace iWeibo.WP8.ViewModels.Sina
{
    public class TimelineViewModel:ViewModel
    {

        private IMessageBox messageBox;

        //private long htPreviousCursor=0;
        private long htNextCursor;
        //private long mtPreviousCursor=0;
        private long mtNextCursor;
        //private long ftPreviousCursor=0;
        private long ftNextCursor;

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


        private WStatus selectedStatus;

        public WStatus SelectedStatus
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


        public ObservableCollection<WStatus> HomeTimeline { get; set; }

        public ObservableCollection<WStatus> MentionsTimeline { get; set; }

        public ObservableCollection<WStatus> FavoritesTimeline { get; set; }


        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand BackKeyPressCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand CreateNewCommand { get; set; }
        public DelegateCommand HomeTimelineCommand { get; set; }
        public DelegateCommand MentionsTimelineCommand { get; set; }
        public DelegateCommand FavoritesTimelineCommand { get; set; }
        


        public TimelineViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            :base(navigationService,phoneApplicationServiceFacade,new Uri(Constants.SinaTimelineView,UriKind.Relative))
        {
            this.messageBox = messageBox;
            this.HomeTimeline = new ObservableCollection<WStatus>();
            this.MentionsTimeline = new ObservableCollection<WStatus>();
            this.FavoritesTimeline = new ObservableCollection<WStatus>();

            this.PageLoadedCommand = new DelegateCommand(LoadDataFromCache, () => !this.IsSyncing);
            this.BackKeyPressCommand = new DelegateCommand(OnBackKeyPress, () => true);
            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);
            this.CreateNewCommand = new DelegateCommand(() => this.NavigationService.Navigate(new Uri(Constants.PostNewView, UriKind.Relative)));
            this.HomeTimelineCommand = new DelegateCommand(() => GetHomeTimeline(htNextCursor), () => !this.IsSyncing);
            this.MentionsTimelineCommand = new DelegateCommand(() => GetMentionsTimeline(mtNextCursor), () => !this.IsSyncing);
            this.FavoritesTimelineCommand = new DelegateCommand(() => GetFavoritesTimeline(ftNextCursor), () => !this.IsSyncing);

        }


        #region Methods

        private void LoadDataFromCache()
        {
            WStatusCollection collection;
            if (HomeTimeline.Count <= 0)
            {
                if (htStorage.TryLoadData<WStatusCollection>(out collection))
                {
                    //this.htPreviousCursor = collection.PreviousCursor;
                    this.htNextCursor = collection.NextCursor;
                    collection.Statuses.ForEach(a => HomeTimeline.Add(a));
                }
                else
                {
                    GetHomeTimeline();
                }
            }
            if (MentionsTimeline.Count <= 0)
            {
                if (mtStorage.TryLoadData<WStatusCollection>(out collection))
                {
                    //this.mtPreviousCursor = collection.PreviousCursor;
                    this.mtNextCursor = collection.NextCursor;
                    collection.Statuses.ForEach(a => MentionsTimeline.Add(a));
                }

            }
            WFavoriteCollection fCollection;
            if (FavoritesTimeline.Count <= 0)
            {
                if (ftStorage.TryLoadData<WFavoriteCollection>(out fCollection))
                {
                    //this.ftPreviousCursor = fCollection.PreviousCursor;
                    this.ftNextCursor = fCollection.NextCursor;
                    fCollection.Favorites.ForEach(a => FavoritesTimeline.Add(a));
                }
            }
        }

        private void HandlePivotSelectedIndexChange()
        {
            switch (SelectedPivotIndex)
            {
                case 0:
                    if (HomeTimeline.Count <= 0)
                        GetHomeTimeline();
                    break;
                case 1:
                    if (MentionsTimeline.Count <= 0)
                        GetMentionsTimeline();
                    break;
                case 2:
                    if (FavoritesTimeline.Count <= 0)
                        GetFavoritesTimeline();
                    break;
            }
        }

        private void HandleSelectedStatusChange()
        {
            if (this.SelectedStatus != null)
            {
                var id = this.SelectedStatus.Id;
                new IsoStorage(Constants.SinaSelectedStatus).SaveData(this.SelectedStatus);
                this.NavigationService.Navigate(new Uri(Constants.SinaStatusDetailView + "?id=" + id, UriKind.Relative));

                this.SelectedStatus = null;
            }
        }

        private void Refresh()
        {
            switch (SelectedPivotIndex)
            {
                case 0:
                    GetHomeTimeline();
                    break;
                case 1:
                    GetMentionsTimeline();
                    break;
                case 2:
                    GetFavoritesTimeline();
                    break;
            }
        }

        private void OnBackKeyPress()
        {
            //if (this.NavigationService.CanGoBack)
            //    this.NavigationService.GoBack();
            this.NavigationService.Navigate(new Uri(Constants.MainPageView, UriKind.Relative));
        }

        private void GetHomeTimeline(long maxId = 0)
        {
            this.IsSyncing = true;

            new Thread(() =>
            {
                timelineService.GetFriendsTimeline(requestCount, maxId, 0, callback =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (callback.Succeed)
                        {
                            if (maxId == 0)
                            {
                                HomeTimeline.Clear();
                                htStorage.SaveData(callback.Data);
                                //htPreviousCursor = callback.Data.PreviousCursor;
                            }
                            htNextCursor = callback.Data.NextCursor;
                            callback.Data.Statuses.ForEach(a => HomeTimeline.Add(a));
                        }
                        else
                        {
                            this.messageBox.Show(callback.ErrorMsg);
                        }
                        this.IsSyncing = false;
                    });
                });
            }).Start();

        }

        private void GetMentionsTimeline(long maxId = 0)
        {
            this.IsSyncing = true;
            new Thread(() =>
            {
                timelineService.GetMentionsTimeline(requestCount, maxId, 0, callback =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (callback.Succeed)
                        {
                            if (maxId == 0)
                            {
                                MentionsTimeline.Clear();
                                mtStorage.SaveData(callback.Data);
                                //mtPreviousCursor=callback.Data.PreviousCursor;
                            }
                            mtNextCursor = callback.Data.NextCursor;
                            callback.Data.Statuses.ForEach(a => MentionsTimeline.Add(a));
                        }
                        else
                        {
                            this.messageBox.Show(callback.ErrorMsg);
                        }
                        this.IsSyncing = false;
                    });
                });
            }).Start();

        }

        private void GetFavoritesTimeline(long maxId = 0)
        {
            this.IsSyncing = true;
            new Thread(() =>
            {
                timelineService.GetFavoritesTimeline(requestCount, maxId, 0, callback =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (callback.Succeed)
                        {
                            if (maxId == 0)
                            {
                                FavoritesTimeline.Clear();
                                ftStorage.SaveData(callback.Data);
                                //ftPreviousCursor=callback.Data.PreviousCursor;
                            }
                            ftNextCursor = callback.Data.NextCursor;
                            callback.Data.Favorites.ForEach(a => FavoritesTimeline.Add(a));
                        }
                        else
                        {
                            this.messageBox.Show(callback.ErrorMsg);
                        }
                        this.IsSyncing = false;
                    });
                });
            }).Start();

        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
