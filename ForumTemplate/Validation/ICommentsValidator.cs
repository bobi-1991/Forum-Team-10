using ForumTemplate.DTOs.CommentDTOs;

namespace ForumTemplate.Validation
{
    public interface ICommentsValidator
    {
        void Validate(CommentRequest commentRequest);

        void Validate(Guid id, CommentRequest commentRequest);

        void Validate(Guid id);
    }
}
