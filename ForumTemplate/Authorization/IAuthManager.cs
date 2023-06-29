using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;

namespace ForumTemplate.Authorization
{
    public interface IAuthManager
    {
        string TrySetCurrentLoggedUser(string credentials);

        string LogoutUser(string username);

        string TryRegisterUser(RegisterUserRequestModel user);

        string TryPromoteUser(string username, UpdateUserRequestModel user);

        string TryDemoteUser(string username, UpdateUserRequestModel user);

        string TryBanUser(string username, UpdateUserRequestModel user);

        string TryUnBanUser(string username, UpdateUserRequestModel user);

        User GetCurrentLoggedUser();

    }
}
