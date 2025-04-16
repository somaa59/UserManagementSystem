using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;
using UserManagementSystem.Services;
using UserManagementSystem.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagementSystem.Controllers
{
    public class AccountController : Controller
    {
		private readonly IAuthService _authService;
		private readonly IUserService _userService;
		private readonly IJwtService _jwtService;

		public AccountController(IAuthService authService, IUserService userService ,IJwtService jwtService)
        {
            _authService = authService;
            _userService = userService;
			_jwtService = jwtService;
		}
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if(!ModelState.IsValid)
				return View(loginModel);

            var result = await _authService.AuthenticateAsync(loginModel);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error);
				}
				return View(loginModel);
			}
            //store the token in cookie
            Response.Cookies.Append("JwtToken",_jwtService.WriteToken(result.Token),new CookieOptions
            {
                HttpOnly=true,
                Secure=true,
                Expires=loginModel.RememberMe ? result.Token.ValidTo :null
            });
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
                return View(registerModel);
            var result = await _authService.RegisterAsync(registerModel);
       
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
       
			AddErrors(result);
			return View(registerModel);         
        }     
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel verifyEmailModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.FindUserByEmailAsync(verifyEmailModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email is incoorect ! .");
                    return View(verifyEmailModel);
                }else
                {
                    return RedirectToAction("ChangePassword", "Account",new { username = user.UserName });
                }
            }
            return View(verifyEmailModel);

        }
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email=username});
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await _userService.FindUserByEmailAsync(model.Email);
            if(user == null)
            {
				ModelState.AddModelError("", "Email Not Found!");
				return View(model);
			}

            var removeResult = await _authService.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                AddErrors(removeResult);
                return View(model);
            }
			var addResult = await _authService.AddPasswordAsync(user, model.NewPassword);
            if (!addResult.Succeeded)
            {
				AddErrors(removeResult);
				return View(model);				
			}
            return RedirectToAction("Login", "Account");         
        }
        public async Task<IActionResult> Logout()
        {
            //await _authService.LogoutAsync();
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors (IdentityResult result)
        {
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}
    }
}
