using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;
using ForumTemplate.Services.Interfaces;
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
            var response = this.authenticationService.Login(request);

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            var response = this.authenticationService.Register(request);

            return Ok(response);
        }
    }
}
