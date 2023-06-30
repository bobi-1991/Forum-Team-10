using ForumTemplate.Authorization;
using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.UserRepository;

namespace ForumTemplate.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;
        private readonly UserMapper userMapper;


        public UserService(IUserRepository userRepository, IPostRepository postRepository, UserMapper userMapper)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.userMapper = userMapper;
        }

        public List<UserResponse> GetAll()
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.IsAdmin)
            { 
                throw new EntityUnauthorizatedException("You are not authorized for this functionality");
            }

            var users = userRepository.GetAll();
            return this.userMapper.MapToUserResponse(users);
        }

        public UserResponse GetById(Guid id)
        {
            var user = userRepository.GetById(id);

            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID: {id} not found.");
            }
            if (!CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new EntityUnauthorizatedException("You are not authorized for this functionality");
            }

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
            var doesExists = this.userRepository.DoesExist(user.Username);

            if (doesExists)
            {
                throw new DuplicateEntityException($"User already exists.");
            }

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

            userRepository.RegisterUser(userDB);

            return "User successfully registered.";
        }

        public string PromoteUser(string username, UpdateUserRequestModel userToPromote)
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

            var userToBePromoted = userRepository.GetByUsername(userToPromote.UserName);

            if(userToBePromoted.IsAdmin) 
            {
                throw new ArgumentException("The user you are trying to promote is already an admin");
            }

            userRepository.PromoteUser(userToBePromoted);

            return "User successfully promoted";
        }

        public string DemoteUser(string username, UpdateUserRequestModel userToDemote)
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

            var userToBeDemoted = userRepository.GetByUsername(userToDemote.UserName);

            if (!userToBeDemoted.IsAdmin)
            {
                throw new ArgumentException("The user you are trying to demote is already a regular user");
            }

            userRepository.DemoteUser(userToBeDemoted);

            return "User successfully demoted";
        }

        public string BanUser(string username, UpdateUserRequestModel userToBeBanned)
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

            var userToBeBannedActual = userRepository.GetByUsername(userToBeBanned.UserName);

            if (userToBeBannedActual.IsBlocked)
            {
                throw new ArgumentException("The user you are trying to ban is already banned");
            }

            userRepository.BanUser(userToBeBannedActual);

            return "User successfully banned";
        }

        public string UnBanUser(string username, UpdateUserRequestModel userToUnBan)
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

            var userToBeUnBanned = userRepository.GetByUsername(userToUnBan.UserName);

            if (!userToBeUnBanned.IsBlocked)
            {
                throw new ArgumentException("The user you are trying to UnBan is not banned");
            }

            userRepository.UnBanUser(userToBeUnBanned);

            return "User successfully UnBanned";
        }

        public UserResponse Update(Guid id, UpdateUserRequest updateUserRequest)
        {
            if(CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.UserId.Equals(id) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("I'm sorry, but you cannot change other user's personal data.");
            }

            var userData = this.userMapper.MapToUser(updateUserRequest);
            var user = userRepository.Update(id, userData);

            return userMapper.MapToUserResponse(user);
        }

        public string Delete(Guid id)
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.UserId.Equals(id) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("Sorry, but you are not authorized to delete other users ");
            }

            return userRepository.Delete(id);
        }
    }
}
