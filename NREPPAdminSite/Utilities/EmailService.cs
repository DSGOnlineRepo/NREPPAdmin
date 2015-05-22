using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using NREPPAdminSite.Constants;

namespace NREPPAdminSite.Utilities
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpClientWrapper _smtpClientWrapper;

        public EmailService() : this(new SmtpClientWrapper()) { }

        public EmailService(ISmtpClientWrapper smtpClientWrapper)
        {
            _smtpClientWrapper = smtpClientWrapper;
        }

        public void SendEmail(MailMessage message)
        {
            int port = Int32.Parse(ConfigurationManager.AppSettings[EmailConstants.EmailPort]);
            int timeout = Int32.Parse(ConfigurationManager.AppSettings[EmailConstants.EmailTimeout]);
            string host = ConfigurationManager.AppSettings[EmailConstants.EmailHost];
            string username = ConfigurationManager.AppSettings[EmailConstants.EmailUsername];
            string password = ConfigurationManager.AppSettings[EmailConstants.EmailPassword];

            SmtpClient client = _smtpClientWrapper.StmpClient();
            client.Port = port;
            client.Host = host;
            client.EnableSsl = true;
            client.Timeout = timeout;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(username, password);

            var mailMessage = message;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mailMessage);
        }
    }
}