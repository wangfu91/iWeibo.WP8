using Coding4Fun.Toolkit.Controls;
using iWeibo.Adapters;
using iWeibo.Services;
using iWeibo.WP8.Common;
using iWeibo.WP8.Resources;
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
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Services.Util;

namespace iWeibo.WP8.ViewModels.Tencent
{
    public class StatusDetailViewModel : ViewModel
    {

        private IMessageBox messageBox;
        private IsoStorage storage = new IsoStorage(Constants.TencentSelectedStatus);
        private int requestNumber = 20;
        private TService tService = new TService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());

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

        private Status status;

        public Status Status
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

        public string StatusId { get; set; }

        public bool CanDelete { get; set; }

        public ObservableCollection<Status> CommentsTimeline { get; set; }

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
            : base(navigationService, phoneApplicationServiceFacade, new Uri(Constants.TencentStatusDetailView, UriKind.Relative))
        {
            this.messageBox = messageBox;
            this.CommentsTimeline = new ObservableCollection<Status>();
            this.RefreshCommand = new DelegateCommand(Refresh, () => !this.IsSyncing);
            this.PageLoadedCommand = new DelegateCommand(Loaded);

            this.CommentsTimelineCommand = new DelegateCommand<string>(p =>
            {
                if (p == "Next")
                {
                    var lastTimeStamp = CommentsTimeline.Count > 0 ? CommentsTimeline.Last().TimeStamp : 0;
                    GetCommentsTimelineAsync(1, lastTimeStamp);
                }
                else
                    Refresh();
            }, p => !this.IsSyncing);
            this.CommentCommand = new DelegateCommand(() =>
            {
                this.NavigationService.Navigate(new Uri(Constants.RepostView + "?id=" + this.Status.Id + "&type=comment"+"&from=tencent", UriKind.Relative));
            });

            this.RepostCommand = new DelegateCommand(() =>
            {
                this.NavigationService.Navigate(new Uri(Constants.RepostView + "?id=" + this.Status.Id + "&type=repost"+"&from=tencent", UriKind.Relative));
            });

            this.CopyCommand = new DelegateCommand(CopyStatus);
            this.DeleteCommand = new DelegateCommand(DeleteStatusAsync, () => this.CanDelete && !this.IsSyncing);
            this.FavoriteCommand = new DelegateCommand(() =>
            {
                if (this.FavoriteText == AppResources.RemoveFromFavoriteText)
                    RemoveFavoriteAsync();
                else
                    AddFavoriteAsync();
            }, () => !this.IsSyncing);

            this.ViewImageCommand = new DelegateCommand<ListBox>(ViewImage);

        }

        private void Loaded()
        {
            Status s;
            if (storage.TryLoadData<Status>(out s))
            {
                this.Status = s;
                this.FavoriteText = s.IsFavorite ? AppResources.RemoveFromFavoriteText : AppResources.AddToFavoriteText;
                this.FavoriteIconUri = s.IsFavorite ? "unfavor" : "favor";
                this.CanDelete = s.IsSelf;
                this.DeleteCommand.RaiseCanExecuteChanged();
            }
            else
                GetStatusAsync();
        }

        private void Refresh()
        {
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    GetStatusAsync();
                    break;
                case 1:
                    var firstTimeStamp = CommentsTimeline.Count > 0 ? CommentsTimeline.First().TimeStamp : 0;
                    GetCommentsTimelineAsync(2,firstTimeStamp);
                    break;
            }
        }

        private void HandleSelectedPivotIndexChange()
        {
            switch (this.SelectedPivotIndex)
            {
                case 0:
                    if (this.Status == null)
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
            this.NavigationService.Navigate(new Uri(Constants.PictureView + "?index=" + listBox.SelectedIndex+"&from=tencent", UriKind.Relative));
        }

        private void ChangeRefreshState()
        {
            if (!IsRefreshEnd)
                this.IsRefreshEnd = true;
            if (IsSyncing)
                this.IsSyncing = false;
        }

        private async void GetStatusAsync()
        {
            this.IsSyncing = true;
            var source = new TaskCompletionSource<Callback<Status>>();
            tService.Show(new ServiceArgument() { Id = this.StatusId }, callback => source.SetResult(callback));
            var result = await source.Task;
            if (result.Succeed)
            {
                this.Status = result.Data;
                this.CanDelete = result.Data.IsSelf;
                this.DeleteCommand.RaiseCanExecuteChanged();
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }
            this.IsSyncing = false;
        }



        private async void GetCommentsTimelineAsync(int pageFlag , long pageTime )
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

            var statusService = new StatusesService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());
            var source = new TaskCompletionSource<Callback<StatusCollection>>();

            statusService.SingleCommentsTimeline(
                new ServiceArgument() { Rootid = this.StatusId, Flag = 2, PageFlag = pageFlag, PageTime = pageTime, Reqnum = requestNumber },
                callback => source.SetResult(callback));

            var result = await source.Task;
            if (result.Succeed)
            {
                if (pageFlag == 2)
                {
                    if (result.Data.Count > 0)
                    {

                        for (int i = result.Data.Count - 1; i >= 0; i--)
                        {
                            CommentsTimeline.Insert(0, result.Data[i]);
                        }
                        if (pageTime != 0)
                            ToastNotification.Show(true, count: result.Data.Count);
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
                        result.Data.ForEach(a => CommentsTimeline.Add(a));
                    }
                    else
                        if (!this.IsLoadingEnd)
                            this.IsLoadingEnd = true;
                }
            }
            else
            {
                ToastNotification.Show(false, msg: result.ErrorMsg);
            }

            ChangeRefreshState();
        }


        private async void AddFavoriteAsync()
        {
            this.IsSyncing = true;
            var source = new TaskCompletionSource<Callback<bool>>();

            tService.AddFavorite(
                new ServiceArgument() { Id = StatusId },
                callback => source.SetResult(callback));
            var result = await source.Task;
            if (result.Succeed)
            {
                this.FavoriteIconUri = "unfavor";
                this.FavoriteText = AppResources.RemoveFromFavoriteText;
                var toast = new ToastPrompt()
                {
                    Message = AppResources.FavoritedText,
                    MillisecondsUntilHidden = 3000
                };
                toast.Show();
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }

            this.IsSyncing = false;
        }

        private async void RemoveFavoriteAsync()
        {
            this.IsSyncing = true;
            var source = new TaskCompletionSource<Callback<bool>>();
            tService.DelFavorite(
                new ServiceArgument() { Id = StatusId },
                callback => source.SetResult(callback));

            var result = await source.Task;
            if (result.Succeed)
            {
                this.FavoriteIconUri = "favor";
                this.FavoriteText = AppResources.AddToFavoriteText;
                var toast = new ToastPrompt()
                {
                    Message = AppResources.UnFavoritedText,
                    MillisecondsUntilHidden = 3000
                };
                toast.Show();
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }
            this.IsSyncing = false;

        }

        private void CopyStatus()
        {
            Clipboard.SetText(this.Status.Text);
            var toast = new ToastPrompt()
            {
                Message = AppResources.CopiedText,
                MillisecondsUntilHidden = 3000
            };
            toast.Show();
        }

        private async void DeleteStatusAsync()
        {
            this.IsSyncing = true;
            var source = new TaskCompletionSource<Callback<bool>>();
            tService.Delete(
                new ServiceArgument() { Id = StatusId },
                callback => source.SetResult(callback));

            var result = await source.Task;
            if (result.Succeed)
            {
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }
            this.IsSyncing = false;
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
