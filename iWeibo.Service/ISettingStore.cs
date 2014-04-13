using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.WP8.Services
{
    public interface ISettingStore
    {
        void AddOrUpdateValue(string key, object value);

        T GetValueOrDefault<T>(string key,T deafultValue);

    }
}
