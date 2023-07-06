using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Services.UserService;
using System.Text;

namespace ForumTemplate.Authorization
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserService userService;
        //To Add Current Logged User
        public AuthManager(IUserService userService)
        {
            this.userService = userService;
        }

		public User TryGetUser(string credentials)
		{
			string[] credentialsArray = credentials.Split(':');
			string username = credentialsArray[0];
			string password = credentialsArray[1];

			string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

			try
			{
				var user = this.userService.GetByUsername(username);
				if (user.Password == encodedPassword)
				{
					return user;
				}
				throw new EntityUnauthorizatedException("Invalid credentials");
			}
			catch (EntityNotFoundException)
			{
				throw new EntityUnauthorizatedException("Invalid username!");
			}
		}

		//public string TrySetCurrentLoggedUser(string credentials)
  //      {
  //          string[] credentialsArray = credentials.Split(':');
  //          string username = credentialsArray[0];
  //          string password = credentialsArray[1];

  //          string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

  //          var user = this.userService.Login(username, encodedPassword);

  //          SetCurrentLoggedUser(user);

  //          return "User successfully logged in";
  //      }

        //public string LogoutUser(string username)
        //{
        //    if (CurrentLoggedUser.LoggedUser != null && CurrentLoggedUser.LoggedUser.Username.Equals(username))
        //    {
        //        this.userService.Logout(username);
        //        CurrentLoggedUser.LoggedUser = null;
        //        return "User successfully logged out";
        //    }
        //    else
        //    {
        //        return "Тhis User is not the currently logged in User";
        //    }
        //}

        public string TryRegisterUser(RegisterUserRequestModel user)
        {
            
            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Password));

            return userService.RegisterUser(user, encodedPassword);
        }

        public string TryPromoteUser(User loggedUser, UpdateUserRequestModel user)
        {
            return this.userService.PromoteUser(loggedUser, user);
        }

        public string TryDemoteUser(User loggedUser, UpdateUserRequestModel user)
        {
            return this.userService.DemoteUser(loggedUser, user);
        }

        public string TryBanUser(User loggedUser, UpdateUserRequestModel user)
        {
            return this.userService.BanUser(loggedUser, user);
        }

        public string TryUnBanUser(User loggedUser, UpdateUserRequestModel user)
        {
            return this.userService.UnBanUser(loggedUser, user);
        }
        //private void SetCurrentLoggedUser(User user)
        //{
        //    CurrentLoggedUser.LoggedUser = user;
        //}

        //public User GetCurrentLoggedUser()
        //{
        //    return CurrentLoggedUser.LoggedUser;
        //}
    }
}
