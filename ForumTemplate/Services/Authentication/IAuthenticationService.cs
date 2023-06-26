using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;

namespace ForumTemplate.Services.Authentication;

public interface IAuthenticationService
{
    public AuthenticationResponse Register(RegisterRequest request);
    public AuthenticationResponse Login(LoginRequest request);
}
