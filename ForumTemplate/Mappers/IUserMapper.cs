using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;

namespace ForumTemplate.Mappers
{
    public interface IUserMapper
    {
        User MapToUser(RegisterUserRequestModel registerUser);
        User MapToUser(UpdateUserRequest updateUserRequest);
        UserResponse MapToUserResponse(User user);
        List<UserResponse> MapToUserResponse(List<User> users);
        RegisterUserRequestModel MapToRegisterUserRequestModel(RegisterViewModel registerViewModel);
        UserViewModel MapToUserViewModel(User user);


    }
}
