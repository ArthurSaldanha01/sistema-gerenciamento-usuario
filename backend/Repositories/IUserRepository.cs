using Microsoft.AspNetCore.Identity;

namespace backend.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllUsers();
        Task<IdentityUser?> GetUserById(string id);
        Task<IdentityUser?> GetUserByEmail(string email);
        Task<bool> AddUser(IdentityUser user, string password);
        Task<bool> UpdateUser(IdentityUser user);
        Task<bool> DeleteUser(string id);
    }
}
