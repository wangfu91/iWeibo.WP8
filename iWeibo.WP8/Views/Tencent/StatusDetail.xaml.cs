using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using tencentvm = iWeibo.WP8.ViewModels.Tencent;

namespace iWeibo.WP8.Views.Tencent
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
            var viewModel = this.DataContext as tencentvm.StatusDetailViewModel;
            viewModel.StatusId = this.NavigationContext.QueryString.ContainsKey("id") ? this.NavigationContext.QueryString["id"] : "";
        }

    }
}