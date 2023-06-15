using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService userService;

        public UserApiController(IUserService userService)
        {
            this.userService = userService;
        }

        //Admin Access Only
        [HttpGet()]
        [Route("")]
        public IActionResult GetAll()
        {
            List<UserResponse> result = this.userService.GetAll();

            return StatusCode(StatusCodes.Status200OK, result);
        }

        //Admin - to see id
        //All - for all others
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                UserResponse user = this.userService.GetById(id);

                return StatusCode(StatusCodes.Status200OK, user);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        //All users
        [HttpPost()]
        [Route("")]
        public IActionResult Create([FromBody] RegisterRequest user)
        {
            try
            {
                UserResponse createdUser = this.userService.Create(user);

                return StatusCode(StatusCodes.Status201Created, user);
            }
            catch (DuplicateEntityException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
        }

        //All users
        //If user wants to update - check is target username matches the currently logged user
        //If Admin - he will have access to all by ID or Username
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] RegisterRequest user)
        {
            try
            {
                UserResponse updatedUser = this.userService.Update(id, user);

                return StatusCode(StatusCodes.Status200OK, updatedUser);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        //To Create Ban User Endpoint


        //All users
        //If user wants to update - check is target username matches the currently logged user
        //If Admin - he will have access to all by ID or Username
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var result = this.userService.Delete(id);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}