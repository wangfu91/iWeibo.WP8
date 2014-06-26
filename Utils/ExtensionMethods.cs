using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.Utils
{
    public static class ExtensionMethods
    {
        public static Task<TTaskEventArgs> ShowAsync<TTaskEventArgs>(this ChooserBase<TTaskEventArgs> chooser)
            where TTaskEventArgs : TaskEventArgs
        {
            var taskCompletionSource = new TaskCompletionSource<TTaskEventArgs>();

            EventHandler<TTaskEventArgs> completed = null;

            completed = (s, e) =>
            {
                chooser.Completed -= completed;

                taskCompletionSource.SetResult(e);
            };

            chooser.Completed += completed;
            chooser.Show();

            return taskCompletionSource.Task;
        }

        //Sample

        //// open the chooser and await for the result

        //var photoResult = await new PhotoChooserTask()
        //    .ShowAsync();

        //// process the result

        //if (photoResult.TaskResult == TaskResult.OK)
        //{
        //    var bitmapImage = new BitmapImage();

        //    using (photoResult.ChosenPhoto)
        //    {
        //        bitmapImage.SetSource(photoResult.ChosenPhoto);

        //        SelectedImage.Source = bitmapImage;
        //    }
    }
}
