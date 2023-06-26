using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Persistence.PostRepository;

namespace ForumTemplate.Validation

{
    public class PostsValidator : IPostsValidator
    {

        private readonly IPostRepository postRepository;
        public PostsValidator(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public void Validate(Guid id)
        {
            var post = this.postRepository.GetById(id);

            //if (id <= 0)
            //{
            //    throw new ValidationException($"Post ID cannot be 0 or negative number");
            //}

            if (post == null)
            {
                throw new ValidationException($"Post with ID: {id} not found.");
            }
        }

        public void Validate(PostRequest postRequest)
        {
            var errors = new List<string>();
            // DEPRECATED - replaced by Fluid Validation
            if (postRequest is null)
            {
                errors.Add("Post input cannot be null");
            }
            // DEPRECATED - replaced by Fluid Validation
            if (string.IsNullOrWhiteSpace(postRequest.Title))
            {
                errors.Add("Post Title cannot be null or whitespace");
            }
            // DEPRECATED - replaced by Fluid Validation
            if (string.IsNullOrWhiteSpace(postRequest.Content))
            {
                errors.Add("Post Description cannot be null or whitespace");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }

        public void Validate(Guid id, PostRequest postRequest)
        {
            var errors = new List<string>();

            //if (id <= 0)
            //{
            //    errors.Add("Post Id cannot be a negative number or 0");
            //}

            //if (id > 0)
            //{
            //    var postToUpdate = this.postRepository.GetById(id);
            //    if (postToUpdate == null)
            //    {
            //        errors.Add($"Post with ID: {id} not found.");
            //    }
            //}
            // DEPRECATED - replaced by Fluid Validation
            if (postRequest is null)
            {
                errors.Add("Post input cannot be null");
            }
            // DEPRECATED - replaced by Fluid Validation
            if (string.IsNullOrWhiteSpace(postRequest.Title))
            {
                errors.Add("Post Title cannot be null or whitespace");
            }
            // DEPRECATED - replaced by Fluid Validation
            if (string.IsNullOrWhiteSpace(postRequest.Content))
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
