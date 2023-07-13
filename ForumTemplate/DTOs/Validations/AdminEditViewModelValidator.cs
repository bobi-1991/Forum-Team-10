using FluentValidation;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models.ViewModels;

namespace ForumTemplate.DTOs.Validations
{
    public class AdminEditViewModelValidator : AbstractValidator<AdminEditViewModel>
    {     
        private const string RequiredErrorMessage = "{PropertyName} is required.";

        private const string InvalidErrorMessage = "{PropertyName} is invalid. You entered {PropertyValue}";

        public AdminEditViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .EmailAddress()
                .WithMessage(InvalidErrorMessage);
        }
    }
}
