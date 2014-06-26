using Funq;
using iWeibo.Adapters;
using iWeibo.WP8.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using sinavm = iWeibo.WP8.ViewModels.Sina;
using tencentvm = iWeibo.WP8.ViewModels.Tencent;

namespace iWeibo.WP8.Services
{
    public class ContainerLocator:IDisposable
    {
        private bool disposed;

        public ContainerLocator()
        {
            this.Container = new Container();
            this.ConfigureContainer();
        }

        public Container Container { get; private set; }

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
                this.Container.Dispose();

            this.disposed = true;
        }


        private void ConfigureContainer()
        {
            this.Container.Register<IPhoneApplicationServiceFacade>(c => new PhoneApplicationServiceFacade());
            this.Container.Register<INavigationService>(c => new ApplicationFrameNavigationService(App.RootFrame));
            this.Container.Register<IPhotoChooserTask>(c => new PhotoChooserTaskAdapter());
            this.Container.Register<IMessageBox>(c => new MessageBoxAdapter());

            //View Model
            this.Container.Register(
                c => new MainViewModel(
                    c.Resolve<INavigationService>(),
                    c.Resolve<IPhoneApplicationServiceFacade>()))
                    .ReusedWithin(ReuseScope.None);

            //this.Container.Register(
            //    c => new sinavm.LoginViewModel(
            //        c.Resolve<INavigationService>(),
            //        c.Resolve<IPhoneApplicationServiceFacade>(),
            //        c.Resolve<IMessageBox>()))
            //        .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new tencentvm.TimelineViewModel(
                    c.Resolve<INavigationService>(),
                    c.Resolve<IPhoneApplicationServiceFacade>(),
                    c.Resolve<IMessageBox>()))
                    .ReusedWithin(ReuseScope.Container);

            this.Container.Register(
                c => new sinavm.TimelineViewModel(
                    c.Resolve<INavigationService>(),
                    c.Resolve<IPhoneApplicationServiceFacade>(),
                    c.Resolve<IMessageBox>()))
                    .ReusedWithin(ReuseScope.Container);

            this.Container.Register(
                c => new sinavm.StatusDetailViewModel(
                    c.Resolve<INavigationService>(),
                    c.Resolve<IPhoneApplicationServiceFacade>(),
                    c.Resolve<IMessageBox>()))
                    .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new RepostViewModel(
                    c.Resolve<INavigationService>(),
                    c.Resolve<IPhoneApplicationServiceFacade>(),
                    c.Resolve<IMessageBox>()))
                    .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new CreateNewViewModel(
                    c.Resolve<INavigationService>(),
                    c.Resolve<IPhoneApplicationServiceFacade>(),
                    c.Resolve<IMessageBox>(),
                    c.Resolve<IPhotoChooserTask>()))
                    .ReusedWithin(ReuseScope.Container);

            this.Container.Register(
                c => new tencentvm.StatusDetailViewModel(
                    c.Resolve<INavigationService>(),
                    c.Resolve<IPhoneApplicationServiceFacade>(),
                    c.Resolve<IMessageBox>()))
                    .ReusedWithin(ReuseScope.None);

            //this.Container.Register(
            //    c => new PictureViewViewModel(
            //        c.Resolve<INavigationService>(),
            //        c.Resolve<IPhoneApplicationServiceFacade>()))
            //        .ReusedWithin(ReuseScope.None);

            this.Container.Register(
                c => new SettingsViewModel(
                    c.Resolve<INavigationService>(),
                    c.Resolve<IPhoneApplicationServiceFacade>(),
                    c.Resolve<IMessageBox>()))
                    .ReusedWithin(ReuseScope.None);

        }

    }
}
