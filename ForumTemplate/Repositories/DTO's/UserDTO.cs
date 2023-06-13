using System.ComponentModel.DataAnnotations;

namespace ForumTemplate.Models
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        //[Required(ErrorMessage = "The {0} field is required")]
        //[Range(1, int.MaxValue, ErrorMessage = "The {0} field must be in the range from {1} to {2}.")]
        //public int PostId { get; set; }
    }
}
