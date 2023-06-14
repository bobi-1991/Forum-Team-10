using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;

namespace ForumTemplate.Services.Interfaces;

public interface IAuthenticationService
{
    public AuthenticationResponse Register(RegisterRequest request);
    public AuthenticationResponse Login(LoginRequest request);
}
