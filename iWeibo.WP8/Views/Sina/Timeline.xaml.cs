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
using System.Diagnostics;
namespace iWeibo.WP8.Views.Sina
{
    public partial class Timeline : PhoneApplicationPage
    {
        private sinavm.TimelineViewModel viewModel;

        public Timeline()
        {
            InitializeComponent();

            viewModel = this.DataContext as sinavm.TimelineViewModel;
        }

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            //Debug.WriteLine(string.Format("e.VerticalVelocity={0}", e.VerticalVelocity));

            if (e.VerticalVelocity < 0)
            {
                GoFullScreen();
            }
            //else if (e.VerticalVelocity > 0)
            //{
            //    GoNormal();
            //}
        }

        private void GoFullScreen()
        {
            if (viewModel.IsFullScreen) return;

            this.TimelinePivot.Margin = new Thickness(0, 0, 0, -158);

            this.TimelinePivot.IsLocked = true;

            //SystemTray.IsVisible = false;
            //this.TimelineAppBar.IsVisible = false;

            PivotTitleDispear.Begin();

            viewModel.IsFullScreen = true;

            this.BackKeyPress += Timeline_BackKeyPress;
        }

        private void GoNormal()
        {
            if (!viewModel.IsFullScreen) return;

            this.TimelinePivot.Margin = new Thickness(0, 0, 0, 0);
            this.TimelinePivot.IsLocked = false;

            //SystemTray.IsVisible = true;
            //this.TimelineAppBar.IsVisible = true;

            PivotTitleAppear.Begin();

            viewModel.IsFullScreen = false;

            this.BackKeyPress -= Timeline_BackKeyPress;
        }

        void Timeline_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (viewModel.IsFullScreen && !viewModel.IsViewingImage)
            {
                GoNormal();
                e.Cancel = true;
            }
            //else if (viewModel.IsViewingImage)
            //{
            //    viewModel.IsViewingImage = false;
            //    e.Cancel = true;
            //}
        }

    }
}