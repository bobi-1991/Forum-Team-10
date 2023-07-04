using FluentValidation;
using ForumTemplate.DTOs.PostDTOs;

namespace ForumTemplate.DTOs.Validations
{
    public class PostUpdateRequestvalidator : AbstractValidator<PostRequest>
    {
        private const int MinTitleLength = 16;
        private const int MaxTitleLength = 64;

        private const string LengthErrorMessage =
            "{PropertyName} must be between {MinLength} and {MaxLength} characters long. You entered {TotalLength} characters";

        private const int MinContentLength = 32;
        private const int MaxContentLength = 8192;

        public PostUpdateRequestvalidator()
        {
            RuleFor(x => x.Title)
                .Length(MinTitleLength, MaxTitleLength)
                .WithMessage(LengthErrorMessage);

            RuleFor(x => x.Content)
                .Length(MinContentLength, MaxContentLength)
                .WithMessage(LengthErrorMessage);
        }
    }
}
