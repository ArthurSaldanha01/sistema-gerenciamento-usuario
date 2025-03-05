using System.Threading.Tasks;
using backend.Repositories;

namespace backend.Services
{
    public class EmailService
    {
        private readonly IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public async Task SendResetPasswordEmailAsync(string toEmail, string token)
        {
            await _emailRepository.SendResetPasswordEmailAsync(toEmail, token);
        }
    }
}
