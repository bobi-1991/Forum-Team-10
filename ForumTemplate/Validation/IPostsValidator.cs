using ForumTemplate.DTOs.PostDTOs;

namespace ForumTemplate.Validation
{
    public interface IPostsValidator
    {
        void Validate(Guid id);
    }
}
