using ForumTemplate.Common.FilterModels;
using ForumTemplate.DTOs.PostDTOs;
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
        var response = this.postService.GetAll();

        return StatusCode(StatusCodes.Status200OK, response);
    }


    //   Not tested yet
    [HttpGet("")]
    public IActionResult GetByFiler([FromQuery] PostQueryParameters filterParameters)
    {
        List<PostResponse> posts = this.postService.FilterBy(filterParameters);

        return this.StatusCode(StatusCodes.Status200OK, posts);
    }

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
    }

}
