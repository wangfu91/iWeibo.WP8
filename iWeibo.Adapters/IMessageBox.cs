using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace iWeibo.Adapters
{
    public interface IMessageBox
    {
        void Show(string messageBoxText);
        void Show(string messageBoxText, string caption, MessageBoxButton button);

    }
}
