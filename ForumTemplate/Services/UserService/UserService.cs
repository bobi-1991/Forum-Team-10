﻿using ForumTemplate.DTOs.Authentication;
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
            var users = userRepository.GetAll();
            return this.userMapper.MapToUserResponse(users);
        }

        public UserResponse GetById(Guid id)
        {
            var user = userRepository.GetById(id);

            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID: {id} not found.");
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

        //public UserResponse Create(RegisterRequest registerRequest)
        //{

        //    var user = userMapper.MapToUser(registerRequest);

        //    var doesExists = userRepository.DoesExist(user.Username);

        //    if (doesExists)
        //    {
        //        throw new DuplicateEntityException($"User {user.Username} already exists.");
        //    }

        //    this.userRepository.AddUser(user);

        //    return userMapper.MapToUserResponse(user);
        //}

        public string PromoteUser(string username, PromoteUserRequestModel userToPromote)
        {
            var userRequestor = userRepository.GetByUsername(username);

            if (!userRequestor.IsLogged)
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

        public UserResponse Update(Guid id, RegisterUserRequestModel registerRequest)
        {
            var userData = this.userMapper.MapToUser(registerRequest);
            var user = userRepository.Update(id, userData);

            return userMapper.MapToUserResponse(user);
        }

        public string Delete(Guid id)
        {
            return userRepository.Delete(id);
        }
    }
}
