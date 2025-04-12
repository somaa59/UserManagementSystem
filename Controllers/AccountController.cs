using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;
using UserManagementSystem.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagementSystem.Controllers
{
    public class AccountController : Controller
    {
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
			_signInManager = signInManager;
            _userManager = userManager;
		}
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password,loginModel.RememberMe,false);
                if (result.Succeeded)
                {
                   return RedirectToAction("Index", "Home");
                }
                else
                {
					ModelState.AddModelError("","Email Or Password is incoorect ! .");
					return View(loginModel);
				}
            }
            return View(loginModel);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    FullName = registerModel.Name,
                    Email = registerModel.Email,
                    Address= registerModel.Address,
                    UserName = registerModel.Email,
                };

                var result = await _userManager.CreateAsync(user,registerModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }else
                {
					AddErrors(result);
					return View(registerModel);
                }
            }
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
                var user = await _userManager.FindByEmailAsync(verifyEmailModel.Email);
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
			var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
				ModelState.AddModelError("", "Email Not Found!");
				return View(model);
			}

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                AddErrors(removeResult);
                return View(model);
            }
			var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addResult.Succeeded)
            {
				AddErrors(removeResult);
				return View(model);				
			}
            return RedirectToAction("Login", "Account");         
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
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
