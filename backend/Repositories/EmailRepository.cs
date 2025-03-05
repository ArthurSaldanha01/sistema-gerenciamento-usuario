using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using backend.Repositories;

namespace backend.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;

        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendResetPasswordEmailAsync(string toEmail, string token)
        {
            // Recupera as configurações via User Secrets ou appsettings.json
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var smtpPass = _configuration["EmailSettings:SmtpPass"];

            // Monta o link para a redefinição de senha (frontend)
            var resetLink = $"http://localhost:4300/reset-password?email={WebUtility.UrlEncode(toEmail)}&token={WebUtility.UrlEncode(token)}";

            // Corpo do e-mail em HTML, com um botão estilizado para redefinir a senha
            var body = $@"
                        <html>
                          <body style='font-family: Arial, sans-serif; margin: 0; padding: 20px;'>
                            <h2 style='color: #333;'>Redefinição de Senha</h2>
                            <p>Olá,</p>
                            <p>Você solicitou a redefinição de sua senha. Clique no botão abaixo para redefinir:</p>
                            <a href='{resetLink}' 
                               style='display: inline-block; padding: 10px 20px; margin: 20px 0; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 5px;'>
                               Redefinir Senha
                            </a>
                            <p>Se o botão acima não funcionar, copie e cole o link a seguir em seu navegador:</p>
                            <p style='word-break: break-all;'><a href='{resetLink}'>{resetLink}</a></p>
                            <br/>
                            <p>Se você não solicitou a redefinição de senha, ignore este e-mail.</p>
                          </body>
                        </html>
                        ";

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "Redefinição de Senha",
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                client.EnableSsl = true;
                await client.SendMailAsync(message);
            }
        }
    }
}
