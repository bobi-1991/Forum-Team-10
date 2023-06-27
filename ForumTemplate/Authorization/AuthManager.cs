using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;
using ForumTemplate.Services.UserService;
using System.Text;

namespace ForumTemplate.Authorization
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserService userService;
        //To Add Current Logged User
        private User CurrentLoggedUser;
        public AuthManager(IUserService userService)
        {
            this.userService = userService;
        }

        public string TrySetCurrentLoggedUser(string credentials)
        {
            string[] credentialsArray = credentials.Split(':');
            string username = credentialsArray[0];
            string password = credentialsArray[1];

            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            var user = this.userService.Login(username, encodedPassword);

            SetCurrentLoggedUser(user);

            return "User successfully logged in";
        }

        public string LogoutUser(string username)
        {
            if (CurrentLoggedUser != null && CurrentLoggedUser.Username.Equals(username))
            {
                this.userService.Logout(username);
                return "User successfully logged out";
            }
            else
            {
                return "Тhis User is not the currently logged in User";
            }
        }

        public string TryRegisterUser(RegisterUserRequestModel user)
        {
            //To Validate for existing username
            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Password));

            return userService.RegisterUser(user, encodedPassword);
        }

        private void SetCurrentLoggedUser(User user)
        {
            CurrentLoggedUser = user;
        }

        public User GetCurrentLoggedUser()
        {
            return CurrentLoggedUser;
        }
    }
}
