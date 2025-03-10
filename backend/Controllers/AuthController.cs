using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService;

        public AuthController(TokenService tokenService, UserManager<ApplicationUser> userManager, EmailService emailService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                return Unauthorized("Credenciais inválidas.");

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "User";

            var token = _tokenService.GenerateToken(user.Id, user.Email, userRole);

            return Ok(new { Token = token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _emailService.SendResetPasswordEmailAsync(request.Email, token);

            return Ok(new { Message = "Email de redefinição enviado." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Senha redefinida com sucesso.");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("Usuário não encontrado.");

            bool isValid = await _userManager.VerifyUserTokenAsync(user, "Default", "RefreshToken", request.Token);
            if (!isValid)
                return Unauthorized("Token inválido.");

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "User";

            var token = _tokenService.GenerateToken(user.Id, user.Email, userRole);

            return Ok(new { Token = token });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
