using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserManagementSystem.Models;
using UserManagementSystem.ViewModels;

namespace UserManagementSystem.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IUserService _userService;
		private readonly IJwtService _jwtService;

		public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , IUserService userService ,IJwtService jwtService)
        {
            _userManager = userManager;
			_signInManager = signInManager;
			_userService = userService;
			_jwtService = jwtService;
		}
		#region Login Using cookie
		//public async Task<SignInResult> LoginAsync(LoginViewModel loginModel)
		//{
		//	return await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);
		//}
		#endregion

		#region Login Using JWT
		public async Task<AuthResult> AuthenticateAsync(LoginViewModel model)
		{
			//check
			var user = await _userService.FindUserByEmailAsync(model.Email);
			if (user == null || !await _userManager.CheckPasswordAsync(user ,model.Password)) {
				return new AuthResult { 
					Succeeded = false,
					Errors = new[] {"Invalid Email Or Password"}
				};
			}
			//create claims
			var claims = await GetUserClaimsAsync(user);
			//Generate Token
			var token = _jwtService.GenerateToken(claims);
			return new AuthResult { 
				Succeeded=true,
				Token = token,
				User = user,
			};
		}

		#endregion
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
		
		private  async  Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
		{
			var UserClaims = new List<Claim> { 
				new Claim(ClaimTypes.NameIdentifier,user.Id),
				new Claim(ClaimTypes.Name,user.FullName),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
			};

			var roles = await _userManager.GetRolesAsync(user);
			UserClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			return UserClaims;
		}
	}	
}
