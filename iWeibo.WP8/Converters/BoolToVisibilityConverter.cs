using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace iWeibo.WP8.Converters
{
    public class BoolToVisibilityConverter:IValueConverter
    {
        public bool Negative { get; set; }

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var visibility = (bool)value;
            return ((this.Negative && !visibility) || (!this.Negative && visibility)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
