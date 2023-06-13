using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Models.Input;
using ForumTemplate.Repositories.DTO_s;
using ForumTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentApiController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet()]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = this.commentService.GetAll();

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityNotFoundException e)
            {

                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var comment = this.commentService.GetById(id);

                return StatusCode(StatusCodes.Status200OK, comment);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost()]
        [Route("Create")]
        public IActionResult Create([FromBody] CommentInputModel comment)
        {
            try
            {
                var createdComment = this.commentService.Create(comment);

                return StatusCode(StatusCodes.Status201Created, comment);
            }
            catch (DuplicateEntityException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] CommentInputModel comment)
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
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
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
        }

    }
}
