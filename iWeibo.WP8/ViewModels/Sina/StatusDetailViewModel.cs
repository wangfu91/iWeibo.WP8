using Coding4Fun.Toolkit.Controls;
using iWeibo.Adapters;
using iWeibo.Services;
using iWeibo.WP8.Resources;
using iWeibo.WP8.Services;
using Microsoft.Practices.Prism.Commands;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WeiboSdk.Models;
using WeiboSdk.Services;

namespace iWeibo.WP8.ViewModels.Sina
{
    public class StatusDetailViewModel : ViewModel
    {

        private IMessageBox messageBox;
        private IsoStorage storage = new IsoStorage(Constants.SinaSelectedStatus);
        private WStatusService statusService = new WStatusService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());
        private int requestCount = 20;

        //private long ctPreviousCursor = 0;
        //private int ctPage = 1;
        private int ctTotalNumber = 0;

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

        private bool isLoadingEnd;

        public bool IsLoadingEnd
        {
            get
            {
                return isLoadingEnd;
            }
            set
            {
                if (value != isLoadingEnd)
                {
                    isLoadingEnd = value;
                    RaisePropertyChanged(() => this.IsLoadingEnd);
                }
            }
        }


        private WStatus status;

        public WStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (value != status)
                {
                    status = value;
                    RaisePropertyChanged(() => this.Status);
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
                    HandleSelectedPivotIndexChange();
                }
            }
        }

        private string favoriteText = AppResources.AddToFavoriteText;

        public string FavoriteText
        {
            get
            {
                return favoriteText;
            }
            set
            {
                if (value != favoriteText)
                {
                    favoriteText = value;
                    RaisePropertyChanged(() => this.FavoriteText);
                }
            }
        }

        private string favoriteIconUri = "favor";

        public string FavoriteIconUri
        {
            get
            {
                return favoriteIconUri;
            }
            set
            {
                if (value != favoriteIconUri)
                {
                    favoriteIconUri = value;
                    RaisePropertyChanged(() => this.FavoriteIconUri);
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



        public string StatusId { get; set; }

        public bool CanDelete { get; set; }

        public ObservableCollection<WStatus> CommentsTimeline { get; set; }

        public DelegateCommand PageLoadedCommand { get; set; }

        public DelegateCommand<string> CommentsTimelineCommand { get; set; }

        public DelegateCommand RefreshCommand { get; set; }

        public DelegateCommand CommentCommand { get; set; }

        public DelegateCommand RepostCommand { get; set; }

        public DelegateCommand FavoriteCommand { get; set; }

        public DelegateCommand CopyCommand { get; set; }

        public DelegateCommand DeleteCommand { get; set; }

        public DelegateCommand<ListBox> ViewImageCommand { get; set; }



        public StatusDetailViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            : base(navigationService, phoneApplicationServiceFacade, new Uri(Constants.SinaStatusDetailView, UriKind.Relative))
        {
            this.messageBox = messageBox;
            this.CommentsTimeline = new ObservableCollection<WStatus>();

            //this.CanDelete = this.status.User.Name == new SettingStore().GetValueOrDefault(Constants.SinaUserName, string.Empty) ? true : false;

            this.PageLoadedCommand = new DelegateCommand(Loaded);

            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);

            this.CommentsTimelineCommand = new DelegateCommand<string>(p =>
            {
                if (p == "Next")
                {
                    var lastCursor = CommentsTimeline.Count > 0 ? CommentsTimeline.Last().Id : 0;
                    GetCommentsTimelineAsync(0, lastCursor);
                }
                else
                    Refresh();
            }, p => !this.IsSyncing);

            this.CommentCommand = new DelegateCommand(() =>
                this.NavigationService.Navigate(new Uri(Constants.RepostView + "?id=" + this.Status.Id + "&type=comment" + "&from=sina", UriKind.Relative)));



            this.RepostCommand = new DelegateCommand(() =>
                this.NavigationService.Navigate(new Uri(Constants.RepostView + "?id=" + this.Status.Id + "&type=repost" + "&from=sina", UriKind.Relative)));

            this.FavoriteCommand = new DelegateCommand(() =>
            {
                if (this.FavoriteText == AppResources.RemoveFromFavoriteText)
                    RemoveFromFavoriteAsync();
                else
                    AddToFavoriteAsync();
            }, () => !this.IsSyncing);

            this.DeleteCommand = new DelegateCommand(() =>
                DeleteStatusAsync(),
                () => this.CanDelete && !this.IsSyncing
            );

            this.CopyCommand = new DelegateCommand(CopyStatus);

            this.ViewImageCommand = new DelegateCommand<ListBox>(ViewImage);

        }


        private void Loaded()
        {
            WStatus s;
            if (storage.TryLoadData<WStatus>(out s))
            {
                this.Status = s;
                this.FavoriteText = s.Favorited ? AppResources.RemoveFromFavoriteText : AppResources.AddToFavoriteText;
                this.FavoriteIconUri = s.Favorited ? "unfavor" : "favor";
            }
            else
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    GetStatusAsync();
                    break;
                case 1:
                    var firstCursor = CommentsTimeline.Count > 0 ? CommentsTimeline.First().Id : 0;
                    GetCommentsTimelineAsync(firstCursor, 0);
                    break;
            }
        }

        private void HandleSelectedPivotIndexChange()
        {
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    if (this.status == null)
                        Refresh();
                    break;
                case 1:
                    if (this.CommentsTimeline.Count <= 0)
                        Refresh();
                    break;
            }
        }

        private void ViewImage(ListBox listBox)
        {
            this.PhoneApplicationServiceFacade.Save("PicUrls", listBox.ItemsSource);
            this.NavigationService.Navigate(new Uri(Constants.PictureView + "?index=" + listBox.SelectedIndex+"&from=sina", UriKind.Relative));
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



        private async void AddToFavoriteAsync()
        {
            this.IsSyncing = true;
            var source = new TaskCompletionSource<Callback<bool>>();
            statusService.AddFavorite(this.StatusId, callback => source.SetResult(callback));
            var result = await source.Task;
            if (result.Succeed)
            {
                this.FavoriteIconUri = "unfavor";
                this.FavoriteText = AppResources.RemoveFromFavoriteText;
                ShowNotification(true, msg: AppResources.FavoritedText);
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }

            this.IsSyncing = false;
        }

        private async void RemoveFromFavoriteAsync()
        {
            this.IsSyncing = true;
            var source = new TaskCompletionSource<Callback<bool>>();
            statusService.DelFavorite(this.StatusId, callback => source.SetResult(callback));
            var result = await source.Task;
            if (result.Succeed)
            {
                this.FavoriteIconUri = "favor";
                this.FavoriteText = AppResources.AddToFavoriteText;
                ShowNotification(true, msg: AppResources.UnFavoritedText);
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }
        }

        private async void GetStatusAsync()
        {
            this.IsSyncing = true;
            var source = new TaskCompletionSource<Callback<WStatus>>();
            statusService.GetStatus(this.StatusId, callback => source.SetResult(callback));
            var result = await source.Task;
            if (result.Succeed)
            {
                this.status = result.Data;
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }

            this.IsSyncing = false;
        }


        private async void GetCommentsTimelineAsync(long sinceId, long maxId)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                this.messageBox.Show(AppResources.NoNetworkText);
                return;
            }

            if (maxId == 0)
                this.IsSyncing = true;
            if (IsRefreshEnd)
                this.IsRefreshEnd = false;

            var timelineService = new TimelineService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());
            var source = new TaskCompletionSource<Callback<WCommentCollection>>();

            timelineService.GetCommentsTimleine(this.StatusId, requestCount, maxId, sinceId, callback => source.SetResult(callback));

            var result = await source.Task;
            if (result.Succeed)
            {
                ctTotalNumber = result.Data.TotalNumber;

                if (maxId == 0)
                {
                    if (result.Data.Comments.Count > 0)
                    {
                        var inOrder = from s in result.Data.Comments
                                      orderby s.CreateDateTime ascending
                                      select s;

                        foreach (var item in inOrder)
                        {
                            CommentsTimeline.Insert(0, item);
                        }
                        if (sinceId != 0)
                            ShowNotification(true, count: result.Data.Comments.Count);

                    }
                    else
                    {
                        ShowNotification(true, msg: AppResources.NoNewText);
                    }
                }
                else
                {
                    if (result.Data.Comments.Count > 1)
                    {
                        result.Data.Comments.RemoveAt(0);
                        result.Data.Comments.ForEach(a => CommentsTimeline.Add(a));
                    }
                    else
                        if (!this.IsLoadingEnd)
                            this.IsLoadingEnd = true;
                }
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }

            if (!IsRefreshEnd)
                this.IsRefreshEnd = true;
            if (IsSyncing)
                this.IsSyncing = false;
        }

        private async void DeleteStatusAsync()
        {
            this.IsSyncing = true;
            var source = new TaskCompletionSource<Callback<bool>>();
            statusService.DestroyStatus(this.StatusId, callback => source.SetResult(callback));
            var result = await source.Task;
            if (result.Succeed)
            {
                ShowNotification(true, msg: AppResources.DeletedText);
                await Task.Delay(3000);
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }
        }

        private void CopyStatus()
        {
            Clipboard.SetText(this.Status.Text);
            ShowNotification(true, msg: AppResources.CopiedText);
        }



        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
