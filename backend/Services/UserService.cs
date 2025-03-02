using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using backend.Repositories;

namespace backend.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<IdentityUser?> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<IdentityUser?> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<bool> AddUser(string email, string password, string role = "User")
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null || !IsValidEmail(email) || !IsValidPassword(password))
                return false;

            var identityUser = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
                return false;

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(identityUser, role);
            return true;
        }

        public async Task<bool> UpdateUser(string id, string newEmail)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null || !IsValidEmail(newEmail))
                return false;

            var emailOwner = await _userManager.FindByEmailAsync(newEmail);
            if (emailOwner != null && emailOwner.Id != id)
                return false;

            existingUser.UserName = newEmail;
            existingUser.Email = newEmail;

            var result = await _userManager.UpdateAsync(existingUser);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6;
        }
    }
}
