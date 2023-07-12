using FluentValidation;
using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.DTOs.TagDTOs;

namespace ForumTemplate.DTOs.Validations
{
    public class TagRequestValidator : AbstractValidator<TagRequest>
    {
        private const int MinContentLength = 3;
        private const int MaxContentLength = 9;

        private const string LengthErrorMessage =
            "{PropertyName} must be between {MinLength} and {MaxLength} characters long. You entered {TotalLength} characters";

        private const string RequiredErrorMessage = "{PropertyName} is required.";

        public TagRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .Length(MinContentLength, MaxContentLength)
                .WithMessage(LengthErrorMessage);
        }
    }
}
