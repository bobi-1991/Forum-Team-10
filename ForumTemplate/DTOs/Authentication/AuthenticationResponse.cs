using ForumTemplate.Models;

namespace ForumTemplate.DTOs.Authentication;

public record AuthenticationResponse(
    UserNew User,
    string Token);

