using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

                var user = userService.GetByUsername("admin");
                var post = postMapper.MapToPostRequest(postViewModel, user.UserId);
                var createdPost = postService.Create(user,post);

                return RedirectToAction("Details", "Posts", new { id = createdPost.Id});

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
            //if (!this.ModelState.IsValid)
            //{
            //    this.InitializeDropDownLists(beerViewModel);
            //    return this.View(beerViewModel);
            //}

            //try
            //{
            //    var user = this.authManager.TryGetUser("admin");
            //    var beer = this.modelMapper.Map(beerViewModel);
            //    var createdBeer = this.beersService.Create(beer, user);

            //    return this.RedirectToAction("Details", "Beers", new { id = createdBeer.Id });
            //}
            //catch (DuplicateEntityException ex)
            //{
            //    this.ModelState.AddModelError("Name", ex.Message);
            //    this.InitializeDropDownLists(beerViewModel);
            //    return this.View(beerViewModel);
            //}

           
        }


    }
}
