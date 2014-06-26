using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace iWeibo.Utils
{
    public static class RadDataBoundListBoxExtensions
    {
        public static bool GetIsEnd(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEndProperty);
        }

        public static void SetIsEnd(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEndProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEndProperty =
            DependencyProperty.RegisterAttached("IsEnd", typeof(bool), typeof(RadDataBoundListBoxExtensions), new PropertyMetadata(true, (d, e) =>
            {
                RadDataBoundListBox listbox = d as RadDataBoundListBox;
                if (listbox != null)
                {
                    if ((bool)e.NewValue)
                        listbox.DataVirtualizationMode = DataVirtualizationMode.None;
                    else
                        listbox.DataVirtualizationMode = DataVirtualizationMode.OnDemandAutomatic;
                }
            }));



        public static bool GetPullToRefreshEnd(DependencyObject obj)
        {
            return (bool)obj.GetValue(PullToRefreshEndProperty);
        }

        public static void SetPullToRefreshEnd(DependencyObject obj, bool value)
        {
            obj.SetValue(PullToRefreshEndProperty, value);
        }

        // Using a DependencyProperty as the backing store for PullToRefreshEnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PullToRefreshEndProperty =
            DependencyProperty.RegisterAttached("PullToRefreshEnd", typeof(bool), typeof(RadDataBoundListBoxExtensions), new PropertyMetadata(false, (d, e) =>
            {
                RadDataBoundListBox listbox = d as RadDataBoundListBox;
                if (listbox != null)
                {
                    if ((bool)e.NewValue)
                        listbox.StopPullToRefreshLoading(true, true);
                }
            }));

        

    }
}
