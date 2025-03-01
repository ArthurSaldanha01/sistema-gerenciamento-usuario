using backend.Models;

namespace backend.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> GetUserById(int id);
        Task AddUser(User users);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}


