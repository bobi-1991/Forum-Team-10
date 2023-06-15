using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;

namespace ForumTemplate.Services.UserService
{
    public interface IUserService
    {
        List<UserResponse> GetAll();

        UserResponse GetById(Guid id);

        UserResponse Create(RegisterRequest registerRequest);

        UserResponse Update(Guid id, RegisterRequest registerRequest);

        string Delete(Guid id);
    }
}
