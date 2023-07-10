using ForumTemplate.Authorization;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ForumTemplate.Controllers.Api
{

    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {

        private readonly IAuthManager authManager;

        public AuthApiController(IAuthManager authManager)
        {
            this.authManager = authManager;
        }  

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserRequestModel user)
        {
            try
            {
                var message = authManager.TryRegisterUser(user);

                return StatusCode(StatusCodes.Status200OK, message);
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

        [HttpPut("promote")]
        public IActionResult Promote([FromHeader] string credentials, [FromBody] UpdateUserRequestModel userToPromote)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var message = authManager.TryPromoteUser(loggedUser, userToPromote);

                return StatusCode(StatusCodes.Status200OK, message);
            }
            catch (EntityUnauthorizatedException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("demote")]
        public IActionResult Demote([FromHeader] string credentials, [FromBody] UpdateUserRequestModel userToDemote)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var message = authManager.TryDemoteUser(loggedUser, userToDemote);
                return StatusCode(StatusCodes.Status200OK, message);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ban")]
        public IActionResult Ban([FromHeader] string credentials, [FromBody] UpdateUserRequestModel userToBeBanned)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var message = authManager.TryBanUser(loggedUser, userToBeBanned);
                return StatusCode(StatusCodes.Status200OK, message);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("unban")]
        public IActionResult UnBan([FromHeader] string credentials, [FromBody] UpdateUserRequestModel userToBeUnBanned)
        {
            try
            {
                var loggedUser = authManager.TryGetUser(credentials);
                var message = authManager.TryUnBanUser(loggedUser, userToBeUnBanned);
                return StatusCode(StatusCodes.Status200OK, message);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
