using ForumTemplate.Authorization;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;
using ForumTemplate.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthManager authManager;
        private readonly IUserService userService;


		public AuthController(IAuthManager authManager, IUserService userService)
		{
			this.authManager = authManager;
			this.userService = userService;
		}

		[HttpGet]
        public IActionResult Login()
        {
            var loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
		public IActionResult Login(LoginViewModel loginViewModel)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(loginViewModel);
			}

			try
            {
                this.authManager.Login(loginViewModel.Username, loginViewModel.Password);

                return this.RedirectToAction("Index", "Home");
            }
			catch (EntityUnauthorizatedException e)
			{
                //this.ModelState.AddModelError("Username", e.Message);
                this.ModelState.AddModelError("Password", e.Message);

                return this.View(loginViewModel);

                //HttpContext.Response.StatusCode= StatusCodes.Status401Unauthorized;
                //this.TempData["ErrorMessage"] = e.Message;


                // return this.View(loginViewModel);

            }

		}
		[HttpGet]
		public IActionResult Logout()
		{
            this.authManager.Logout();

            return this.RedirectToAction("Index", "Home");
            //this.HttpContext.Session.Remove("LoggedUser");

            //return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
        //[HttpGet]
        //public IActionResult Register()
        //{
        //    var viewModel = new RegisterViewModel();

        //    return this.View(viewModel);
        //}

        //[HttpPost]
        //public IActionResult Register(RegisterViewModel viewModel)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View(viewModel);
        //    }

        //    if (this.usersService.UsernameExists(viewModel.Username))
        //    {
        //        this.ModelState.AddModelError("Username", "User with same username already exists.");

        //        return this.View(viewModel);
        //    }

        //    if (viewModel.Password != viewModel.ConfirmPassword)
        //    {
        //        this.ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");

        //        return this.View(viewModel);
        //    }

        //    User user = this.modelMapper.Map(viewModel);
        //    this.usersService.Create(user);

        //    return this.RedirectToAction("Login", "Users");
        //}
    }
}
