using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Repositories;

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

        public UserResponse Create(RegisterRequest registerRequest)
        {

            var user = userMapper.MapToUser(registerRequest);

            var doesExists = userRepository.DoesExist(user.Username);

            if (doesExists)
            {
                throw new DuplicateEntityException($"User {user.Username} already exists.");
            }

            this.userRepository.AddUser(user);

            return userMapper.MapToUserResponse(user);
        }



        public UserResponse Update(Guid id, RegisterRequest registerRequest)
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
