using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserService _userService;

		public UserController(UserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _userService.GetAllUsers();
			return Ok(users);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserById(int id)
		{
			var user = await _userService.GetUserById(id);
			if (user == null) return NotFound();
			return Ok(user);
		}

		[HttpPost]
		public async Task<IActionResult> AddUser([FromBody] User user)
		{
			var result = await _userService.AddUser(user);
			if (!result) return BadRequest("Usuário já existe.");
			return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
		{
			if (id != user.Id) return BadRequest("ID inconsistente.");

			var result = await _userService.UpdateUser(user);
			if (!result) return NotFound();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var result = await _userService.DeleteUser(id);
			if (!result) return NotFound();
			return NoContent();
		}
	}
}
