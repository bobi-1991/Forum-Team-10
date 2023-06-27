using FluentValidation;
using ForumTemplate.DTOs.UserDTOs;

//namespace ForumTemplate.DTOs.Validations
//{
//    public class LoginRequestValidator : AbstractValidator<LoginRequest>
//    {
//        private const int MinPasswordLength = 8;
//        private const int MaxPasswordLength = 32;
//        private const string PasswordLengthErrorMessage =
//            "{PropertyName} must be between {MinLength} and {MaxLength} characters long. You entered {TotalLength} characters";
//        private const string RequiredErrorMessage = "{PropertyName} is required.";
//        private const string InvalidErrorMessage = "{PropertyName} is invalid. You entered '{PropertyValue}'";

//        public LoginRequestValidator()
//        {
//            RuleFor(x => x.Email)
//                .NotEmpty()
//                .WithMessage(RequiredErrorMessage)
//                .EmailAddress()
//                .WithMessage(InvalidErrorMessage);

//            RuleFor(x => x.Password)
//                .NotEmpty()
//                .WithMessage(RequiredErrorMessage)
//                .Length(MinPasswordLength, MaxPasswordLength)
//                .WithMessage(PasswordLengthErrorMessage);
//        }
//    }
//}
