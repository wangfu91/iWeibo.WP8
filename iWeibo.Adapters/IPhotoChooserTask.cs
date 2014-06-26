using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.Adapters
{
    public interface IPhotoChooserTask
    {
        SettablePhotoResult TaskEventArgs
        {
            get;
            set;
        }

        event EventHandler<SettablePhotoResult> Completed;

        void Show();
    }
}
