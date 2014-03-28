using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace iWeibo.Adapters
{
    public class MessageBoxAdapter:IMessageBox
    {
        public void Show(string messageBoxText)
        {
            MessageBox.Show(messageBoxText);
        }

        public void Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageBox.Show(messageBoxText, caption, button);
        }
    }
}
