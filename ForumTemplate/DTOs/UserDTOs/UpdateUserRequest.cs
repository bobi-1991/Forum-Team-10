using System.ComponentModel.DataAnnotations;

namespace ForumTemplate.DTOs.UserDTOs
{
    public class UpdateUserRequest
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; } 
        public string? Password { get; set; }
        public string? Email { get; set; } 
        public string? Country { get; set; } 

    }
}
