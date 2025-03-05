using System.Threading.Tasks;

namespace backend.Repositories
{
    public interface IEmailRepository
    {
        Task SendResetPasswordEmailAsync(string toEmail, string token);
    }
}
