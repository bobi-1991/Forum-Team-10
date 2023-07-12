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
        public void ValidateIfEmailDoesExist(string email)
        {
            var doesExists = this.userRepository.EmailDoesExists(email);

            if (doesExists)
            {
                throw new DuplicateEntityException($"User with this email already exists.");
            }
        }
		public void ValidateIfEmailAndUserEmailIsSame(User loggedUser, string email)
		{
			var doesExists = this.userRepository.EmailDoesExists(email);

			if (doesExists)
			{
				if (!loggedUser.Email.Equals(email))
				{
					throw new DuplicateEntityException($"User with this email already exists.");
				}
			}
		}

		public void ValidateIfUsernameExist(string username)
		{
			var doesExists = this.userRepository.DoesExist(username);

			if (!doesExists)
			{
				throw new EntityNotFoundException($"User with username: {username} not found.");
			}
		}

		public void ValidateLoggedUserIsAdmin(User user)
		{
			if (!user.IsAdmin)
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

		public void ValidateByGUIDUserLoggedAndAdmin(User loggedUser, Guid Id)
		{

			if (!loggedUser.UserId.Equals(Id) && !loggedUser.IsAdmin)
			{
				throw new ValidationException("I'm sorry, but you cannot change other user's personal data.");
			}
		}

		public void ValidatePostCreateIDMatchAndNotBlocked(User loggedUser, PostRequest postRequest)
		{
			if (!loggedUser.UserId.Equals(postRequest.UserId))
			{
				throw new ValidationException("The id you entered does not match your id");
			}
			if (loggedUser.IsBlocked)
			{
				throw new EntityBannedException("I'm sorry, but you cannot create a new post. You are permanently banned.");
			}
		}

		public void ValidateUserIdMatchAuthorIdPost(User loggedUser,Guid authorId)
		{
			if (!loggedUser.UserId.Equals(authorId) && !loggedUser.IsAdmin)
			{
				throw new ValidationException("The id you entered does not match yours post(s) id");
			}
		}

		public void ValidateUserIsNotBannedCommentCreate(User loggedUser)
		{
			if (loggedUser.IsBlocked)
			{
				throw new EntityBannedException("I'm sorry, but you cannot write a comment. You are currently banned.");
			}
		}

        public void ValidateUserIsNotBannedTagCreate(User loggedUser)
        {
            if (loggedUser.IsBlocked)
            {
                throw new EntityBannedException("I'm sorry, but you cannot write a tag. You are currently banned.");
            }
        }

        public void ValidateUserIdMatchAuthorIdComment(User loggedUser, Guid? authorId)
		{
			if (!loggedUser.UserId.Equals(authorId) && !loggedUser.IsAdmin)
			{
				throw new ValidationException("The id you entered does not match yours comment(s) id");
			}
		}

        public void ValidateUserIdMatchAuthorIdTag(User loggedUser, Guid? authorId)
        {
            if (!loggedUser.UserId.Equals(authorId) && !loggedUser.IsAdmin)
            {
                throw new ValidationException("The id you entered does not match yours Tag(s) id");
            }
        }


    }
}
