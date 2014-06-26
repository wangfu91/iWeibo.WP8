using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iWeibo.Utils
{
    public class MTLongListSelector : LongListSelector
    {
        public MTLongListSelector()
        {
            ItemRealized += OnItemRealized;
            SelectionChanged += MTLongListSelector_SelectionChanged;
        }

        void MTLongListSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedItem = base.SelectedItem;
        }

        public static readonly DependencyProperty SelectedItemProperty =
           DependencyProperty.Register(
               "SelectedItem",
               typeof(object),
               typeof(MTLongListSelector),
               new PropertyMetadata(null, OnSelectedItemChanged)
           );

        private void SetSelectedItem(DependencyPropertyChangedEventArgs e)
        {
            base.SelectedItem = e.NewValue;
        }
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (MTLongListSelector)d;
            selector.SetSelectedItem(e);
        }



        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(MTLongListSelector), new PropertyMetadata(default(bool)));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        private const int Offset = 1;

        public event EventHandler DataRequest;

        protected virtual void OnDataRequest()
        {
            EventHandler handler = DataRequest;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OnItemRealized(object sender, ItemRealizationEventArgs itemRealizationEventArgs)
        {
            if (!IsLoading && ItemsSource != null && ItemsSource.Count >= Offset)
            {
                if (itemRealizationEventArgs.ItemKind == LongListSelectorItemKind.Item)
                {
                    var offsetItem = ItemsSource[ItemsSource.Count - Offset];
                    if ((itemRealizationEventArgs.Container.Content == offsetItem))
                    {
                        OnDataRequest();
                    }
                }
            }
        }
    }
}
