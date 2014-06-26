using Coding4Fun.Toolkit.Controls;
using iWeibo.Adapters;
using iWeibo.Services;
using iWeibo.Utils;
using iWeibo.WP8.Resources;
using Microsoft.Practices.Prism.Commands;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Services.Util;
using WeiboSdk.Models;
using WeiboSdk.Services;

namespace iWeibo.WP8.ViewModels
{
    public class RepostViewModel : ViewModel, ITextBoxController
    {

        public event FocusEventHandler Focus;

        public event SelectEventHandler Select;

        private IMessageBox messageBox;

        private WStatusService statusService = new WStatusService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());
        
        private TService tService = new TService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());
        
        private bool isSending;

        public bool IsSending
        {
            get
            {
                return isSending;
            }
            set
            {
                if (value != isSending)
                {
                    isSending = value;
                    RaisePropertyChanged(() => this.IsSending);
                    this.SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string StatusId { get; set; }

        public bool IsSina { get; set; }

        private bool isRepost;

        public bool IsRepost
        {
            get
            {
                return isRepost;
            }
            set
            {
                if (value != isRepost)
                {
                    isRepost = value;
                    RaisePropertyChanged(() => this.IsRepost);
                }
            }
        }

        private string type;

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value != type)
                {
                    type = value;
                    RaisePropertyChanged(() => this.Type);
                }
            }
        }


        private string repostText;

        public string RepostText
        {
            get
            {
                return repostText;
            }
            set
            {
                if (value != repostText)
                {
                    repostText = value;
                    RaisePropertyChanged(() => this.RepostText);
                    HandleTextChange();
                }
            }
        }


        private int wordsCounter;

        public int WordsCounter
        {
            get
            {
                return wordsCounter;
            }
            set
            {
                if (value != wordsCounter)
                {
                    wordsCounter = value;
                    RaisePropertyChanged(() => this.WordsCounter);
                }
            }
        }

        private string wordsCounterColor;

        public string WordsCounterColor
        {
            get
            {
                return wordsCounterColor;
            }
            set
            {
                if (value != wordsCounterColor)
                {
                    wordsCounterColor = value;
                    RaisePropertyChanged(() => this.WordsCounterColor);
                }
            }
        }

        private bool hasText;

        public bool HasText
        {
            get
            {
                return hasText;
            }
            set
            {
                if (value != hasText)
                {
                    hasText = value;
                    RaisePropertyChanged(() => this.HasText);
                }
            }
        }

        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand ClearTextCommand { get; set; }
        public DelegateCommand SendCommand { get; set; }

        public DelegateCommand AddTopicCommand { get; set; }

        public RepostViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            IMessageBox messageBox)
            : base(navigationService, phoneApplicationServiceFacade, new Uri(Constants.RepostView, UriKind.Relative))
        {
            this.messageBox = messageBox;

            this.PageLoadedCommand = new DelegateCommand(() =>
            {
                this.Type = this.IsRepost ? AppResources.ForwardText : AppResources.CommentText;

                if (Focus != null)
                    Focus(this);
            });

            this.ClearTextCommand = new DelegateCommand(() => this.RepostText = "");

            this.SendCommand = new DelegateCommand(() =>
            {
                if (this.IsRepost)
                    RepostAsync();
                else
                    CommentAsync();
            }, () => !this.IsSending && !string.IsNullOrWhiteSpace(this.RepostText) && this.RepostText.Length <= 140);

            this.AddTopicCommand = new DelegateCommand(() =>
            {
                if (Select != null)
                {
                    int start = string.IsNullOrEmpty(RepostText) ? 1 : RepostText.Length + 1;
                    int length = 7;
                    RepostText += "#在此处输入话题#";
                    Select(this, start, length);
                }
            });
        }

        private void HandleTextChange()
        {
            this.HasText = string.IsNullOrEmpty(this.RepostText) ? false : true;
            this.WordsCounter = 140 - this.RepostText.Length;
            this.WordsCounterColor = this.WordsCounter < 0 ? "Red" : "White";
            this.SendCommand.RaiseCanExecuteChanged();

        }

        private async void RepostAsync()
        {
            this.IsSending = true;
            var source = new TaskCompletionSource<Callback<bool>>();
            if (IsSina)
                statusService.Repost(this.StatusId, this.RepostText, 0,
                    callback => source.SetResult(callback));
            else
                tService.Repost(
                    new ServiceArgument() { Reid = this.StatusId, Content = this.RepostText },
                    callback => source.SetResult(callback));

            var result = await source.Task;

            if (result.Succeed)
            {
                var toast = new ToastPrompt();
                toast.Message = "转发成功...";

                if (IsSina)
                    toast.ImageSource = new BitmapImage(new Uri(@"/Assets/Logos/sinalogo30.png", UriKind.Relative));
                else
                    toast.ImageSource = new BitmapImage(new Uri(@"/Assets/Logos/tencentlogo28.png", UriKind.Relative));

                toast.MillisecondsUntilHidden = 3000;
                toast.Show();
                toast.Completed += toast_Completed;
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }

            this.IsSending = false;
        }


        private async void CommentAsync()
        {
            this.IsSending = true;
            var source = new TaskCompletionSource<Callback<bool>>();
            if (IsSina)
                statusService.Comment(this.StatusId, this.RepostText, 0,
                    callback => source.SetResult(callback));
            else
                tService.Comment(
                    new ServiceArgument() { Reid = this.StatusId, Content = this.RepostText },
                    callback => source.SetResult(callback));

            var result = await source.Task;
            if (result.Succeed)
            {
                var toast = new ToastPrompt();
                toast.Message = "评论成功...";

                if (IsSina)
                    toast.ImageSource = new BitmapImage(new Uri(@"/Assets/Logos/sinalogo30.png", UriKind.Relative));
                else
                    toast.ImageSource = new BitmapImage(new Uri(@"/Assets/Logos/tencentlogo28.png", UriKind.Relative));

                toast.MillisecondsUntilHidden = 3000;
                toast.Show();
                toast.Completed += toast_Completed;
            }
            else
            {
                this.messageBox.Show(result.ErrorMsg);
            }

            this.IsSending = false;
        }

        void toast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (this.NavigationService.CanGoBack)
                this.NavigationService.GoBack();
        }





        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }
    }
}
