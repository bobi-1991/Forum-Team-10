using ForumTemplate.Authorization;
using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Services.CommentService;
using ForumTemplate.Services.TagService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers.Api
{
    [ApiController]
    [Route("api/tags")]
    public class TagApiController : ControllerBase
    {
        private readonly ITagService tagService;

        private readonly IAuthManager authManager;

        public TagApiController(ITagService tagService, IAuthManager authManager)
        {
            this.authManager = authManager;
            this.tagService = tagService;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromHeader] string credentials, Guid id)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var tag = tagService.GetById(id);

                return StatusCode(StatusCodes.Status200OK, tag);
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
        public IActionResult Create([FromHeader] string credentials, [FromBody] TagRequest tag)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var createdTag = tagService.Create(loggedUser, tag);

                return StatusCode(StatusCodes.Status201Created, createdTag);
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

        [HttpDelete("{id}")]
        public IActionResult Delete([FromHeader] string credentials, Guid id)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var result = tagService.Delete(loggedUser, id);
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
