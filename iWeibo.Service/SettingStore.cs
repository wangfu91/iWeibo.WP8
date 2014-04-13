using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.WP8.Services
{
    public class SettingStore:ISettingStore
    {
        private readonly IsolatedStorageSettings settingStore;

        public void AddOrUpdateValue(string key, object value)
        {
            bool valueChanged = false;
            try
            {
                //if new value is different, save the value.
                if(settingStore[key]!=value)
                {
                    settingStore[key]=value;
                    valueChanged=true;
                }
            }
            catch(KeyNotFoundException)
            {
                settingStore.Add(key,value);
                valueChanged=true;
            }
            catch(ArgumentException)
            {
                settingStore.Add(key,value);
                valueChanged=true;
            }

            if(valueChanged)
            {
                Save();
            }
        }

        public T GetValueOrDefault<T>(string key, T deafultValue)
        {
            T value;
            try
            {
                value = (T)settingStore[key];
            }
            catch(KeyNotFoundException)
            {
                value = deafultValue;
            }
            catch(ArgumentException)
            {
                value = deafultValue;
            }
            return value;
        }

        private void Save()
        {
            settingStore.Save();
        }
    }
}
