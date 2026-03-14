namespace Umre.Web.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string host, int port, string user, string password, string toEmail, string subject, string body);
    }
}
