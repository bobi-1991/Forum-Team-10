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

            if (post == null)
            {
                throw new ValidationException($"Post with ID: {id} not found.");
            }
        }
    }
}
