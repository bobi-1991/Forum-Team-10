using ForumTemplate.Models.ViewModels;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService postService;
        private readonly IUserService userService;

        public HomeController(IPostService postService, IUserService userService)
        {
            this.postService = postService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                TopCommentedPosts = postService.GetTopCommentedPosts(10),
                RecentlyCreatedPosts =postService.GetRecentlyCreatedPosts(10),
                TotalPostCount = postService.GetAll().Count(),
                TotalUserCount = userService.GetAll().Count(),
            };

            return View(model);
        }

		[HttpGet]
		public IActionResult About()
		{
			return View();
		}
	}
}
