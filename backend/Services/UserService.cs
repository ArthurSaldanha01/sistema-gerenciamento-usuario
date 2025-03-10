using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using backend.Repositories;
using backend.Models;
using backend.DTOs;

namespace backend.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<ApplicationUser?> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

 
        public async Task<bool> AddUser(ApplicationUser user, string password, string role = "User")
        {
            Console.WriteLine($"Tentativa de cadastro - Email: {user.Email}, Role: {role}");

            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                Console.WriteLine("Erro: Usuário já existe.");
                return false;
            }

            if (!IsValidEmail(user.Email))
            {
                Console.WriteLine("Erro: E-mail inválido.");
                return false;
            }

            if (!IsValidPassword(password))
            {
                Console.WriteLine("Erro: Senha inválida.");
                return false;
            }

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                Console.WriteLine("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                Console.WriteLine($"Criando novo papel: {role}");
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);
            Console.WriteLine("Usuário cadastrado com sucesso!");
            return true;
        }

 
        public async Task<bool> UpdateUser(string id, UpdateProfileRequest request)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null || !IsValidEmail(request.NewEmail))
                return false;

 
            var emailOwner = await _userManager.FindByEmailAsync(request.NewEmail);
            if (emailOwner != null && emailOwner.Id != id)
                return false;

            existingUser.UserName = request.NewEmail;
            existingUser.Email = request.NewEmail;
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.DateOfBirth = request.DateOfBirth;
            existingUser.Address = request.Address;
            existingUser.Gender = request.Gender;
            existingUser.JobTitle = request.JobTitle;

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
