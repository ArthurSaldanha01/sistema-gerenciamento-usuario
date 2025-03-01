using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<bool> AddUser(User user)
        {
            var existingUser = await _userRepository.GetUserById(user.Id);
            if (existingUser != null) return false;

            await _userRepository.AddUser(user);
            return true;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await _userRepository.GetUserById(user.Id);
            if (existingUser == null) return false;

            await _userRepository.UpdateUser(user);
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return false;

            await _userRepository.DeleteUser(id);
            return true;
        }
    }
}
