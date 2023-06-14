using ForumTemplate.Models;

namespace ForumTemplate.DTOs.Authentication;

public record AuthenticationResponse(
    string Id,
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Token);

