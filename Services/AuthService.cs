using Microsoft.AspNetCore.Identity;
using UserManagementSystem.Models;
using UserManagementSystem.ViewModels;

namespace UserManagementSystem.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
			_signInManager = signInManager;
        }
		public async Task<SignInResult> LoginAsync(LoginViewModel loginModel)
		{
			return await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);
		}
		public async Task<IdentityResult> RegisterAsync(RegisterViewModel registerModel)
		{
			ApplicationUser user = new ApplicationUser
			{
				FullName = registerModel.Name,
				Email = registerModel.Email,
				Address = registerModel.Address,
				UserName = registerModel.Email,
			};
			return await _userManager.CreateAsync(user,registerModel.Password);
		}

		public Task<IdentityResult> RemovePasswordAsync(ApplicationUser user)
		{
			return _userManager.RemovePasswordAsync(user);
		}
		public async Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
		{
			return await _userManager.AddPasswordAsync(user, password);
		}
		public async Task LogoutAsync()
		{
			await _signInManager.SignOutAsync();
		}

	}
}
