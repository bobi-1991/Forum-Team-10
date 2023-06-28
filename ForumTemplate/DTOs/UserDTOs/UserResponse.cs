namespace ForumTemplate.DTOs.UserDTOs
{
    public record UserResponse(
       Guid Id,
       string FirstName,
       string LastName,
       string Country,
       string Username,
       string Email,
       DateTime UpdatedDate);

}
