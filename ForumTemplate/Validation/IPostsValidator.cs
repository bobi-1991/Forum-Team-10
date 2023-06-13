using ForumTemplate.Exceptions;
using ForumTemplate.Models.Input;
using ForumTemplate.Repositories;

namespace ForumTemplate.Validation
{
    public interface IPostsValidator
    {
        void Validate(int id);

        void Validate(PostInputModel postInput);

        void Validate(int id, PostInputModel postInput);

    }
}
