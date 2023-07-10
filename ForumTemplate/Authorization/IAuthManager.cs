using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;
using System.Net;

namespace ForumTemplate.Authorization
{
	public interface IAuthManager
	{
		User TryGetUser(string credentials);
		User TryGetUser(string username, string password);
		void Login(string username, string password);
		void Logout();
		User CurrentUser { get; set; }
		string EncodePassword(string password);
		string TryRegisterUser(RegisterUserRequestModel user);
		string TryPromoteUser(User loggedUser, UpdateUserRequestModel user);
		string TryDemoteUser(User loggeduser, UpdateUserRequestModel user);
		string TryBanUser(User loggeduser, UpdateUserRequestModel user);
		string TryUnBanUser(User loggedUser, UpdateUserRequestModel user);

	}
}
