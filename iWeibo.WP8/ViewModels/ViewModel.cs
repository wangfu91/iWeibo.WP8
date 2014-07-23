using iWeibo.Adapters;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.WP8.ViewModels
{
    public abstract class ViewModel : NotificationObject,IDisposable
    {
        private readonly INavigationService navigationService;
        private readonly IPhoneApplicationServiceFacade phoneApplicationServiceFacade;
        private bool disposed;
        private readonly Uri pageUri;
        private static Uri currentPageUri;

        protected ViewModel(
            INavigationService navigationService,
            IPhoneApplicationServiceFacade phoneApplicationServiceFacade,
            Uri pageUri)
        {
            this.pageUri = pageUri;
            this.navigationService = navigationService;
            this.phoneApplicationServiceFacade = phoneApplicationServiceFacade;

            this.navigationService.Navigated += this.OnNavigationService_Navigated;
            this.navigationService.Navigating += this.OnNavigationService_Navigating;

        }



        private void OnNavigationService_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (currentPageUri == null || pageUri == null) return;
            if (currentPageUri.ToString().StartsWith(pageUri.ToString()))
            {
                OnPageDeactivation(e.IsNavigationInitiator);
            }
        }

        private void OnNavigationService_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (IsResumingFromTombstoning)
            {
                if (e.Uri.ToString().StartsWith(pageUri.ToString()))
                {
                    OnPageResumeFromTombstoning();
                    navigationService.UpdateTombstonedPageTracking(pageUri);
                }
            }

            currentPageUri = e.Uri;
        }

        ~ViewModel()
        {
            this.Dispose();
        }

        protected bool IsResumingFromTombstoning
        {
            get
            {
                return navigationService.DoesPageNeedtoRecoverFromTombstoning(pageUri);
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return this.navigationService;
            }
        }

        public IPhoneApplicationServiceFacade PhoneApplicationServiceFacade
        {
            get
            {
                return this.phoneApplicationServiceFacade;
            }
        }


        public virtual void OnPageDeactivation(bool isIntentionalNavigation)
        {

        }

        public abstract void OnPageResumeFromTombstoning();


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
            {
                navigationService.Navigated -= this.OnNavigationService_Navigated;
                navigationService.Navigating -= this.OnNavigationService_Navigating;
            }

            this.disposed = true;
        }
    }
}
