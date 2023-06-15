namespace ForumTemplate.DTOs.Authentication
{
    public record UserResponse(
       Guid Id,
       string FirstName,
       string LastName,
       string Username,
       string Email,
       DateTime UpdatedDate);

}
