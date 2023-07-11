using ForumTemplate.Authorization;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
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
        private readonly IUserMapper userMapper;


		public AuthController(IAuthManager authManager, IUserService userService, IUserMapper userMapper)
		{
			this.authManager = authManager;
			this.userService = userService;
			this.userMapper = userMapper;
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

                this.ModelState.AddModelError("Password", e.Message);

                return this.View(loginViewModel);
            }

		}
		[HttpGet]
		public IActionResult Logout()
		{
            this.authManager.Logout();

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var viewModel = new RegisterViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            if (this.userService.UsernameExists(viewModel.Username))
            {
                this.ModelState.AddModelError("Username", "User with same username already exists.");

                return this.View(viewModel);
            }

            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                this.ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");

                return this.View(viewModel);
            }

            try
            {
                var user = this.userMapper.MapToRegisterUserRequestModel(viewModel);
                string encodedPassword = this.authManager.EncodePassword(user.Password);
                this.userService.RegisterUser(user, encodedPassword);
                this.authManager.Login(user.Username, user.Password);

                this.TempData["SuccessMessage"] = "Registration successfully";

                return this.RedirectToAction("Index", "Home");
            }
            catch (DuplicateEntityException e)
            {
                this.ModelState.AddModelError("Email", e.Message);

                return this.View(viewModel);
            }
        }
        
    }
}
