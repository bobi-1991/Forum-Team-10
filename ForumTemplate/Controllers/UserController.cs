using ForumTemplate.Authorization;
using ForumTemplate.Mappers;
using ForumTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthManager authManager;
        private readonly IUserMapper userMapper;

        public UserController(IAuthManager authManager, IUserMapper userMapper)
        {
            this.authManager = authManager;
            this.userMapper = userMapper;
        }

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
    }
}
