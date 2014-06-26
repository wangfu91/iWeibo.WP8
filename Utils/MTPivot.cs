using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace iWeibo.Utils
{
    /// <summary>
    /// 优化的Pivot主要使用方法同Pivot，主要作用是提高页面切换速度，谁用谁知道
    /// </summary>
    public class MTPivot : Pivot
    {
        private PhoneApplicationFrame _currentApplicationFrame;
        private PhoneApplicationPage _currentPage;
        private bool _initilized;

        public MTPivot()
        {
            LoadedPivotItem += OnLoadedPivotItem;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!_initilized)
            {
                InitilizeEvent();
                _initilized = true;
                //dataBinding 的pivot存在一个已知的问题 首次进入不会触发itemloaded事件，selectedItem也为空
                //因此检测到选中为空的情况下，手动给selectedItem赋值
                if (SelectedItem == null)
                {
                    if (Items != null && Items.Count > 0)
                        SelectedItem = Items[0];
                }
            }
        }

        private void CurrentPageOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!Execute.InDesignMode)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Thread.Sleep(250);
                    Dispatcher.BeginInvoke(SetSelectedItemVisible);
                });
            }
        }

        private void FrameOnNavigated(object sender, NavigationEventArgs e)
        {
            string currentPageName = _currentPage.ToString().Split('.').LastOrDefault();
            if (currentPageName != null)
            {
                //从_current页面离开的情况下将当前页面中的的Pivot SelectedItem隐藏
                if (e.NavigationMode == NavigationMode.New)
                {
                    if (CurrentPageAtBackStackTop(currentPageName))
                    {
                        SetSelectedItemCollapse();
                    }
                }
                if (e.NavigationMode == NavigationMode.Back)
                {
                    if (CurrentPageNotExsitInBackStack(currentPageName))
                    {
                        //当前页面不在回退栈的情况分为两种：
                        //1、从当前页面离开
                        //2、回退到当前页面
                        //第一种情况下注销引用
                        if (NotNavigateToCurrentPage(e, currentPageName))
                        {
                            ReleasePageAndFrame();
                        }
                    }
                }
            }
        }

        private static bool NotNavigateToCurrentPage(NavigationEventArgs e, string currentPageName)
        {
            return e.Uri.OriginalString.Contains(currentPageName) == false;
        }

        private bool CurrentPageNotExsitInBackStack(string currentPageName)
        {
            return
                !_currentApplicationFrame.BackStack.Any(
                    journalEntry => journalEntry.Source.OriginalString.Contains(currentPageName));
        }

        private bool CurrentPageAtBackStackTop(string currentPageName)
        {
            JournalEntry backStackTopPage = _currentApplicationFrame.BackStack.FirstOrDefault();
            return (backStackTopPage != null && backStackTopPage.Source.OriginalString.Contains(currentPageName));
        }

        private void SetSelectedItemCollapse()
        {
            if (SelectedItem != null)
            {
                SetPivotItemVisibility(SelectedItem, Visibility.Collapsed);
            }
        }

        private void SetSelectedItemVisible()
        {
            if (SelectedItem != null)
            {
                SetPivotItemVisibility(SelectedItem, Visibility.Visible);
            }
        }

        /// <summary>
        ///     注册Frame和Page的事件以根据Page事件控制自身显示
        /// </summary>
        private void InitilizeEvent()
        {
            _currentApplicationFrame = (Application.Current.RootVisual) as PhoneApplicationFrame;
            if (_currentApplicationFrame != null)
            {
                _currentApplicationFrame.Navigated += FrameOnNavigated;
                _currentPage = _currentApplicationFrame.Content as PhoneApplicationPage;
                if (_currentPage != null)
                    _currentPage.Loaded += CurrentPageOnLoaded;
            }
        }

        /// <summary>
        ///     释放掉引用的资源
        /// </summary>
        private void ReleasePageAndFrame()
        {
            _currentApplicationFrame.Navigated -= FrameOnNavigated;
            _currentApplicationFrame = null;
            _currentPage.Loaded -= CurrentPageOnLoaded;
            _currentPage = null;
        }

        /// <summary>
        ///     PivotItemLoaded之后根据当前所在的PivotItem控制Items的可见行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (!Execute.InDesignMode)
            {
                //非databinding pivot的item.content是pivotItem类型
                if (e.Item.Content is UIElement)
                {
                    foreach (object pivotItem in Items)
                    {
                        if (pivotItem != e.Item)
                            //非当前Item则隐藏
                            SetPivotItemVisibility(pivotItem, Visibility.Collapsed);
                        else
                        {
                            //Item为当前选中的Item则显示
                            SetPivotItemVisibility(pivotItem, Visibility.Visible);
                        }
                    }
                }
                //dataBinding Pivot
                else
                {
                    foreach (object item in Items)
                    {
                        if (item != e.Item.Content)
                        {
                            SetPivotItemVisibility(item, Visibility.Collapsed);
                        }
                        else
                        {
                            SetPivotItemVisibility(item, Visibility.Visible);
                        }
                    }
                }
            }
        }

        public void SetPivotItemVisibility(object item, Visibility visibility)
        {
            if (item == null) return;
            UIElement content;
            if (item is PivotItem)
            {
                content = ((PivotItem)item).Content as UIElement;
            }
            else
            {
                content = ItemContainerGenerator.ContainerFromItem(item) as UIElement;
            }
            Execute.BeginInvokeWithDelay(100, () =>
            {
                if (content != null)
                {
                    content.Visibility = visibility;
                }
            });
        }
    }

    internal class Execute
    {
        //延迟ticks毫秒之后在UI线程上执行
        public static void BeginInvokeWithDelay(int ticks, Action action)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.Sleep(ticks);
                Deployment.Current.Dispatcher.BeginInvoke(action);
            });
        }

        public static bool InDesignMode
        {
            get { return DesignerProperties.IsInDesignTool; }
        }

    }
}
