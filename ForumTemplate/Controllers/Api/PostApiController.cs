using ForumTemplate.Authorization;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Services.PostService;
using Microsoft.AspNetCore.Mvc;
using ValidationException = ForumTemplate.Exceptions.ValidationException;

namespace ForumTemplate.Controllers.Api;


[ApiController]
[Route("api/posts")]
public class PostApiController : ControllerBase
{
    private readonly IPostService postService;
    private readonly IAuthManager authManager;
    public PostApiController(IPostService postService, IAuthManager authManager)
    {
        this.postService = postService;
        this.authManager = authManager;
    }

    [HttpGet()]
    public IActionResult GetAll([FromHeader] string credentials)
    {
        try
        {
            var loggedUser = authManager.TryGetUser(credentials);
            var response = postService.GetAll();
            return StatusCode(StatusCodes.Status200OK, response);
        }
        catch (EntityUnauthorizatedException e)
        {
            return Unauthorized(e.Message);
        }
        catch (EntityLoginException e)
        {
            return BadRequest(e.Message);
        }
    }

    //  Not tested yet
    [HttpGet("query")]
    public IActionResult GetByMultipleCriteria([FromHeader] string credentials, [FromQuery] PostQueryParameters filterParameters)
    {
        try
        {
            var loggedUser = authManager.TryGetUser(credentials);
            List<PostResponse> posts = postService.FilterBy(filterParameters);

            return StatusCode(StatusCodes.Status200OK, posts);
        }
        catch (EntityUnauthorizatedException e)
        {
            return Unauthorized(e.Message);
        }

    }

    [HttpGet("{id}")]
    public IActionResult Get([FromHeader] string credentials, Guid id)
    {
        try
        {
            var loggedUser = authManager.TryGetUser(credentials);
            var post = postService.GetById(id);

            return StatusCode(StatusCodes.Status200OK, post);
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
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost()]
    public IActionResult Create([FromHeader] string credentials, [FromBody] PostRequest postRequest)
    {
        try
        {
            var loggedUser = authManager.TryGetUser(credentials);
            var createdPost = postService.Create(loggedUser, postRequest);

            return StatusCode(StatusCodes.Status201Created, postRequest);
        }
        catch (EntityUnauthorizatedException e)
        {
            return Unauthorized(e.Message);
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
    public IActionResult Update([FromHeader] string credentials, Guid id, [FromBody] PostRequest postRequest)
    {
        try
        {
            var loggedUser = authManager.TryGetUser(credentials);
            var updatedPost = postService.Update(loggedUser, id, postRequest);

            return StatusCode(StatusCodes.Status200OK, updatedPost);
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
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromHeader] string credentials, Guid id)
    {
        try
        {
            var loggedUser = authManager.TryGetUser(credentials);
            var result = postService.Delete(loggedUser, id);

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
            return Unauthorized(e.Message);
        }
    }

}
