using FluentValidation;
using ForumTemplate.DTOs.CommentDTOs;

namespace ForumTemplate.DTOs.Validations
{
    public class CommentRequestValidator : AbstractValidator<CommentRequest>
    {
        private const int MaxContentLength = 8192;
        private const string ContentLengthErrorMessage = "Content cannot exceed {MaxLength} characters. Please write less than {TotalLength} characters.";

        public CommentRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required")
                .MaximumLength(MaxContentLength)
                .WithMessage(ContentLengthErrorMessage);
        }
    }
}
