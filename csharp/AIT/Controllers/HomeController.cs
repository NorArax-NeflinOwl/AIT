using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace AIT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //mailSender("ppudi7368@gmail.com", "http://warofclicks.com/User/Activation/1380e538-9a29-4773-bfd5-37152da18d53");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private void mailSender(string to, string activationLint)
        {
            string from = "ait.wms.nano@gmail.com";
            string password = "#_@rnn0I";

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                message.From = new MailAddress(from);
                message.To.Add(to);
                message.Subject = "New User Activation Request";
                message.Body = $"Activation Link: {activationLint}";

                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(from, password);

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}