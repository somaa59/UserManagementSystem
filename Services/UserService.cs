using Microsoft.AspNetCore.Identity;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserService(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}
		public async Task<ApplicationUser> FindUserByEmailAsync(string email)
				=> await _userManager.FindByEmailAsync(email);

		public async Task<ApplicationUser> FindUserByNameAsync(string name)
				=> await _userManager.FindByNameAsync(name);

	}
}
