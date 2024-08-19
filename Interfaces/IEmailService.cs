namespace Restoranas.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}