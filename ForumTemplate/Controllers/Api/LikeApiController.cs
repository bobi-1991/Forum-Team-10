using ForumTemplate.Authorization;
using ForumTemplate.Exceptions;
using ForumTemplate.Services.LikeService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers.Api
{
    [ApiController]
    [Route("api/like")]

    public class LikeApiController : ControllerBase
    {
        private readonly ILikeService likeService;
        private readonly IAuthManager authManager;

        public LikeApiController(ILikeService likeService, IAuthManager authManager)
        {
            this.likeService = likeService;
            this.authManager = authManager;
        }

        [HttpPost("{postId}")]
        public IActionResult LikeUnlike([FromHeader] string credentials, Guid postId)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var result = likeService.LikeUnlike(loggedUser, postId);
                return Ok(result);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityLoginException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ValidationException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
