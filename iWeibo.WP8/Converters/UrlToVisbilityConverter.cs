using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace iWeibo.WP8.Converters
{
    public class UrlToVisbilityConverter:IValueConverter
    {
        public bool Negative { get; set; }

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var url = (string)value;
            var visibility = url.EndsWith(".gif");
            return ((this.Negative && !visibility) || (!this.Negative && visibility)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
