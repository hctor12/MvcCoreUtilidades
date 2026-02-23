using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace MvcCoreUtilidades.Controllers
{
    public class MailsController : Controller
    {
        private IConfiguration configuration;

        public MailsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(string to, string asunto, string mensaje)
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(user);
            mail.To.Add(to);
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            string password = this.configuration.GetValue<string>("MailSettings:Credentials:Password");
            string host = this.configuration.GetValue<string>("Server:Host");
            int port = this.configuration.GetValue<int>("Server:Port");
            bool ssl = this.configuration.GetValue<bool>("Server:Ssl");
            bool defaultCredentials = this.configuration.GetValue<bool>("Server:DefaultCredentials");
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = port;
            client.EnableSsl = ssl;
            client.UseDefaultCredentials = defaultCredentials;  // Set BEFORE credentials
            NetworkCredential credentials = new NetworkCredential(user, password);
            client.Credentials = credentials;  // Set AFTER
            await client.SendMailAsync(mail);
            ViewData["MENSAJE"] = "Mensaje enviado correctamente.";
            return View();
        }
    }
}
