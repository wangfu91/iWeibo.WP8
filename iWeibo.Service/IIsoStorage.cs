using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iWeibo.Services
{
    public interface IIsoStorage
    {

        void Clear();

        void SaveData(object value);

        T LoadData<T>() where T : new();

        bool TryLoadData<T>(out T t) where T : new();
    }
}
