using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ForumTemplate.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            // TODO: Map LogqinRequest to LoginQuery

            var response = authenticationService.Login(request);

            // TODO: Handle errors

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            // TODO: Map RegisterRequest to RegisterCommand

            var response = authenticationService.Register(request);

            // TODO: Handle errors

            return CreatedAtAction(nameof(Register),response);
        }
    }
}
