using ForumTemplate.Models.Input;

namespace ForumTemplate.Validation
{
    public interface ICommentsValidator
    {
        void Validate(CommentInputModel commentInput);

        void Validate(int id, CommentInputModel commentInput);

        void Validate(int id);
    }
}
