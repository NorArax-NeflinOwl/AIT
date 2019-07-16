using System;
using Newtonsoft.Json; //references: Json.NET .Net3.5 \ Newtonsoft.Json
using System.Diagnostics;
using AIT.Helpers;
using AIT.Constant; 

namespace AIT.FilesManagers
{
    public class aitCryptJsonManager
    {        
        private JsonSerializerSettings m_Setting;

        private aitCryptJsonManager() 
        {
            m_Setting = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }

        private static object locker = new object();
        private static aitCryptJsonManager m_Instance;
        public static aitCryptJsonManager Instance
        {
            get
            {
                lock (locker)
                {
                    if (m_Instance == null)
                        m_Instance = new aitCryptJsonManager();
                    return m_Instance;
                }
            }
        }

        public string Serialize(object obj)
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj, m_Setting);
                return aitEncryptionHelper.Encrypt(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                aitExceptionsLogger.Instance.LogToFile(new aitLog { Title = aitLogTitle.EXCEPTION, Message = ex.ToString() });
                return string.Empty;
            }
        }
        public T Deserialize<T>(string text)
        {
            try
            {
                var json = aitEncryptionHelper.Decrypt(text);
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                aitExceptionsLogger.Instance.LogToFile(new aitLog { Title = aitLogTitle.EXCEPTION, Message = ex.ToString() });
                return default(T);
            }
        }

        public void SerializeObjectToFile(object obj)
        {
            try
            {
                var content = Serialize(obj);
                var dirPath = aitFileManager.CombinePath(aitStrings.OBJDIR_SUBPATH);
                var filePath = aitFileManager.CombinePath(dirPath, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                using (var manager = new aitFileManager())
                {
                    manager.WriteToFile(filePath, content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                aitExceptionsLogger.Instance.LogToFile(new aitLog { Title = aitLogTitle.EXCEPTION, Message = ex.ToString() });
            }
        }
    }
}
