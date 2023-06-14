namespace ForumTemplate.DTOs.Authentication;

public record LoginRequest(
    string Email,
    string Password);
