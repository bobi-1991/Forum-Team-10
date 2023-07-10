using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Services.UserService
{
    public interface IUserService
    {
        List<UserResponse> GetAll();
        List<User> GetAllUsers();
        UserResponse GetById(Guid id);
		User GetByUsername(string username);
        bool UsernameExists(string username);
		UserResponse Update(User loggedUser, Guid id, UpdateUserRequest registerRequest);
        string Delete(User loggedUser, Guid id);
        string RegisterUser(RegisterUserRequestModel user, string encodedPassword);
		string PromoteUser(User user, UpdateUserRequestModel userToPromote);
		string DemoteUser(User loggedUser, UpdateUserRequestModel userToPromote);
        string BanUser(User loggedUser, UpdateUserRequestModel userToDemote);
        string UnBanUser(User loggedUser, UpdateUserRequestModel userToUnBan);
    }
}
