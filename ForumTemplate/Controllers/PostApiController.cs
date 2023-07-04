using ForumTemplate.Authorization;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Services.PostService;
using Microsoft.AspNetCore.Mvc;
using ValidationException = ForumTemplate.Exceptions.ValidationException;

namespace ForumTemplate.Controllers;


[ApiController]
[Route("api/posts")]
public class PostApiController : ControllerBase
{
    private readonly IPostService postService;
    public PostApiController(IPostService postService)
    {
        this.postService = postService;
    }

    [HttpGet()]
    public IActionResult GetAll()
    {
        try
        {
            var response = this.postService.GetAll();
            return StatusCode(StatusCodes.Status200OK, response);
        }
        catch (EntityLoginException e)
        {
            return BadRequest(e.Message);
        }
    }


    // Not tested yet
    //[HttpGet("query")]
    //public IActionResult GetByMultipleCriteria([FromQuery] PostQueryParameters filterParameters)
    //{
    //    List<PostResponse> posts = this.postService.FilterBy(filterParameters);

    //    return this.StatusCode(StatusCodes.Status200OK, posts);
    //}

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
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
        catch (EntityLoginException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost()]
    public IActionResult Create([FromBody] PostRequest postRequest)
    {
        try
        {
            var createdPost = this.postService.Create(postRequest);

            return StatusCode(StatusCodes.Status201Created, postRequest);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityBannedException e)
        {
            return Unauthorized(e.Message);
        }
        catch (EntityLoginException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] PostRequest postRequest)
    {
        try
        {
            var updatedPost = this.postService.Update(id, postRequest);

            return StatusCode(StatusCodes.Status200OK, updatedPost);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityLoginException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
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
        catch (EntityLoginException e)
        {
            return Unauthorized(e.Message);
        }
    }

}
