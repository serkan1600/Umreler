using System.Net;
using System.Net.Mail;

namespace Umre.Web.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string host, int port, string user, string password, string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(user, password);
                
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(user),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
