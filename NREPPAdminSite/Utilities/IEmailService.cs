using System.Net.Mail;

namespace NREPPAdminSite.Utilities
{
    public interface IEmailService
    {
        void SendEmail(MailMessage user);
    }
}