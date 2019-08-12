using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;
using WPF.Models;

namespace WPF.Managers.Helpers
{
    public class MailDownloader
    {
        public static List<MailBoxItemModel> UnreadMailsDownload()
        {
            var crypter = new EncryptionManager();
            string email = ConfigurationManager.AppSettings["AppEmail"].ToString();
            string password = crypter.Decrypt(ConfigurationManager.AppSettings["AppEmailPassword"].ToString(), email);

            try
            {
                string response;
                List<MailBoxItemModel> mailBox = new List<MailBoxItemModel>();
                System.Net.WebClient objClient = new System.Net.WebClient();

                XmlDocument doc = new XmlDocument();
                objClient.Credentials = new System.Net.NetworkCredential(email, password);
                response = Encoding.UTF8.GetString(objClient.DownloadData(@"https://mail.google.com/mail/feed/atom/all"));
                response = response.Replace(@"<feed version=""0.3"" xmlns=""http://purl.org/atom/ns#"">", @"<feed>");

                doc.LoadXml(response);
                //string nr = doc.SelectSingleNode(@"/feed/fullcount").InnerText;

                foreach (XmlNode node in doc.SelectNodes(@"/feed/entry"))
                {
                    var mailBoxItem = new MailBoxItemModel();
                    mailBoxItem.Title = node.SelectSingleNode("title").InnerText;
                    mailBoxItem.Summary = node.SelectSingleNode("summary").InnerText;
                    var time = node.SelectSingleNode("issued").InnerText;
                    mailBoxItem.Issued = DateTime.Parse(time);

                    var authorNode = node.SelectNodes(@"author");
                    foreach (XmlNode authorInfo in authorNode)
                    {
                        mailBoxItem.Author.Name = authorInfo.SelectSingleNode("name").InnerText;
                        mailBoxItem.Author.Email = authorInfo.SelectSingleNode("email").InnerText;
                    }
                    mailBox.Add(mailBoxItem);
                }
                return mailBox;
            }
            catch (Exception exe)
            {
                LogManager.Instance.LogExceptionToFile(exe, "Check your network connection");
            }
            return null;
        }
    }
}
