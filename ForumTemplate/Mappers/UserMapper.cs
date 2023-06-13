using ForumTemplate.Models;
using ForumTemplate.Services;

namespace ForumTemplate.Mappers
{
    public class UserMapper
    {

        private readonly IPostService postService;

        public UserMapper(IPostService postService)
        {
            this.postService = postService;
        }

        public User Map(UserDTO dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                Country = dto.Country,
                //Post = this.postService.GetById(dto.PostId)
            };
        }

    }
}
