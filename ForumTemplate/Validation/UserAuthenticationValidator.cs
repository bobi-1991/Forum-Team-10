using ForumTemplate.Authorization;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.UserRepository;

namespace ForumTemplate.Validation
{
    public class UserAuthenticationValidator : IUserAuthenticationValidator
    {

        private readonly IUserRepository userRepository;
        public UserAuthenticationValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void ValidateDoesExist(string username)
        {
            var doesExists = this.userRepository.DoesExist(username);

            if (doesExists)
            {
                throw new DuplicateEntityException($"User already exists.");
            }
        }

        public void ValidateUserExistAndIsLoggedAndIsAdmin(string username)
        {
            var userRequestor = userRepository.GetByUsername(username);

            if (CurrentLoggedUser.LoggedUser is null || !userRequestor.IsLogged)
            {
                throw new ArgumentException("User who is requesting is found, but is not logged in, please log in");
            }
            if (userRequestor.IsLogged && !userRequestor.IsAdmin)
            {
                throw new ArgumentException("I am sorry, you are not an admin to perform this operation");
            }
        }

        public void ValidateUserAlreadyAdmin(User user)
        {
            if (user.IsAdmin)
            {
                throw new ArgumentException("The user you are trying to promote is already an admin");
            }
        }

        public void ValidateUserAlreadyRegular(User user)
        {
            if (!user.IsAdmin)
            {
                throw new ArgumentException("The user you are trying to demote is already a regular user");
            }
        }

        public void ValidateUserAlreadyBanned(User user)
        {
            if (user.IsBlocked)
            {
                throw new ArgumentException("The user you are trying to ban is already banned");
            }
        }

        public void ValidateUserNotBanned(User user)
        {
            if (!user.IsBlocked)
            {
                throw new ArgumentException("The user you are trying to UnBan is not banned");
            }
        }

        public void ValidateByGUIDUserLoggedAndAdmin(Guid Id)
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.UserId.Equals(Id) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("I'm sorry, but you cannot change other user's personal data.");
            }
        }

        public void ValidateUserIsLoggedAndAdmin()
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new EntityUnauthorizatedException("You are not authorized for this functionality");
            }
        }

        public void ValidateUserIsLogged()
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
        }

        public void ValidatePostCreateIDMatchAndNotBlocked(PostRequest postRequest)
        {
            if (!CurrentLoggedUser.LoggedUser.UserId.Equals(postRequest.UserId))
            {
                throw new ValidationException("The id you entered does not match your id");
            }
            if (CurrentLoggedUser.LoggedUser.IsBlocked)
            {
                throw new EntityBannedException("I'm sorry, but you cannot create a new post. You are permanently banned.");
            }
        }

        public void ValidateUserIdMatchAuthorIdPost(Guid authorId)
        {
            if (!CurrentLoggedUser.LoggedUser.UserId.Equals(authorId) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("The id you entered does not match yours post(s) id");
            }
        }

        public void ValidateUserIsLoggedAndNotBannedCommentCreate()
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (CurrentLoggedUser.LoggedUser.IsBlocked)
            {
                throw new EntityBannedException("I'm sorry, but you cannot write a comment. You are currently banned.");
            }
        }

        public void ValidateUserIdMatchAuthorIdComment(Guid? authorId)
        {
            if (!CurrentLoggedUser.LoggedUser.UserId.Equals(authorId) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("The id you entered does not match yours comment(s) id");
            }
        }

    }
}
