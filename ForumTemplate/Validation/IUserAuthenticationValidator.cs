using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Validation
{
    public interface IUserAuthenticationValidator
    {
        void ValidateDoesExist(string username);

        void ValidateUserExistAndIsLoggedAndIsAdmin(string username);

        void ValidateUserAlreadyAdmin(User user);

        void ValidateUserAlreadyRegular(User user);

        void ValidateUserAlreadyBanned(User user);

        void ValidateUserNotBanned(User user);

        void ValidateByGUIDUserLoggedAndAdmin(Guid Id);

        void ValidateUserIsLoggedAndAdmin();

        void ValidateUserIsLogged();

        void ValidatePostCreateIDMatchAndNotBlocked(PostRequest postRequest);

        void ValidateUserIdMatchAuthorIdPost(Guid authorId);

        void ValidateUserIsLoggedAndNotBannedCommentCreate();
    }
}
