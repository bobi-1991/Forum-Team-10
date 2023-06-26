using FluentValidation;

namespace ForumTemplate.DTOs.Validations.CustomValidators
{
    public static class TitleValidator
    {  
        public static IRuleBuilderOptions<T, string> LengthAndPatternRules<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            int minLength = 16;
            int maxLength = 64;
            // regex pattern that starts with capital case
            // and allows only letters, numbers, spaces, dots, commas, exclamation marks and question marks
            string pattern = @"^[A-Z][a-zA-Z0-9\s\.\,\!\?]*$";


            return ruleBuilder
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Matches(pattern)
                .WithMessage("{PropertyName} must start with a capital case letter.You provided {PropertyValue}.")
                .Length(minLength, maxLength)
                .WithMessage(string.Format("{PropertyName} must be between {0} and {1} letters. You provided {1}.", minLength, maxLength));
        }
    }
}
