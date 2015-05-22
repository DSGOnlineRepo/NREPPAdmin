using System.Net.Mail;

namespace NREPPAdminSite.Utilities
{
    public interface ISmtpClientWrapper
    {
        SmtpClient StmpClient();
    }
}