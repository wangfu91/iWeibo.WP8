using iWeibo.Adapters;
using iWeibo.WP8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sinavm = iWeibo.WP8.ViewModels.Sina;
using tencentvm = iWeibo.WP8.ViewModels.Tencent;

namespace iWeibo.WP8.ViewModels
{
    public class ViewModelLocator : IDisposable
    {
        private readonly ContainerLocator containerLocator;
        private bool disposed;

        public ViewModelLocator()
        {
            this.containerLocator = new ContainerLocator();
        }

        public MainViewModel MainPageViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<MainViewModel>();
            }
        }

        //public Sina.LoginViewModel SinaLoginViewModel
        //{
        //    get
        //    {
        //        return this.containerLocator.Container.Resolve<Sina.LoginViewModel>();
        //    }
        //}

        public tencentvm.TimelineViewModel TencentTimelineViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<tencentvm.TimelineViewModel>();
            }
        }

        public tencentvm.StatusDetailViewModel TencentStatusDetailViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<tencentvm.StatusDetailViewModel>();
            }
        }

        public RepostViewModel RepostViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<RepostViewModel>();
            }
        }

        public sinavm.TimelineViewModel SinaTimelineViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<sinavm.TimelineViewModel>();
            }
        }

        public sinavm.StatusDetailViewModel SinaStatusDetailViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<sinavm.StatusDetailViewModel>();
            }
        }


        public CreateNewViewModel CreateNewViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<CreateNewViewModel>();
            }
        }

        //public PictureViewViewModel PictureViewViewModel
        //{
        //    get
        //    {
        //        return this.containerLocator.Container.Resolve<PictureViewViewModel>();
        //    }
        //}

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return this.containerLocator.Container.Resolve<SettingsViewModel>();
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return this.containerLocator.Container.Resolve<INavigationService>();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
                this.containerLocator.Dispose();

            this.disposed = true;
        }
    }
}
