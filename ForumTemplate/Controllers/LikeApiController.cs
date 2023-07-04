using ForumTemplate.Exceptions;
using ForumTemplate.Services.LikeService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    [ApiController]
    [Route("api/like")]

    public class LikeApiController : ControllerBase
    {
        private readonly ILikeService likeService;

        public LikeApiController(ILikeService likeService)
        {
            this.likeService = likeService;
        }

        [HttpPost("{postId}")]
        public IActionResult LikeUnlike(Guid postId)
        {
            try
            {
                var result = likeService.LikeUnlike(postId);
                return Ok(result);
            }
            catch (EntityLoginException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
