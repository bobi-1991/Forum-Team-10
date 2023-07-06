﻿using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Validation
{
    public interface IUserAuthenticationValidator
    {
        void ValidateDoesExist(string username);
        void ValidateIfUsernameExist(string username);

      //  void ValidateUserExistAndIsLoggedAndIsAdmin(string username);
		void ValidateLoggedUserIsAdmin(User user);
		//void ValidateUserById(Guid userId);


		void ValidateUserAlreadyAdmin(User user);

        void ValidateUserAlreadyRegular(User user);

        void ValidateUserAlreadyBanned(User user);

        void ValidateUserNotBanned(User user);

        void ValidateByGUIDUserLoggedAndAdmin(User loggedUser,Guid Id);

     //   void ValidateUserIsLoggedAndAdmin();

      //  void ValidateUserIsLogged();
		void ValidateUserIsNotBannedCommentCreate(User loggedUser);

		void ValidatePostCreateIDMatchAndNotBlocked(User loggedUser,PostRequest postRequest);

        void ValidateUserIdMatchAuthorIdPost(User loggedUser, Guid authorId);

        void ValidateUserIdMatchAuthorIdComment(User loggedUser, Guid? authorId);
    }
}
