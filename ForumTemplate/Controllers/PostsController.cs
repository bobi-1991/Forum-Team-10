using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace ForumTemplate.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly IPostMapper postMapper;

        public PostsController(IPostService postService, IUserService userService, IPostMapper postMapper)
        {
            this.postService = postService;
            this.userService = userService;
            this.postMapper = postMapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Post> posts = this.postService.GetAllPosts();

            return this.View(posts);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            try
            {
                var post = this.postService.GetByPostId(id);

                return this.View(post);
            }
            catch (ValidationException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var postViewModel = new PostViewModel();
            return this.View(postViewModel);
        }

        [HttpPost]
        public IActionResult Create(PostViewModel postViewModel)
        {

            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(postViewModel);
                }
                // Warning: We bypass authentication and authorization just for this moment
                var user = userService.GetByUsername("admin");
                var post = postMapper.MapToPostRequest(postViewModel, user.UserId);
                var createdPost = postService.Create(user, post);

                return RedirectToAction("Details", "Posts", new { id = createdPost.Id });

            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
            catch (ValidationException e)
            {
                this.Response.StatusCode = StatusCodes.Status400BadRequest;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
            catch (EntityBannedException e)
            {
                this.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] Guid id)
        {
            try
            {
                var post = postService.GetByPostId(id);
                var postViewModel = postMapper.MapToPostViewModel(post);

                return View(postViewModel);

            }
            catch (ValidationException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] Guid id, PostViewModel postViewModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(postViewModel);
                }
                // Warning: We bypass authentication and authorization just for this moment
                var user = userService.GetByUsername("admin");
                var post = postMapper.MapToPostRequest(postViewModel, user.UserId);
                var updatedPost = postService.Update(user, id, post);
                var updatedPostViewModel = postMapper.MapToPostViewModel(updatedPost);

                return RedirectToAction("Details", "Posts", new { id = updatedPost.Id });

            }
            catch (ValidationException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                var post = postService.GetByPostId(id);
                var postViewModel = postMapper.MapToPostViewModel(post);

                return View(postViewModel);

            }
            catch (ValidationException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed([FromRoute] Guid id)
        {
            // Warning: We bypass authentication and authorization just for this moment
            var user = userService.GetByUsername("admin");
            _ = postService.Delete(user, id);

            return RedirectToAction("Index", "Posts");
        }
    }
}
