using Microsoft.AspNetCore.Identity;
using UserManagementSystem.Models;
using UserManagementSystem.ViewModels;

namespace UserManagementSystem.Services
{
	public interface IAuthService
	{
		//Task<SignInResult>LoginAsync(LoginViewModel loginModel);
		Task<AuthResult> AuthenticateAsync(LoginViewModel model);
		Task<IdentityResult>RegisterAsync(RegisterViewModel registerModel);
		Task<IdentityResult> RemovePasswordAsync(ApplicationUser user);
		Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password);
		Task LogoutAsync();
	}
}
