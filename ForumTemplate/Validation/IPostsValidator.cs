using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Exceptions;

namespace ForumTemplate.Validation
{
    public interface IPostsValidator
    {
        void Validate(Guid id);

        void Validate(PostRequest postRequest);

        void Validate(Guid id, PostRequest postRequest);

    }
}
