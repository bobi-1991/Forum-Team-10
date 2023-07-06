using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;
using System.Net;

namespace ForumTemplate.Authorization
{
    public interface IAuthManager
    {
		User TryGetUser(string credentials);
	//	string TrySetCurrentLoggedUser(string credentials);

       // string LogoutUser(string username);

        string TryRegisterUser(RegisterUserRequestModel user);

        string TryPromoteUser(User loggedUser, UpdateUserRequestModel user);

        string TryDemoteUser(User loggeduser, UpdateUserRequestModel user);

        string TryBanUser(User loggeduser, UpdateUserRequestModel user);

        string TryUnBanUser(User loggedUser, UpdateUserRequestModel user);

      //  User GetCurrentLoggedUser();

    }
}
