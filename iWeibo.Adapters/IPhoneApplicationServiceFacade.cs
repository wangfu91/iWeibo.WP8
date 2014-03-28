using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWeibo.Adapters
{
    public interface IPhoneApplicationServiceFacade
    {
        void Save(string key, object value);
        T Load<T>(string key);
        void Remove(string key);
    }
}
