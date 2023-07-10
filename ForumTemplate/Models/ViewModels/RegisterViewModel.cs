using ForumTemplate.DTOs.Authentication;

namespace ForumTemplate.Models.ViewModels
{
    public class RegisterViewModel:RegisterUserRequestModel
    {
        public string ConfirmPassword { get; set;}
    }
}
