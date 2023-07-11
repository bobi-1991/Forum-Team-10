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
		private readonly IAuthManager authManager;
		public CommentsController(IPostService postService, IPostMapper postMapper, IAuthManager authManager,ICommentService cpmmentService)
		{
			this.postService = postService;
			this.postMapper = postMapper;
			this.authManager = authManager;
			this.commentService = cpmmentService;
		}

		[HttpGet]
		public IActionResult Create(Guid postId)
		{
			// Създайте модела за създаване на коментари и предайте необходимите данни на изгледа
			var commentViewModel =new CommentViewModel
	        { 
			PostId = postId
			};

			return View(commentViewModel);
		}

		[HttpPost]
		public IActionResult Create(CommentViewModel commentViewModel)
		{
			if (this.authManager.CurrentUser == null)
			{
				return this.RedirectToAction("Login", "Auth");
			}

			if (!this.ModelState.IsValid)
			{
				return this.View(commentViewModel);
			}

			try
			{
				var user = this.authManager.CurrentUser;
				var comment = new CommentRequest(commentViewModel.Content, user.UserId, commentViewModel.PostId);
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
		

		//[HttpPost]
		//public IActionResult Create([FromRoute] Guid id, [FromBody] string content)
		//{
		//	if (this.authManager.CurrentUser == null)
		//	{
		//		return this.RedirectToAction("Login", "Auth");
		//	}

		//	try
		//	{
		//		var post = this.postService.GetByPostId(id);

		//		//if (!this.ModelState.IsValid)
		//		//{
		//		//	return this.View(postViewModel);
		//		//}


		//		var user = this.authManager.CurrentUser;
		//		var comment = new CommentRequest(content, user.UserId, post.PostId);
		//	    var	createdComment = this.commentService.Create(user,comment);

		//		//return RedirectToAction("Details", "Posts", new { id = createdComment.Id });
		//		return RedirectToAction("Details", "Posts");


		//	}
		//	catch (EntityBannedException e)
		//	{
		//		this.Response.StatusCode = StatusCodes.Status403Forbidden;
		//		this.ViewData["ErrorMessage"] = e.Message;

		//		return this.View("Error");
		//	}
		//	catch (ValidationException e)
		//	{
		//		this.Response.StatusCode = StatusCodes.Status404NotFound;
		//		this.ViewData["ErrorMessage"] = e.Message;

		//		return this.View("Error");
		//	}
		//	catch (EntityNotFoundException e)
		//	{
		//		this.Response.StatusCode = StatusCodes.Status404NotFound;
		//		this.ViewData["ErrorMessage"] = e.Message;

		//		return this.View("Error");
		//	}
		//}
	}
}
