using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.TagRepository;

namespace ForumTemplate.Validation
{
    public class TagsValidator : ITagsValidator
    {
        private readonly IPostRepository postRepository;
        private readonly ITagRepository tagRepository;
        public TagsValidator(IPostRepository postRepository, ITagRepository tagRepository)
        {
            this.postRepository = postRepository;
            this.tagRepository = tagRepository;
        }
        public void Validate(TagRequest tagRequest)
        {
            var errors = new List<string>();

            if (!this.postRepository.Exist(tagRequest.PostId))
            {
                errors.Add($"Tag with Id {tagRequest.PostId} does not exist");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }

        public void Validate(Guid id)
        {
            var tag = this.tagRepository.GetById(id);

            if (tag == null)
            {
                throw new ValidationException($"Tag with ID: {id} not found.");
            }
        }

        public void GetTagsByPostID(Guid postId)
        {
            var tagsById = this.tagRepository.GetByPostId(postId);

            if (tagsById == null)
            {
                throw new EntityNotFoundException($"Tag with ID: {postId} not found.");
            }
        }
    }
}
