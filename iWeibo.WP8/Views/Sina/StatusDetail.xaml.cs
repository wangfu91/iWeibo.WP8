using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using sinavm = iWeibo.WP8.ViewModels.Sina;

namespace iWeibo.WP8.Views.Sina
{
    public partial class StatusDetail : PhoneApplicationPage
    {
        public StatusDetail()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var viewModel = this.DataContext as sinavm.StatusDetailViewModel;
            string id = "";
            viewModel.StatusId = this.NavigationContext.QueryString.TryGetValue("id", out id) ? id : "";
        }
    }
}