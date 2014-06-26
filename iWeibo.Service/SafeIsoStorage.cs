using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace iWeibo.Services
{
    /// <summary>
    /// SafeIsolatedStorage类，用于将数据安全存储到用户独立存储空间
    /// </summary>
    /// 推荐使用该类进行数据存储及读取，使用该类可以最大限度保证数据的完整性
    /// <typeparam name="T">需要存储的数据类型</typeparam>
    public class SafeIsoStorage:IIsoStorage
    {
        private string fileName = string.Empty;
        private string BackupFileName
        {
            get
            {
                return string.Format("{0}_bak", this.fileName);
            }
        }

        public SafeIsoStorage(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// 清除已存储的文件
        /// </summary>
        public void Clear()
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists(fileName))
                {
                    file.DeleteFile(fileName);
                }
                if (file.FileExists(BackupFileName))
                {
                    file.DeleteFile(BackupFileName);
                }
            }
        }

        /// <summary>
        /// 从用户独立存储空间的文件中读取数据
        /// </summary>
        /// <returns>返回读取出的数据</returns>
        public T LoadData<T>() where T:new()
        {
            T t = new T();
            try
            {
                t = InternalLoadData<T>(BackupFileName);
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (file.FileExists(fileName))
                    {
                        file.DeleteFile(fileName);
                    }
                    file.CopyFile(BackupFileName, fileName);
                };
            }
            catch
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        t = InternalLoadData<T>(fileName);
                        if (file.FileExists(BackupFileName))
                        {
                            file.DeleteFile(BackupFileName);
                        }
                        file.CopyFile(fileName, BackupFileName);
                    }
                    catch
                    {
                        if (file.FileExists(BackupFileName))
                        {
                            file.DeleteFile(BackupFileName);
                        }
                    }
                }
            }
            return t;
        }

        private T InternalLoadData<T>(string fileName)where T:new()
        {
            T t = new T();
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists(fileName))
                {
                    //using (IsolatedStorageFileStream stream = file.OpenFile(fileName, FileMode.Open))
                    //{
                    //    //反序列化指定对象
                    //    t = (T)new DataContractSerializer(t.GetType()).ReadObject(stream);
                    //}
                    using (TextReader reader = new StreamReader(file.OpenFile(fileName, FileMode.Open)))
                    {
                        t = (T)new JsonSerializer().Deserialize(reader, t.GetType());
                    }
                }
                else
                {
                    file.Dispose();
                    throw new Exception("File Not Exist");
                }
            }
            return t;
        }

        public bool TryLoadData<T>(out T t)where T:new()
        {
            t = new T();
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists(fileName))
                {
                    try
                    {
                        //using (IsolatedStorageFileStream stream = file.OpenFile(fileName, FileMode.Open))
                        //{
                        //    //反序列化指定对象
                        //    t = (T)new DataContractSerializer(t.GetType()).ReadObject(stream);
                        //}
                        using (TextReader reader = new StreamReader(file.OpenFile(fileName, FileMode.Open)))
                        {
                            t = (T)new JsonSerializer().Deserialize(reader, t.GetType());
                        }
                    }
                    catch
                    {
                        return false;
                    }

                    return true;
                }
                return false;
            }

        }

        /// <summary>
        /// 将数据存储到用户独立存储空间的文件
        /// </summary>
        /// <param name="value">需要存储的实例</param>
        public void SaveData(object value)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    if (file.FileExists(fileName))
                    {
                        file.DeleteFile(fileName);
                    }
                    //using (IsolatedStorageFileStream stream = file.CreateFile(fileName))
                    //{
                    //    new DataContractSerializer(t.GetType()).WriteObject(stream, t);
                    //}

                    using (TextWriter writer = new StreamWriter(file.CreateFile(fileName)))
                    {
                        new JsonSerializer().Serialize(writer, value);
                    }

                    if (file.FileExists(BackupFileName))
                    {
                        file.DeleteFile(BackupFileName);
                    }
                    file.CopyFile(fileName, BackupFileName);
                }
                catch
                {
                    if (file.FileExists(fileName))
                    {
                        file.DeleteFile(fileName);
                    }
                    file.CopyFile(BackupFileName, fileName);
                }
            }
        }

    }
}
