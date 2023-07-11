using ForumTemplate.Authorization;
using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;
using ForumTemplate.Services.CommentService;
using ForumTemplate.Services.PostService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ForumTemplate.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IPostService postService;
        private readonly ICommentService commentService;
        private readonly IPostMapper postMapper;
        private readonly ICommentMapper commentMapper;
        private readonly IAuthManager authManager;
        public CommentsController(IPostService postService, IPostMapper postMapper, IAuthManager authManager, ICommentService cpmmentService, ICommentMapper commentMapper)
        {
            this.postService = postService;
            this.postMapper = postMapper;
            this.authManager = authManager;
            this.commentService = cpmmentService;
            this.commentMapper = commentMapper;
        }

        [HttpGet]
        public IActionResult Create(Guid id)
        {
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var commentViewModel = new CommentViewModel
            {
                Id = id
            };

            return View(commentViewModel);
        }

        [HttpPost]
        public IActionResult Create([FromRoute]Guid id, CommentViewModel commentViewModel)
        {
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(commentViewModel);
            }

            var viewModel = new CommentViewModel
            {
                Id = id
            };

            try
            {
                var user = this.authManager.CurrentUser;
                var comment = new CommentRequest
                {
                    Content = commentViewModel.Content,
                    UserId = user.UserId,
                    PostId = commentViewModel.Id
                };
                var createdComment = this.commentService.Create(user, comment);

                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
                //return RedirectToAction("Details", "Posts");
            }
            catch (EntityBannedException e)
            {
                this.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
            catch (ValidationException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
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
        public IActionResult Edit(Guid id)
        {
            if (this.authManager.CurrentUser == null)
            {
                return this.RedirectToAction("Login", "Auth");
            }

            var comment = this.commentService.GetById(id);
            var commentViewModel = new CommentViewModel
            {
                Id = comment.PostId
            };

            return View(commentViewModel);
        }

        [HttpPost]
        public IActionResult Edit(CommentViewModel commentViewModel)
        {
            try
            {
                var user = this.authManager.CurrentUser;
                var commentResponse = this.commentService.GetById(commentViewModel.Id);
                var commentRequest = this.commentMapper.MapToCommentRequest(commentResponse);
                commentRequest.Content = commentViewModel.Content;
                var comment = this.commentService.Update(user,commentViewModel.Id, commentRequest);

                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            catch (ValidationException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
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
    }
}
