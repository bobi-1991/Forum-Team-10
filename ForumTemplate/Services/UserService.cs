using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Repositories;

namespace ForumTemplate.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;

        public UserService(IUserRepository userRepository, IPostRepository postRepository)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
        }

        public List<User> GetAll()
        {
            return this.userRepository.GetAll();
        }

        public User GetById(int id)
        {
            var user = this.userRepository.GetById(id);

            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID: {id} not found.");
            }

            var userPosts = postRepository.GetByUserId(user.Id);
            var posts = userPosts.Select(x => x.MapToPostResultModel()).ToList();
            user.Posts = posts;

            return user;
        }

        public User Create(User user)
        {
            var doesExists = this.userRepository.DoesExist(user.Username);

            if (doesExists)
            {
                throw new DuplicateEntityException($"User {user.Username} already exists.");
            }

            return this.userRepository.Create(user);
        }

        

        public User Update(int id, User user)
        {
            var userToUpdate = this.userRepository.GetById(id);

            if (userToUpdate == null)
            {
                throw new EntityNotFoundException($"User with ID: {id} not found.");
            }

            return this.userRepository.Update(id, user);
        }

        public string Delete(int id)
        {
            var userToDelete = this.userRepository.GetById(id);

            if (userToDelete == null)
            {
                throw new EntityNotFoundException($"User with ID: {id} not found.");
            }

            return this.userRepository.Delete(id);
        }
    }
}
