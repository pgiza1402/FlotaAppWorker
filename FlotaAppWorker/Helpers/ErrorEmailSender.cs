using FlotaAppWorker.Data;
using FlotaAppWorker.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Helpers
{
    internal class ErrorEmailSender : IErrorEmailSender
    {

        private readonly ILogger<ErrorEmailSender> _logger;
        private readonly IConfiguration _config;

        public ErrorEmailSender(ILogger<ErrorEmailSender> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        public void Send(string statusCode, string message)
        {
            string serviceLogin = _config["login"];
            string servicePassword = _config["password"];
            string AdministratorEmail = _config["AdministratorEmail"];
            string smtp = _config["smtp"];


            try
            {
                SmtpClient mySmtpClient = new SmtpClient(smtp);
                mySmtpClient.UseDefaultCredentials = false;
                NetworkCredential basicAuthenticationInfo = new
                NetworkCredential(serviceLogin, servicePassword);
                mySmtpClient.Credentials = basicAuthenticationInfo;
                mySmtpClient.EnableSsl = true;

                MailAddress from = new MailAddress(serviceLogin, "Flota Manager");
                MailAddress to = new MailAddress(AdministratorEmail);
                MailMessage myMail = new MailMessage(from, to);

                myMail.Subject = statusCode != null ? $"Błąd wysyłania SMS: kod błędu - {statusCode} " : $"Błąd wysyłania SMS";
                myMail.SubjectEncoding = Encoding.UTF8;
                myMail.Body = message;
                myMail.BodyEncoding = Encoding.UTF8;
                myMail.IsBodyHtml = true;
                mySmtpClient.Send(myMail);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

    }
}
