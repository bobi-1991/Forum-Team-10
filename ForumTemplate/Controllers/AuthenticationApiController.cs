using ForumTemplate.Authorization;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{

    [ApiController]
    [Route("api")]
    public class AuthenticationApiController : ControllerBase
    {

        private readonly IAuthManager authManager;

        public AuthenticationApiController(IAuthManager authManager)
        {
            this.authManager = authManager;
        }

        [HttpGet("login")]
        public IActionResult Login([FromHeader] string credentials)
        {
            try
            {
                var message = this.authManager.TrySetCurrentLoggedUser(credentials);

                return StatusCode(StatusCodes.Status200OK, message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout([FromHeader] string username)
        {
            try
            {
                var message = this.authManager.LogoutUser(username);

                return StatusCode(StatusCodes.Status200OK, message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserRequestModel user)
        {
            try
            {
                var message = this.authManager.TryRegisterUser(user);

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
        public IActionResult Promote([FromHeader] string username, [FromBody] PromoteUserRequestModel userToPromote)
        {
            try
            {
                var message = this.authManager.TryPromoteUser(username, userToPromote);

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
