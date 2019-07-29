using Newtonsoft.Json;
using System;
using WPF.Properties;

namespace WPF.Managers
{
    public class CryptoJsonManager : IDisposable
    {
        private readonly string CRYPTO_FLAG = "[CRYPT]";
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
                return !string.IsNullOrEmpty(password) ? CRYPTO_FLAG + m_Encrypter.Encrypt(json, password) : json;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
                return string.Empty;
            }
        }

        public T Deserialize<T>(string text, string password = null)
        {
            try
            {
                var json = text;
                if(text.Contains(CRYPTO_FLAG))
                {
                    text = text.Substring(CRYPTO_FLAG.Length);
                    json = m_Encrypter.Decrypt(text, password);
                }
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
                return default(T);
            }
        }

        public void SerializeObjectToFile(object obj)
        {
            try
            {
                var content = Serialize(obj);
                var ext = ".json";
                if(content.Contains(CRYPTO_FLAG))
                {
                    ext = Resources.CRYPT_EXT;
                }

                var dirPath = FileManager.CombinePath(Resources.OBJDIR_SUBPATH);
                var filePath = FileManager.CombinePath(dirPath, DateTime.Now.Date.ToString("yyyy-MM-dd"), ext);
                using (var manager = new FileManager())
                {
                    manager.WriteToFile(filePath, content);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
