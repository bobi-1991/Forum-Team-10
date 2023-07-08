using ForumTemplate.Authorization;
using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Services.CommentService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers.Api
{

    [ApiController]
    [Route("api/comments")]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly IAuthManager authManager;

        public CommentApiController(ICommentService commentService, IAuthManager authManager)
        {
            this.commentService = commentService;
            this.authManager = authManager;
        }

        [HttpGet()]
        [Route("")]
        public IActionResult GetAll([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var result = commentService.GetAll();

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityLoginException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromHeader] string credentials, Guid postId)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var comment = commentService.GetById(postId);

                return StatusCode(StatusCodes.Status200OK, comment);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
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
        public IActionResult Create([FromHeader] string credentials, [FromBody] CommentRequest comment)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var createdComment = commentService.Create(loggedUser, comment);

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
        public IActionResult Update([FromHeader] string credentials, Guid id, [FromBody] CommentRequest comment)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var updatedComment = commentService.Update(loggedUser, id, comment);
                return StatusCode(StatusCodes.Status200OK, updatedComment);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
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
        public IActionResult Delete([FromHeader] string credentials, Guid id)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var result = commentService.Delete(loggedUser, id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
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
