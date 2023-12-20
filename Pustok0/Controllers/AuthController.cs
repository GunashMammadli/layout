using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok0.Helpers;
using Pustok0.Models;
using Pustok0.ViewModels.AuthVM;

namespace Pustok0.Controllers
{
    public class AuthController : Controller
    {
		SignInManager<AppUser> _signInManager { get; }
		UserManager<AppUser> _userManager { get; }
		RoleManager<IdentityRole> _roleManager { get; }

		public AuthController(SignInManager<AppUser> signInManager,
			   UserManager<AppUser> userManager,
			   RoleManager<IdentityRole> roleManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(string? returnUrl, LoginVM vm)
		{
			AppUser user;
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			if (vm.UsernameOrEmail.Contains("@"))
			{
				user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
			}
			else
			{
				user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
			}
			if (user == null)
			{
				ModelState.AddModelError("", "Username or password is wrong");
				return View(vm);
			}
			var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
			if (!result.Succeeded)
			{
				if (result.IsLockedOut)
				{
					ModelState.AddModelError("", "Too many attempts please wait 10 minutes");
				}
				else
				{
					ModelState.AddModelError("", "Username or password is wrong");
				}
				return View(vm);
			}
			if (returnUrl != null)
			{
				return LocalRedirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}
		public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = new AppUser
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(vm);
            }
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<bool> CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString()
                    });
                    if (!result.Succeeded)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public async Task<IActionResult> ChangeInfos()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ChangeInfosVM vm = new ChangeInfosVM();
            if (user != null)
            {
                vm = new ChangeInfosVM()
                {
                    Fullname = user.Fullname,
                    Username = user.UserName,
                    Email = user.Email,
                };
            }

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeInfos(ChangeInfosVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if(User.Identity.Name == null) return NotFound();
         
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.Email = vm.Email;
            user.UserName = vm.Username;
           
            await _userManager.UpdateAsync(user);
            
            return View();
        }
    }
}
