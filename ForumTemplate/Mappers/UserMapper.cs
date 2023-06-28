using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Mappers
{
    public class UserMapper
    {
        public User MapToUser(RegisterUserRequestModel registerUser)
        {
            return User.Create(
                registerUser.FirstName,
                registerUser.LastName,
                registerUser.Country,
                registerUser.Username,
                registerUser.Email,
                registerUser.Password,
                registerUser.Country
            );
        }
        public UserResponse MapToUserResponse(User user)
        {
            return new UserResponse(
                user.UserId,
                user.FirstName,
                user.LastName,
                user.Country,
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
                         user.UserId,
                         user.FirstName,
                         user.LastName,
                         user.Country,
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
