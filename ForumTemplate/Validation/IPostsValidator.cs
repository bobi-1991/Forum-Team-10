using ForumTemplate.DTOs.PostDTOs;

namespace ForumTemplate.Validation
{
    public interface IPostsValidator
    {
        void Validate(Guid id);

        void Validate(PostRequest postRequest);

        void Validate(Guid id, PostRequest postRequest);
    }
}
