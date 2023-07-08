﻿using FluentValidation;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.DTOs.Validations
{
    public class PostViewModelValidator: AbstractValidator<PostViewModel>
    {
        private const int MinTitleLength = 16;
        private const int MaxTitleLength = 64;

        private const string LengthErrorMessage =
            "{PropertyName} must be between {MinLength} and {MaxLength} characters long. You entered {TotalLength} characters";
        private const string RequiredErrorMessage = "{PropertyName} is required.";

        private const int MinContentLength = 32;
        private const int MaxContentLength = 8192;

        public PostViewModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .Length(MinTitleLength, MaxTitleLength)
                .WithMessage(LengthErrorMessage);

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage(RequiredErrorMessage)
                .Length(MinContentLength, MaxContentLength)
                .WithMessage(LengthErrorMessage);
        }
    }
}
