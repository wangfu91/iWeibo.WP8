using System.ComponentModel;
using System.Windows;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// BaseModel 为所有 Model 的基类. 实现了 INotifyPropertyChanged 接口.
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public BaseModel()
        { }
		
        /// <summary>
        /// Model 的属性发生变更时触发 PropertyChanged 事件.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 若该属性绑定到了UI，则触发 PropertyChanged事件
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            });
        }
    }
}
