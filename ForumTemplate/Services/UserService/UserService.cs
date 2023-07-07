﻿using ForumTemplate.Authorization;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Services.LikeService;
using ForumTemplate.Services.PostService;
using ForumTemplate.Validation;

namespace ForumTemplate.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUserMapper userMapper;
        private readonly IUserAuthenticationValidator userValidator;
        private readonly ILikeService likeService;
        private readonly IPostService postService;


        public UserService(IUserRepository userRepository, IUserMapper userMapper, IUserAuthenticationValidator userValidator, ILikeService likeService, IPostService postService)
        {
            this.userRepository = userRepository;
            this.userMapper = userMapper;
            this.userValidator = userValidator;
            this.likeService = likeService;
            this.postService = postService;
        }

        public List<UserResponse> GetAll()
        {  
            var users = userRepository.GetAll();
            return this.userMapper.MapToUserResponse(users);
        }

        public UserResponse GetById(Guid id)
        {
            var user = userRepository.GetById(id);

            return userMapper.MapToUserResponse(user);
        }
		public User GetByUsername(string username)
		{
            userValidator.ValidateIfUsernameExist(username);

			return this.userRepository.GetByUsername(username);
		}

		//Authentication
        public string RegisterUser(RegisterUserRequestModel user, string encodedPassword)
        {
            userValidator.ValidateDoesExist(user.Username);

            //Must be done by mapper
            var userDB = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Password = encodedPassword,
                Country = user.Country
            };

            return userRepository.RegisterUser(userDB);
        }

        public string PromoteUser(User loggedUser, UpdateUserRequestModel userToPromote)
        {
            userValidator.ValidateLoggedUserIsAdmin(loggedUser);

            userValidator.ValidateIfUsernameExist(userToPromote.UserName);

            var userToBePromoted = userRepository.GetByUsername(userToPromote.UserName);

            userValidator.ValidateUserAlreadyAdmin(userToBePromoted);

            return userRepository.PromoteUser(userToBePromoted);
        }

        public string DemoteUser(User loggedUser, UpdateUserRequestModel userToDemote)
        {
			userValidator.ValidateLoggedUserIsAdmin(loggedUser);

			userValidator.ValidateIfUsernameExist(userToDemote.UserName);

			var userToBeDemoted = userRepository.GetByUsername(userToDemote.UserName);

            userValidator.ValidateUserAlreadyRegular(userToBeDemoted);

            return userRepository.DemoteUser(userToBeDemoted);
        }

        public string BanUser(User loggedUser, UpdateUserRequestModel userToBeBanned)
        {
			userValidator.ValidateLoggedUserIsAdmin(loggedUser);

			userValidator.ValidateIfUsernameExist(userToBeBanned.UserName);

			var userToBeBannedActual = userRepository.GetByUsername(userToBeBanned.UserName);

            userValidator.ValidateUserAlreadyBanned(userToBeBannedActual);

            return userRepository.BanUser(userToBeBannedActual);
        }

        public string UnBanUser(User loggedUser, UpdateUserRequestModel userToUnBan)
        {
			userValidator.ValidateLoggedUserIsAdmin(loggedUser);

			userValidator.ValidateIfUsernameExist(userToUnBan.UserName);

			var userToBeUnBanned = userRepository.GetByUsername(userToUnBan.UserName);

            userValidator.ValidateUserNotBanned(userToBeUnBanned);

            return userRepository.UnBanUser(userToBeUnBanned);
        }

        public UserResponse Update(User loggedUser,Guid id, UpdateUserRequest updateUserRequest)
        {
            userValidator.ValidateByGUIDUserLoggedAndAdmin(loggedUser, id);     

            var userData = this.userMapper.MapToUser(updateUserRequest);

            var user = userRepository.Update(id, userData);

            return userMapper.MapToUserResponse(user);
        }

        public string Delete(User loggedUser, Guid id)
        {
            userValidator.ValidateByGUIDUserLoggedAndAdmin(loggedUser, id);

            this.likeService.DeleteByUserId(id);

            this.postService.DeleteByUserId(id);

            return userRepository.Delete(id);
        }
    }
}
