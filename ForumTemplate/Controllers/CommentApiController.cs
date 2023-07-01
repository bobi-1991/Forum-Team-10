using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Services.CommentService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    
    [ApiController]
    [Route("api/comments")]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentApiController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet()]
        [Route("")]
        public IActionResult GetAll()
        {
            try
            {
                var result = this.commentService.GetAll();

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityLoginException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid postId)
        {
            try
            {
                var comment = this.commentService.GetById(postId);

                return StatusCode(StatusCodes.Status200OK, comment);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
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

        [HttpPost()]
        [Route("")]
        public IActionResult Create([FromBody] CommentRequest comment)
        {
            try
            {
                var createdComment = this.commentService.Create(comment);

                return StatusCode(StatusCodes.Status201Created, comment);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityLoginException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityBannedException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CommentRequest comment)
        {
            try
            {
                var updatedComment = this.commentService.Update(id, comment);
                return StatusCode(StatusCodes.Status200OK, updatedComment);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
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

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var result = this.commentService.Delete(id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityLoginException e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
