using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WPF.Enums;
using WPF.Models;
using WPF.Properties;

namespace WPF.Managers
{
    public class CryptoJsonManager
    {
        private JsonSerializerSettings m_Setting;
        private EncryptionManager m_Encrypter;

        private CryptoJsonManager()
        {
            m_Encrypter = new EncryptionManager();
            m_Setting = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }

        private static object locker = new object();
        private static CryptoJsonManager m_Instance;
        public static CryptoJsonManager Instance
        {
            get
            {
                lock (locker)
                {
                    if (m_Instance == null)
                        m_Instance = new CryptoJsonManager();
                    return m_Instance;
                }
            }
        }

        public string Serialize(object obj, string password = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj, m_Setting);
                return m_Encrypter.Encrypt(json, password);
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.LogExceptionToFile(ex);
                return string.Empty;
            }
        }

        public T Deserialize<T>(string text, string password = null)
        {
            try
            {
                var json = m_Encrypter.Decrypt(text, password);
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.LogExceptionToFile(ex);
                return default(T);
            }
        }

        public void SerializeObjectToFile(object obj)
        {
            try
            {
                var content = Serialize(obj);
                var dirPath = FileManager.CombinePath(Resources.OBJDIR_SUBPATH);
                var filePath = FileManager.CombinePath(dirPath, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                using (var manager = new FileManager())
                {
                    manager.WriteToFile(filePath, content);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.LogExceptionToFile(ex);
            }
        }
    }
}
