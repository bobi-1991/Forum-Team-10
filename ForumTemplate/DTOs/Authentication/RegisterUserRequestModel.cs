using System.ComponentModel.DataAnnotations;

namespace ForumTemplate.DTOs.Authentication
{
    public class RegisterUserRequestModel
    {
        [MinLength(4, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string FirstName { get; set; }

        [MinLength(4, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9-.]+)@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.)|(([a-zA-Z0-9-]+.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$",
           ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }

        public string Country { get; set; }

    }
}
