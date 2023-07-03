using ForumTemplate.Authorization;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Validation;

namespace ForumTemplate.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUserMapper userMapper;
        private readonly IUserAuthenticationValidator userValidator;


        public UserService(IUserRepository userRepository, IUserMapper userMapper, IUserAuthenticationValidator userValidator)
        {
            this.userRepository = userRepository;
            this.userMapper = userMapper;
            this.userValidator = userValidator;
        }

        public List<UserResponse> GetAll()
        {
            userValidator.ValidateUserIsLoggedAndAdmin();

            var users = userRepository.GetAll();

            return this.userMapper.MapToUserResponse(users);
        }

        public UserResponse GetById(Guid id)
        {
            userValidator.ValidateUserIsLoggedAndAdmin();

            var user = userRepository.GetById(id);

            return userMapper.MapToUserResponse(user);
        }

        //Authentication
        public User Login(string username, string encodedPassword)
        {
            return this.userRepository.Login(username, encodedPassword);
        }

        public User Logout(string username)
        {
            return this.userRepository.Logout(username);
        }

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

        public string PromoteUser(string username, UpdateUserRequestModel userToPromote)
        {
            userValidator.ValidateUserExistAndIsLoggedAndIsAdmin(username);

            var userToBePromoted = userRepository.GetByUsername(userToPromote.UserName);

            userValidator.ValidateUserAlreadyAdmin(userToBePromoted);

            return userRepository.PromoteUser(userToBePromoted);
        }

        public string DemoteUser(string username, UpdateUserRequestModel userToDemote)
        {
            userValidator.ValidateUserExistAndIsLoggedAndIsAdmin(username);

            var userToBeDemoted = userRepository.GetByUsername(userToDemote.UserName);

            userValidator.ValidateUserAlreadyRegular(userToBeDemoted);

            return userRepository.DemoteUser(userToBeDemoted);
        }

        public string BanUser(string username, UpdateUserRequestModel userToBeBanned)
        {
            userValidator.ValidateUserExistAndIsLoggedAndIsAdmin(username);

            var userToBeBannedActual = userRepository.GetByUsername(userToBeBanned.UserName);

            userValidator.ValidateUserAlreadyBanned(userToBeBannedActual);

            return userRepository.BanUser(userToBeBannedActual);
        }

        public string UnBanUser(string username, UpdateUserRequestModel userToUnBan)
        {
            userValidator.ValidateUserExistAndIsLoggedAndIsAdmin(username);

            var userToBeUnBanned = userRepository.GetByUsername(userToUnBan.UserName);

            userValidator.ValidateUserNotBanned(userToBeUnBanned);

            return userRepository.UnBanUser(userToBeUnBanned);
        }

        public UserResponse Update(Guid id, UpdateUserRequest updateUserRequest)
        {
            userValidator.ValidateByGUIDUserLoggedAndAdmin(id);     

            var userData = this.userMapper.MapToUser(updateUserRequest);
            var user = userRepository.Update(id, userData);

            return userMapper.MapToUserResponse(user);
        }

        public string Delete(Guid id)
        {
            userValidator.ValidateByGUIDUserLoggedAndAdmin(id);

            return userRepository.Delete(id);
        }
    }
}
