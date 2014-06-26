using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iWeibo.WP8.ViewModels;

namespace iWeibo.WP8.Views
{
    public partial class RepostView : PhoneApplicationPage
    {
        public RepostView()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as RepostViewModel;

            string statusId = "";
            this.NavigationContext.QueryString.TryGetValue("id", out statusId);
            viewModel.StatusId = statusId;

            string type = "";
            this.NavigationContext.QueryString.TryGetValue("type", out type);
            viewModel.IsRepost = type == "repost" ? true : false;

            string from = "";
            this.NavigationContext.QueryString.TryGetValue("from", out from);
            viewModel.IsSina = from == "sina" ? true : false;

            base.OnNavigatedTo(e);
        }
    }
}