using ForumTemplate.Authorization;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net;

namespace ForumTemplate.Controllers.Api
{
    [ApiController]
    [Route("api/users")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthManager authManager;

        public UserApiController(IUserService userService, IAuthManager authManager)
        {
            this.userService = userService;
            this.authManager = authManager;
        }

        //Admin Access Only
        [HttpGet()]
        [Route("")]
        public IActionResult GetAll([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                List<UserResponse> result = userService.GetAll();

                return StatusCode(StatusCodes.Status200OK, result);
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

        //Admin - to see id
        //All - for all others
        [HttpGet("{id}")]
        public IActionResult GetById([FromHeader] string credentials, Guid id)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                UserResponse user = userService.GetById(id);

                return StatusCode(StatusCodes.Status200OK, user);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
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

        //All users
        //If user wants to update - check is target username matches the currently logged user
        //If Admin - he will have access to all by ID or Username
        [HttpPut("{id}")]
        public IActionResult Update([FromHeader] string credentials, Guid id, [FromBody] UpdateUserRequest user)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                UserResponse updatedUser = userService.Update(loggedUser, id, user);

                return StatusCode(StatusCodes.Status200OK, updatedUser);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityLoginException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        //All users
        //If user wants to update - check is target username matches the currently logged user
        //If Admin - he will have access to all by ID or Username
        [HttpDelete("{id}")]
        public IActionResult Delete([FromHeader] string credentials, Guid id)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var result = userService.Delete(loggedUser, id);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityLoginException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}