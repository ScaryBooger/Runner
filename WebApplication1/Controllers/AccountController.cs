using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.context = context;

        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {

                //User is found, check Password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index", "Race");
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Wrong credentials. please, try again";
                return View(loginViewModel);
            }
            //user not found 
            TempData["Error"] = "Wrong credentials plesase try again";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {

            var response = new RegisterViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {


            if (!ModelState.IsValid)
            {
                return View(registerViewModel);

            }
            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "this email address is already in use";
                return View(registerViewModel);
            }
            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,

            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Race");

        }



    }
}
