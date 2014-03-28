using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace iWeibo.Services
{
    public class IsoStorage:IIsoStorage
    {
        private string fileName = string.Empty;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="fileName">文件名</param>
        public IsoStorage(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// 清除已存储文件
        /// </summary>
        public void Clear()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(fileName))
                {
                    isf.DeleteFile(fileName);
                }
            }
        }

        /// <summary>
        /// 将数据存储到用户独立储存空间的文件中
        /// </summary>
        /// <param name="value">需要存储的实例</param>
        public void SaveData(object value)
        {
            if (value == null)
                return;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (isf.FileExists(fileName))
                {
                    isf.DeleteFile(fileName);
                }
                //using (IsolatedStorageFileStream stream = isf.CreateFile(fileName))
                //{
                //    //序列化指定类型对象，将其完整内容写入XML文档
                //    new DataContractSerializer(t.GetType()).WriteObject(stream, t);
                //}

                using (TextWriter writer = new StreamWriter(isf.CreateFile(fileName)))
                {
                    new JsonSerializer().Serialize(writer, value);
                }

            }
        }

        //public void SaveString(string txt)
        //{
        //    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        if (isf.FileExists(fileName))
        //        {
        //            isf.DeleteFile(fileName);
        //        }

        //        using (StreamWriter writer = new StreamWriter(isf.CreateFile(fileName)))
        //        {
        //            writer.Write(txt);
        //        }
        //    }
        //}

        /// <summary>
        /// 从用户独立储存空间的文件中读取数据
        /// </summary>
        /// <returns>返回读取到的数据</returns>
        public T LoadData<T>() where T:new()
        {
            T t=new T();
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(fileName))
                {
                    //using (IsolatedStorageFileStream stream = isf.OpenFile(fileName, FileMode.Open))
                    //{
                    //    //读取XML文档并返回反序列化的对象
                    //    t = (T)new DataContractSerializer(t.GetType()).ReadObject(stream);
                    //}

                    using (TextReader reader = new StreamReader(isf.OpenFile(fileName, FileMode.Open)))
                    {
                        t = (T)new JsonSerializer().Deserialize(reader, t.GetType());
                    }
                }
            }
            return t;
        }

        public bool TryLoadData<T>(out T t)where T:new()
        {
            t = new T();
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(fileName))
                {
                    try
                    {
                        //using (IsolatedStorageFileStream stream = isf.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                        //{
                        //    t = (T)new DataContractSerializer(t.GetType()).ReadObject(stream);
                        //}

                        using (TextReader reader = new StreamReader(isf.OpenFile(fileName, FileMode.Open)))
                        {
                            t = (T)new JsonSerializer().Deserialize(reader, t.GetType());
                        }
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }
        }
    }
}
