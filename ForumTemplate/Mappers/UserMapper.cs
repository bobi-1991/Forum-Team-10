using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models;
using ForumTemplate.Services.PostService;

namespace ForumTemplate.Mappers
{
    public class UserMapper
    {
        public User MapToUser(RegisterRequest registerUser)
        {
            return User.Create(
                registerUser.FirstName,
                registerUser.LastName,
                registerUser.Username,
                registerUser.Email,
                registerUser.Password
            );
        }
        public UserResponse MapToUserResponse(User user)
        {
            return new UserResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Username,
                user.Email,
                user.UpdatedAt
            );
        }
        public List<UserResponse> MapToUserResponse(List<User> users)
        {
            var userResponses = new List<UserResponse>();

            foreach (var user in users)
            {
                var result = new UserResponse(
                         user.Id,
                         user.FirstName,
                         user.LastName,
                         user.Username,
                         user.Email,
                         user.UpdatedAt
                         );

                userResponses.Add(result);
            }

            return userResponses;
        }
    }
}
