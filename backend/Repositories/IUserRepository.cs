using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser?> GetUserById(string id);
        Task<ApplicationUser?> GetUserByEmail(string email);
        Task<bool> AddUser(ApplicationUser user, string password);
        Task<bool> UpdateUser(ApplicationUser user);
        Task<bool> DeleteUser(string id);
    }
}
