using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;

namespace ForumTemplate.Services.UserService
{
    public interface IUserService
    {
        List<UserResponse> GetAll();
        List<User> GetAllUsers();
        UserResponse GetById(Guid id);
        User GetByUserId(Guid id);
		User GetByUsername(string username);
        void ValidateUpdatedUserEmail(User loggedUser, string email);
		bool UsernameExists(string username);
		UserResponse Update(User loggedUser, Guid id, UpdateUserRequest registerRequest);
        List<User> SearchByAdminCriteria(string searchInfo);
        User AdminEditionUpdate(User loggedUser, AdminEditViewModel adminEditViewModel);
        string Delete(User loggedUser, Guid id);
        string RegisterUser(RegisterUserRequestModel user, string encodedPassword);
		string PromoteUser(User user, UpdateUserRequestModel userToPromote);
		string DemoteUser(User loggedUser, UpdateUserRequestModel userToPromote);
        string BanUser(User loggedUser, UpdateUserRequestModel userToDemote);
        string UnBanUser(User loggedUser, UpdateUserRequestModel userToUnBan);
    }
}
