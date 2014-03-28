using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWeibo.Adapters
{
    public class PhotoChooserTaskAdapter : IPhotoChooserTask
    {
        public PhotoChooserTaskAdapter()
        {
            WrappedSubject = new PhotoChooserTask();
            WrappedSubject.ShowCamera = true;
            AttachToEvents();
        }

        private PhotoChooserTask WrappedSubject { get; set; }

        public SettablePhotoResult TaskEventArgs
        {
            get
            {
                return new SettablePhotoResult(WrappedSubject.TaskEventArgs);
            }
            set
            {
                WrappedSubject.TaskEventArgs = value;
            }
        }

        public event EventHandler<SettablePhotoResult> Completed;

        private void AttachToEvents()
        {
            WrappedSubject.Completed += WrappedSubjectCompleted;
        }

        void WrappedSubjectCompleted(object sender, PhotoResult e)
        {
            CompletedRelay(sender, new SettablePhotoResult(e));
        }

        public void Show()
        {
            WrappedSubject.Show();
        }

        private void CompletedRelay(object sender, SettablePhotoResult e)
        {
            var handler = Completed;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
