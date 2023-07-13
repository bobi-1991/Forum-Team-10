using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;
using System.Text;

namespace ForumTemplate.Mappers
{
    public class UserMapper : IUserMapper
    {
        public User MapToUser(RegisterUserRequestModel registerUser)
        {
            return User.Create(
                registerUser.FirstName,
                registerUser.LastName,
                registerUser.Username,
                registerUser.Email,
                registerUser.Password,
                registerUser.Country
            );
        }
        public User MapToUser(UpdateUserRequest updateUserRequest)
        {
            return new User
            {
                FirstName = updateUserRequest.FirstName,
                LastName = updateUserRequest.LastName,
                Email = updateUserRequest.Email,
                Password = updateUserRequest.Password,
                Country = updateUserRequest.Country
            };
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
        public RegisterUserRequestModel MapToRegisterUserRequestModel(RegisterViewModel registerViewModel)
        {
            return new RegisterUserRequestModel
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Country = registerViewModel.Country,
                Username = registerViewModel.Username,
                Password = registerViewModel.Password,
                Email = registerViewModel.Email
            };
        }
        public UserViewModel MapToUserViewModel(User user)
        {
            return new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Country = user.Country,
                Username = user.Username,
                Email = user.Email,
                Role = user.IsAdmin ? "Admin" : "User",
                IsBlocked = user.IsBlocked ? "Blocked" : "Active"
            };
        }

        public UserEditViewModel MapToUserEditViewModel(User user)
        {
            string encodedPassword = user.Password;
            byte[] passwordBytes = Convert.FromBase64String(encodedPassword);
            string decodedPassword = Encoding.UTF8.GetString(passwordBytes);


            return new UserEditViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Country = user.Country,
                Password = decodedPassword,
                ConfirmPassword = decodedPassword,
                Email = user.Email,
                Username = user.Username
            };
        }
        public UpdateUserRequest MapToUpdateUserRequest(UserEditViewModel userEditViewModel)
        {


            return new UpdateUserRequest
            {
                FirstName = userEditViewModel.FirstName,
                LastName = userEditViewModel.LastName,
                Country = userEditViewModel.Country,
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(userEditViewModel.Password)),
                Email = userEditViewModel.Email
            };
        }
        public UpdateUserRequestModel MapToUpdateUserRequestModel(User user)
        {
            return new UpdateUserRequestModel
            {
                UserName = user.Username
            };
        }
        public AdminEditViewModel MapToAdminEditUserModel(User user)
        {
            string encodedPassword = user.Password;
            byte[] passwordBytes = Convert.FromBase64String(encodedPassword);
            string decodedPassword = Encoding.UTF8.GetString(passwordBytes);

            return new AdminEditViewModel
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Country = user.Country,
                Username = user.Username,
                Password = decodedPassword,
                // Password = user.Password,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                IsBlocked = user.IsBlocked
            };
        }
        public User MapToUser(AdminEditViewModel adminEditViewModel)
        {
            return new User
            {
                UserId = adminEditViewModel.Id,
                FirstName = adminEditViewModel.FirstName,
                LastName = adminEditViewModel.LastName,
                Country = adminEditViewModel.Country,
                Username = adminEditViewModel.Username,
                // Password = adminEditViewModel.Password,
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(adminEditViewModel.Password)),
                Email = adminEditViewModel.Email,
                IsAdmin = adminEditViewModel.IsAdmin,
                IsBlocked = adminEditViewModel.IsBlocked
            };
        }
    }
}
