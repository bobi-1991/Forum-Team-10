using ForumTemplate.Authorization;
using ForumTemplate.Services.LikeService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    public class LikesController : Controller
    {
        private readonly ILikeService likeService;
        private readonly IAuthManager authManager;

        public LikesController(IAuthManager authManager, ILikeService likeService)
        {

            this.authManager = authManager;
            this.likeService = likeService;
        }

        [HttpPost]
        public IActionResult Like(Guid postId)
        {
            // Проверка дали потребителят е логнат
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            // Извикване на сервизния метод за лайкване на поста
            var user = this.authManager.CurrentUser;
            likeService.LikeUnlike(user,postId);

            // Пренасочване към детайлите на поста
            return RedirectToAction("Details", "Posts", new { id = postId });
        }
    }
}
