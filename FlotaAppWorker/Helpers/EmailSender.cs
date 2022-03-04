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
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IConfiguration _config;

        public EmailSender(ILogger<EmailSender> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void Send(Car car, string message)
        {
            string login = _config["login"];
            string password = _config["password"];
            string smtp = _config["smtp"];
            string Email = _config["AnnaEmail"];

            try
            {
                SmtpClient mySmtpClient = new SmtpClient(smtp);
                mySmtpClient.UseDefaultCredentials = false;
                NetworkCredential basicAuthenticationInfo = new
                NetworkCredential(login, password);
                mySmtpClient.Credentials = basicAuthenticationInfo;
                mySmtpClient.EnableSsl = true;

                MailAddress from = new MailAddress(login, "Flota Manager");
                MailAddress to = car?.User?.Email == null ? new MailAddress(Email) : new MailAddress(car.User.Email);
                MailMessage myMail = new MailMessage(from, to);

                myMail.Subject = $"Powiadomienie odnośnie pojazdu: {car.Brand} {car.Model} - {car.RegistrationNumber}";
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


