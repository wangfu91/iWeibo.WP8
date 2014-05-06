using System;
using System.Net;
using System.Threading;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.IO;
using TencentWeiboSDK.Model;

namespace TencentWeiboSDK.Util
{
    /// <summary>
    /// TokenIso 用于安全储存 AccessToken.
    /// </summary>
    public class TokenIso : SafeIsolatedStorage<TencentAccessToken>
    {
        private static TokenIso instance = new TokenIso();

        private TokenIso()
            : base("internal_token.dat")
        { }

        /// <summary>
        /// 实例.
        /// </summary>
        public static TokenIso Current
        {
            get {
                return instance;
            }
        }
    }
    
    /// <summary>
    /// SafeIsolatedStorage 类，用于将数据安全储存到用户独立储存空间.
    /// </summary>
    /// 推荐使用该类进行数据存储及读取，因为在存储过程中可能遇到系统崩溃等情况，导致
    /// 文件的损坏，使用该类可以最大限度保证数据的完整性.
    /// <typeparam name="T">需要存储的数据类型.</typeparam>
    public class SafeIsolatedStorage<T> where T : new()
    {
        private string fileName = string.Empty;

        /// <summary>
        /// 构造函数.
        /// </summary>
        internal SafeIsolatedStorage()
        { }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="fileName">需要储存的文件名.</param>
        public SafeIsolatedStorage(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// 清除已存储的文件.
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
        /// 从用户独立储存空间的文件中读取数据.
        /// </summary>
        /// <returns>返回读取出来的数据.</returns>
        public T LoadData()
        {
            T t = new T();
            try
            {
                t = InternalLoadData(BackupFileName);

                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (file.FileExists(fileName))
                        file.DeleteFile(fileName);
                    file.CopyFile(BackupFileName, fileName);
                };
            }
            catch (Exception ex)
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        t = InternalLoadData(fileName);

                        if (file.FileExists(BackupFileName))
                            file.DeleteFile(BackupFileName);
                        file.CopyFile(fileName, BackupFileName);
                    }
                    catch (Exception e)
                    {
                        file.DeleteFile(BackupFileName);
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// 将数据存储到用户独立储存空间的文件.
        /// </summary>
        /// <param name="t">需要储存的实例.</param>
        public void SaveData(T t)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    if (file.FileExists(fileName))
                        file.DeleteFile(fileName);

                    using (IsolatedStorageFileStream stream = file.CreateFile(fileName))
                    {
                        new DataContractSerializer(t.GetType()).WriteObject(stream, t);
                    }

                    if (file.FileExists(BackupFileName))
                        file.DeleteFile(BackupFileName);

                    file.CopyFile(fileName, BackupFileName);
                }
                catch (Exception ex)
                {
                    if (file.FileExists(fileName))
                        file.DeleteFile(fileName);

                    file.CopyFile(BackupFileName, fileName);
                }
            }
        }

        private T InternalLoadData(string fileName)
        {
            T t = new T();

            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists(fileName))
                {
                    using (IsolatedStorageFileStream stream = file.OpenFile(fileName, FileMode.Open))
                    {
                        t = (T)new DataContractSerializer(t.GetType()).ReadObject(stream);
                    }
                }
                else
                {
                    file.Dispose();
                    throw new Exception("file not exist");
                }
            }

            return t;
        }

        private string BackupFileName
        {
            get
            {
                return string.Format("{0}_bak", fileName);
            }
        }
    }
}
