using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWeibo.Adapters
{
    public class PhoneApplicationServiceFacade:IPhoneApplicationServiceFacade
    {
        public void Save(string key, object value)
        {
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                PhoneApplicationService.Current.State.Remove(key);
            }

            PhoneApplicationService.Current.State.Add(key, value);
        }


        public T Load<T>(string key)
        {
            object result;
            if (!PhoneApplicationService.Current.State.TryGetValue(key, out result))
            {
                result = default(T);
            }
            else
            {
                PhoneApplicationService.Current.State.Remove(key);
            }
            return (T)result;
        }

        public void Remove(string key)
        {
            if (PhoneApplicationService.Current.State.ContainsKey(key))
            {
                PhoneApplicationService.Current.State.Remove(key);
            }
        }
    }
}
