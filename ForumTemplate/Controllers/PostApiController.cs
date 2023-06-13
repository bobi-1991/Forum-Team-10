using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Models.Input;
using ForumTemplate.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{

    [ApiController]
    [Route("api/post")]
    public class PostApiController : ControllerBase
    {
        private readonly IPostService postService;
        public PostApiController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet()]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var result = this.postService.GetAll();

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var post = this.postService.GetById(id);

                return StatusCode(StatusCodes.Status200OK, post);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost()]
        [Route("Create")]
        public IActionResult Create([FromBody] PostInputModel post)
        {
            try
            {
                var createdPost = this.postService.Create(post);

                return StatusCode(StatusCodes.Status201Created, post);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] PostInputModel post)
        {
            try
            {
                var updatedPost = this.postService.Update(id, post);

                return StatusCode(StatusCodes.Status200OK, updatedPost);
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
                var result = this.postService.Delete(id);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
