using ForumTemplate.Authorization;
using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.TagService;
using ForumTemplate.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

namespace ForumTemplate.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IPostMapper postMapper;
        private readonly IAuthManager authManager;
        private readonly ITagService tagService;

        public PostsController(IPostService postService, IPostMapper postMapper, IAuthManager authManager, ITagService tagService)
        {
            this.postService = postService;
            this.postMapper = postMapper;
            this.authManager = authManager;
            this.tagService = tagService;
        }

        [HttpGet]
        //  public IActionResult Index([FromQuery] PostQueryParameters postQueryParameters)
        public IActionResult Index([FromQuery] PostQueryParameters parameter)
        {
            var posts = this.postService.SearchBy(parameter);

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
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

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

                if (this.authManager.CurrentUser == null)
                {
                    return this.RedirectToAction("Login", "Auth");
                }

                var user = this.authManager.CurrentUser;
                var post = postMapper.MapToPostRequest(postViewModel, user.UserId);
                var createdPost = postService.Create(user, post);

                var tagRegex = new Regex(@"#(\w+)");
                var matches = tagRegex.Matches(post.Title);
                var tag = "";

                foreach (Match match in matches)
                {
                    tag = "#" + match.Groups[1].Value;
                    break;
                }

                if (tag != "")
                {
                    var tagRequest = new TagRequest
                    {
                        Content = tag,
                        UserId = user.UserId,
                        PostId = createdPost.Id
                    };
                    this.tagService.Create(user, tagRequest);
                }

                return RedirectToAction("Details", "Posts", new { id = createdPost.Id });

            }
            catch (EntityBannedException e)
            {
                this.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }

        }

        [HttpGet]
        public IActionResult Edit([FromRoute] Guid id)
        {
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction(actionName: "Login", controllerName: "Auth");
            }

            try
            {
                var post = postService.GetByPostId(id);

                if (post.UserId != this.authManager.CurrentUser.UserId && !this.authManager.CurrentUser.IsAdmin)
                {
                    this.Response.StatusCode = StatusCodes.Status403Forbidden;
                    this.ViewData["ErrorMessage"] = $"You cannot edit the post since your are not the author.";

                    return this.View("Error");
                }

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

                var user = this.authManager.CurrentUser;
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
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction(actionName: "Login", controllerName: "Auth");
            }

            try
            {
                var post = postService.GetByPostId(id);

                if (post.UserId != this.authManager.CurrentUser.UserId && !this.authManager.CurrentUser.IsAdmin)
                {
                    this.Response.StatusCode = StatusCodes.Status403Forbidden;
                    this.ViewData["ErrorMessage"] = $"You cannot delete the post since your are not the author.";

                    return this.View("Error");
                }

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
            var user = this.authManager.CurrentUser;
            _ = postService.Delete(user, id);

            return RedirectToAction("Index", "Posts");
        }
    }
}
