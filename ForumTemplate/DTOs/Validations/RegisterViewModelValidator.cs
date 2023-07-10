using FluentValidation;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models.ViewModels;

namespace ForumTemplate.DTOs.Validations
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        private const int MinNameLength = 4;
        private const int MaxNameLength = 32;

        private const int MinPasswordLength = 8;
        private const string PasswordLengthErrorMessage = "{PropertyName} must be at least {MinLength} characters long. You entered {TotalLength} characters";

        private const string PasswordRegex = @"^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[A-Z])(?=.*[a-z]).*$";
        private const string PasswordRegexErrorMessage = "{PropertyName} must contain at least one uppercase letter, one lowercase letter and one digit. You entered {PropertyValue}";

        private const string RequiredErrorMessage = "{PropertyName} is required.";
        private const string LengthErrorMessage = "{PropertyName} must be between {MinLength} and {MaxLength} characters long. You entered {TotalLength} characters";
        private const string InvalidErrorMessage = "{PropertyName} is invalid. You entered {PropertyValue}";

        public RegisterViewModelValidator()
        {

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(LengthErrorMessage);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(LengthErrorMessage);

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .Length(MinNameLength, MaxNameLength)
                .WithMessage(LengthErrorMessage);


            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .EmailAddress()
                .WithMessage(InvalidErrorMessage);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .MinimumLength(MinPasswordLength)
                .WithMessage(PasswordLengthErrorMessage)
                .Matches(PasswordRegex)
                .WithMessage(PasswordRegexErrorMessage);

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage);
        }
    }
}
