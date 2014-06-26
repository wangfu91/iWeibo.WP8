using Coding4Fun.Toolkit.Controls;
using iWeibo.Adapters;
using iWeibo.Services;
using iWeibo.Utils;
using iWeibo.WP8.Models.Sina;
using iWeibo.WP8.Models.Tencent;
using Microsoft.Phone;
using Microsoft.Phone.Reactive;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TencentWeiboSDK.Model;
using TencentWeiboSDK.Services;
using TencentWeiboSDK.Services.Util;
using WeiboSdk;
using WeiboSdk.Models;
using WeiboSdk.Services;
using Windows.Storage;
using Shared;
using System.IO.IsolatedStorage;

namespace iWeibo.WP8.ViewModels
{
    public class CreateNewViewModel : ViewModel, ITextBoxController
    {
        private IPhotoChooserTask photoChoosertask;
        private IMessageBox messageBox;

        public event FocusEventHandler Focus;
        public event SelectEventHandler Select;

        private bool isTencentSent = false;
        private bool isSinaSent = false;
        private bool isNavigated = false;


        private bool isSendTencent;

        public bool IsSendTencent
        {
            get
            {
                return isSendTencent;
            }
            set
            {
                if (value != isSendTencent)
                {
                    isSendTencent = value;
                    RaisePropertyChanged(() => this.IsSendTencent);
                    SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool isSendSina;

        public bool IsSendSina
        {
            get
            {
                return isSendSina;
            }
            set
            {
                if (value != isSendSina)
                {
                    isSendSina = value;
                    RaisePropertyChanged(() => this.IsSendSina);
                    SendCommand.RaiseCanExecuteChanged();
                }
            }
        }


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
                    SendCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private string statusText;

        public string StatusText
        {
            get
            {
                return statusText;
            }
            set
            {
                if (value != statusText)
                {
                    statusText = value;
                    RaisePropertyChanged(() => this.StatusText);
                    HandleTextChange();
                }
            }
        }

        private int wordsCounter = 140;

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


        private bool isTencentAuthorized;

        public bool IsTencentAuthorized
        {
            get
            {
                return isTencentAuthorized;
            }
            set
            {
                if (value != isTencentAuthorized)
                {
                    isTencentAuthorized = value;
                    RaisePropertyChanged(() => this.IsTencentAuthorized);
                }
            }
        }

        private bool isSinaAuthorized;

        public bool IsSinaAuthorized
        {
            get
            {
                return isSinaAuthorized;
            }
            set
            {
                if (value != isSinaAuthorized)
                {
                    isSinaAuthorized = value;
                    RaisePropertyChanged(() => this.IsSinaAuthorized);
                }
            }
        }

        private bool hasPic = false;

        public bool HasPic
        {
            get
            {
                return hasPic;
            }
            set
            {
                if (value != hasPic)
                {
                    hasPic = value;
                    RaisePropertyChanged(() => this.HasPic);
                }
            }
        }

        private bool choosingPhoto;

        public bool ChoosingPhoto
        {
            get
            {
                return choosingPhoto;
            }
            set
            {
                if (value != choosingPhoto)
                {
                    choosingPhoto = value;
                    RaisePropertyChanged(() => this.ChoosingPhoto);
                }
            }
        }


        private WriteableBitmap picture;

        public WriteableBitmap Picture
        {
            get
            {
                return picture;
            }
            set
            {
                if (value != picture)
                {
                    picture = value;
                    RaisePropertyChanged(() => this.Picture);
                }
            }
        }

        private BitmapImage bmp;

        public BitmapImage BMP
        {
            get
            {
                return bmp;
            }
            set
            {
                if (value != bmp)
                {
                    bmp = value;
                    RaisePropertyChanged(() => this.BMP);
                }
            }
        }


        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand ClearTextCommand { get; set; }
        public DelegateCommand ClearPicCommand { get; set; }
        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand ChoosePhotoCommand { get; set; }
        public DelegateCommand AddTopicCommand { get; set; }
        public DelegateCommand BackKeyPressCommand { get; set; }

        public CreateNewViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacede,
            IMessageBox messageBox,
            IPhotoChooserTask photoChooserTask)
            : base(navigationService, phoneApplicationServiceFacede, new Uri(Constants.CreateNewView, UriKind.Relative))
        {
            this.messageBox = messageBox;
            this.photoChoosertask = photoChooserTask;

            // Subscribe to handle new photo stream
            Observable.FromEvent<SettablePhotoResult>(h => this.photoChoosertask.Completed += h, h => this.photoChoosertask.Completed -= h)
                .Where(e => e.EventArgs.ChosenPhoto != null)
                .Subscribe(result =>
                {
                    this.ChoosingPhoto = false;
                    SetImageSource(result.EventArgs.ChosenPhoto);
                    HasPic = true;
                    this.ChoosePhotoCommand.RaiseCanExecuteChanged();
                });

            // Subscribe to user cancelling photo capture to re-enable Capture command
            Observable.FromEvent<SettablePhotoResult>(h => this.photoChoosertask.Completed += h, h => this.photoChoosertask.Completed -= h)
                .Where(e => e.EventArgs.ChosenPhoto == null && e.EventArgs.Error == null)
                .Subscribe(p =>
                {
                    this.ChoosingPhoto = false;
                    this.ChoosePhotoCommand.RaiseCanExecuteChanged();
                });

            // Subscribe to Error condition
            Observable.FromEvent<SettablePhotoResult>(h => this.photoChoosertask.Completed += h, h => this.photoChoosertask.Completed -= h)
                .Where(e => e.EventArgs.Error != null && !string.IsNullOrEmpty(e.EventArgs.Error.Message))
                .Subscribe(p =>
                {
                    this.ChoosingPhoto = false;
                    MessageBox.Show(p.EventArgs.Error.Message);
                    this.ChoosePhotoCommand.RaiseCanExecuteChanged();
                });

            this.PageLoadedCommand = new DelegateCommand(() =>
            {
                if (Focus != null)
                {
                    Focus(this);
                }
                this.IsTencentAuthorized = TencentConfig.Validate();
                this.IsSinaAuthorized = SinaConfig.Validate();
                this.IsSendSina = this.IsSinaAuthorized;
                this.IsSendTencent = this.IsTencentAuthorized;
                //OnPageResumeFromTombstoning();
            });
            this.ClearTextCommand = new DelegateCommand(() => StatusText = "");
            this.ClearPicCommand = new DelegateCommand(() =>
            {
                HasPic = false;
                Picture = null;
                BMP = null;
            });
            this.SendCommand = new DelegateCommand(Send, () => !this.IsSending && this.HasText && !(StatusText.Length > 140) && (IsSendTencent || IsSendSina));

            this.ChoosePhotoCommand = new DelegateCommand(this.ChoosePhoto, () => !this.ChoosingPhoto);
            this.AddTopicCommand = new DelegateCommand(() =>
            {
                if (this.Select != null)
                {
                    int start = string.IsNullOrEmpty(StatusText) ? 1 : StatusText.Length + 1;
                    int length = 7;
                    StatusText += "#在此处输入话题#";
                    Select(this, start, length);
                }
            });

            this.BackKeyPressCommand = new DelegateCommand(GoBack);

        }

        private void Send()
        {
            if (IsSendSina)
                SendSinaAsync();
            if (IsSendTencent)
                SendTencentAsync();

        }

        private void HandleTextChange()
        {
            this.HasText = string.IsNullOrWhiteSpace(this.StatusText) ? false : true;
            this.WordsCounter = 140 - this.StatusText.Length;

            this.WordsCounterColor = this.WordsCounter < 0 ? "Red" : "White";

            this.SendCommand.RaiseCanExecuteChanged();

        }

        private void ChoosePhoto()
        {
            if (!this.ChoosingPhoto)
            {
                this.photoChoosertask.Show();
                this.ChoosingPhoto = true;
                this.ChoosePhotoCommand.RaiseCanExecuteChanged();
            }
        }

        private void SetImageSource(Stream chosenPhoto)
        {
            this.BMP = new BitmapImage();
            this.BMP.SetSource(chosenPhoto);

            byte[] imageBytes = new byte[chosenPhoto.Length];
            chosenPhoto.Read(imageBytes, 0, imageBytes.Length);

            // Seek back so we can create an image.
            chosenPhoto.Seek(0, SeekOrigin.Begin);

            // Create an image from the stream.
            //var imageSource = PictureDecoder.DecodeJpeg(chosenPhoto);

            var imageSource = new WriteableBitmap(this.BMP);

            this.Picture = imageSource;
        }

        private async void SendTencentAsync()
        {
            this.IsSending = true;
            var pic = (BMP == null) ? null : new UploadPictureHelper(BMP,"tencent");
            var tencentToast = new ToastPrompt();
            tencentToast.MillisecondsUntilHidden = 2000;
            tencentToast.ImageSource = new BitmapImage(new Uri(@"/Assets/Logos/tencentlogo28.png", UriKind.Relative));
            tencentToast.Completed += tencentToast_Completed;
            tencentToast.Title = "发送成功...";

            var source = new TaskCompletionSource<Callback<bool>>();

            TService tService = new TService(TokenIsoStorage.TencentTokenStorage.LoadData<TencentAccessToken>());
            if (null != pic)
            {
                tService.AddPic(
                    new ServiceArgument() { Content = StatusText, Pic = pic },
                    (callback) => source.SetResult(callback));
            }
            else
            {
                tService.Add(
                    new ServiceArgument() { Content = StatusText },
                    (callback) => source.SetResult(callback));
            }

            var result = await source.Task;

            if (result.Succeed)
            {
                this.isTencentSent = true;
                tencentToast.Show();
            }
            else
            {
                MessageBox.Show("发送失败，请稍后重试..." + result.ErrorMsg, "腾讯微博", MessageBoxButton.OK);
            }
            this.IsSending = false;
        }


        void tencentToast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (isTencentSent && !isNavigated)
            {
                if (!IsSendSina || isSinaSent)
                {
                    CleanUp();
                    GoBack();
                }
            }
        }

        private async void SendSinaAsync()
        {
            this.IsSending = true;
            var pic = (BMP == null) ? null : new UploadPictureHelper(BMP,"sina");
            var sinaToast = new ToastPrompt();
            sinaToast.MillisecondsUntilHidden = 2000;
            sinaToast.ImageSource = new BitmapImage(new Uri(@"/Assets/Logos/sinalogo30.png", UriKind.Relative));
            sinaToast.Completed += sinaToast_Completed;
            sinaToast.Title = "发送成功...";

            var source = new TaskCompletionSource<Callback<bool>>();

            var wService = new WStatusService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());
            if (pic != null)
            {
                wService.AddPic(StatusText, pic, callback => source.SetResult(callback));
            }
            else
            {
                wService.Add(StatusText, callback => source.SetResult(callback));
            }

            var result = await source.Task;

            if (result.Succeed)
            {
                this.isSinaSent = true;
                sinaToast.Show();
            }
            else
            {
                this.messageBox.Show("发送失败，" + result.ErrorMsg, "新浪微博", MessageBoxButton.OK);
            }
            this.IsSending = false;
        }

        private void sinaToast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (isSinaSent && !isNavigated)
            {
                if (!IsSendTencent || isTencentSent)
                {
                    CleanUp();
                    GoBack();
                }
            }
        }

        private async void GoBack()
        {
            //CleanUp();

            await Task.Delay(1000);

            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
                this.isNavigated = true;
            }
        }
        private void CleanUp()
        {
            this.StatusText = string.Empty;
            this.BMP = null;
            this.Picture = null;
        }

        public override void OnPageResumeFromTombstoning()
        {
            //throw new NotImplementedException();
        }

        public override void OnPageDeactivation(bool isIntentionalNavigation)
        {
            base.OnPageDeactivation(isIntentionalNavigation);
        }

    }
}
