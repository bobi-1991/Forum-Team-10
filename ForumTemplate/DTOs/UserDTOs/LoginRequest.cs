namespace ForumTemplate.DTOs.UserDTOs;

public record LoginRequest(
    string Email,
    string Password);
