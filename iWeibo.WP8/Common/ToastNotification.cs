using Coding4Fun.Toolkit.Controls;
using iWeibo.Adapters;
using iWeibo.WP8.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iWeibo.WP8.Common
{
    public class ToastNotification
    {
        public static void Show(bool succeed,int count=0,string msg="")
        {
            if (succeed)
            {
                var toast = new ToastPrompt()
                {
                    Message = string.IsNullOrEmpty(msg) ? string.Format(AppResources.ReceivedWeiboText, count) : msg,
                    MillisecondsUntilHidden = 3000
                };
                toast.Show();
            }
            else
            {
                MessageBox.Show(msg);
            }
        }
    }
}
