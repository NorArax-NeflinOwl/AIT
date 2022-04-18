using AITLib.Constants;
using AITLib.Helpers;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace AIT.Helpers
{
    public class AitCryptJsonManager
    {        
        private JsonSerializerSettings m_Setting;
        private AitEncryptionHelper m_Encrypter;

        private AitCryptJsonManager() 
        {
            m_Encrypter = new AitEncryptionHelper();
            m_Setting = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }

        private static object locker = new object();
        private static AitCryptJsonManager m_Instance;
        public static AitCryptJsonManager Instance
        {
            get
            {
                lock (locker)
                {
                    if (m_Instance == null)
                        m_Instance = new AitCryptJsonManager();
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
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });
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
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });
                return default(T);
            }
        }

        public void SerializeObjectToFile(object obj)
        {
            try
            {
                var content = Serialize(obj);
                var dirPath = AitFileManager.CombinePath(AitStrings.OBJDIR_SUBPATH);
                var filePath = AitFileManager.CombinePath(dirPath, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                using (var manager = new AitFileManager())
                {
                    manager.WriteToFile(filePath, content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });
            }
        }
    }
}
