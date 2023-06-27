using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Services.UserService
{
    public interface IUserService
    {
        List<UserResponse> GetAll();

        UserResponse GetById(Guid id);

        //UserResponse Create(RegisterRequest registerRequest);

        UserResponse Update(Guid id, RegisterUserRequestModel registerRequest);

        string Delete(Guid id);

        //Authentication
        User Login(string username, string encodedPassword);

        User Logout(string username);

        string RegisterUser(RegisterUserRequestModel user, string encodedPassword);
    }
}
