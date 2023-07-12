using ForumTemplate.Authorization;
using ForumTemplate.Mappers;
using ForumTemplate.Models.ViewModels;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ForumTemplate.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthManager authManager;
        private readonly IUserMapper userMapper;
        private readonly IUserService userService;

		public UsersController(IAuthManager authManager, IUserMapper userMapper, IUserService userService)
		{
			this.authManager = authManager;
			this.userMapper = userMapper;
			this.userService = userService;
		}

		[HttpGet]
        public IActionResult Index()
        {
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var user = authManager.CurrentUser;
            var userViewModel = userMapper.MapToUserViewModel(user);

            return View(userViewModel);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var user = authManager.CurrentUser;
            var userEditModel = userMapper.MapToUserEditViewModel(user);

            return View(userEditModel);
        }

		[HttpPost]
		public IActionResult Edit(UserEditViewModel userEditModel)
		{
			try
			{
				if (!this.ModelState.IsValid)
				{
					return this.View(userEditModel);
				}

				var user = this.authManager.CurrentUser;
				var userInfoForUpdate = userMapper.MapToUpdateUserRequest(userEditModel);
				var updatedUser = userService.Update(user, user.UserId, userInfoForUpdate);

				return RedirectToAction("Index", "Users");

			}
			catch (ValidationException e)
			{
				this.Response.StatusCode = StatusCodes.Status404NotFound;
				this.ViewData["ErrorMessage"] = e.Message;

				return View("Error");
			}
		}
	}
}
