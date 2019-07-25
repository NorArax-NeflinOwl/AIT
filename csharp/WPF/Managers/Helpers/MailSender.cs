using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace WPF.Managers.Helpers
{
    public class MailSender
    {
        public static void SendTo(string to, string title, string content)
        {
            var crypter = new EncryptionManager();
            string from = ConfigurationManager.AppSettings["AppEmail"].ToString();
            string password = crypter.Decrypt(ConfigurationManager.AppSettings["AppEmailPassword"].ToString(), from);

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                message.From = new MailAddress(from);
                message.To.Add(to);
                message.Subject = title;
                message.Body = content;

                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(from, password);

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogExceptionToFile(ex);
            }
        }
    }
}
