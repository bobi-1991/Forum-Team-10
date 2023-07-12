﻿using ForumTemplate.Authorization;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models.ViewModels;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.UserService;
using ForumTemplate.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ForumTemplate.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthManager authManager;
        private readonly IUserMapper userMapper;
        private readonly IUserService userService;
        private readonly IUserAuthenticationValidator uservalidator;

		public UsersController(IAuthManager authManager, IUserMapper userMapper, IUserService userService, IUserAuthenticationValidator uservalidator)
		{
			this.authManager = authManager;
			this.userMapper = userMapper;
			this.userService = userService;
			this.uservalidator = uservalidator;
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


				if (userEditModel.Password != userEditModel.ConfirmPassword)
				{
					this.ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");

					return this.View(userEditModel);
				}

				var user = this.authManager.CurrentUser;
				var userInfoForUpdate = userMapper.MapToUpdateUserRequest(userEditModel);
				this.userService.ValidateUpdatedUserEmail(user, userInfoForUpdate.Email);
				var updatedUser = userService.Update(user, user.UserId, userInfoForUpdate);
				this.userService.ValidateUpdatedUserEmail(user, updatedUser.Email);

				return RedirectToAction("Index", "Users");
			}
			catch (DuplicateEntityException e)
			{
				this.ModelState.AddModelError("Email", "This email is already exist.");

				return this.View(userEditModel);
			}
		}

        [HttpGet]
        public IActionResult Tools()
        {
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var users = this.userService.GetAllUsers();

            return View(users);
        }

        [HttpGet]
        public IActionResult Info(Guid id)
        {
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var user = this.userService.GetByUserId(id);
            var userViewModel = this.userMapper.MapToUserViewModel(user);
            this.ViewBag.UserId = id;
            return View(userViewModel);
        }

        [HttpGet]
        public IActionResult AdminEdit(Guid id)
        {
            try
            {
                var loggeduser = this.authManager.CurrentUser;

                var user = userService.GetByUserId(id);
                var editModel = this.userMapper.MapToAdminEditUserModel(user);
              
                return this.View(editModel);
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
        }

        [HttpPost]
        public IActionResult AdminEdit(AdminEditViewModel adminEditViewModel)
        {
            try
            {
                var loggeduser = this.authManager.CurrentUser;

              //  var user = userService.GetByUserId(id);


                return RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
            catch (ArgumentException e)
            {
                this.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
        }
    }
}
