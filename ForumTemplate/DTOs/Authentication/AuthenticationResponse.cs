using ForumTemplate.Models;

namespace ForumTemplate.DTOs.Authentication;

public record AuthenticationResponse(
    User User,
    string Token);

