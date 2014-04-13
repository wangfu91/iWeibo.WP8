using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace iWeibo.WP8.Converters
{
    public class SourceConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string so = string.Empty;
            if (value != null)
            {
                string source = value.ToString();
                if (!string.IsNullOrEmpty(source))
                {
                    int pos = source.IndexOf(">");
                    if (-1 != pos)
                    {
                        int end = source.IndexOf("</a>", pos, source.Length - pos);
                        so = source.Substring(pos + 1, end - pos - 1);
                    }
                }
            }
            return so;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
