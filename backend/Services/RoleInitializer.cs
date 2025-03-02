using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class RoleInitializer
{
	public static async Task InitializeRoles(RoleManager<IdentityRole> roleManager)
	{
		string[] roles = { "Admin", "User" };

		foreach (var role in roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new IdentityRole(role));
			}
		}
	}
}
