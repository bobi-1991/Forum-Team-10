using ForumTemplate.Exceptions;
using ForumTemplate.Models.Input;
using ForumTemplate.Repositories;


namespace ForumTemplate.Validation

{
    public class PostsValidator : IPostsValidator
    {

        private readonly IPostRepository postRepository;
        public PostsValidator(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public void Validate(int id)
        {
            var post = this.postRepository.GetById(id);

            if (id <= 0)
            {
                throw new ValidationException($"Post ID cannot be 0 or negative number");
            }

            if (post == null)
            {
                throw new ValidationException($"Post with ID: {id} not found.");
            }
        }

        public void Validate(PostInputModel postInput)
        {
            var errors = new List<string>();

            if (postInput is null)
            {
                errors.Add("Post input cannot be null");
            }

            if (string.IsNullOrWhiteSpace(postInput.Title))
            {
                errors.Add("Post Title cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(postInput.Description))
            {
                errors.Add("Post Description cannot be null or whitespace");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }

        public void Validate(int id, PostInputModel postInput)
        {
            var errors = new List<string>();

            if (id <= 0)
            {
                errors.Add("Post Id cannot be a negative number or 0");
            }

            if (id > 0)
            {
                var postToUpdate = this.postRepository.GetById(id);
                if (postToUpdate == null)
                {
                    errors.Add($"Post with ID: {id} not found.");
                }
            }

            if (postInput is null)
            {
                errors.Add("Post input cannot be null");
            }

            if (string.IsNullOrWhiteSpace(postInput.Title))
            {
                errors.Add("Post Title cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(postInput.Description))
            {
                errors.Add("Post Description cannot be null or whitespace");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }

    }
}
