using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;
using ForumTemplate.Services.UserService;
using System.Text;

namespace ForumTemplate.Authorization
{
    public class AuthManager : IAuthManager
    {
        private const string CURRENT_USER = "CURRENT_USER";
        private readonly IUserService userService;
        private readonly IHttpContextAccessor contextAccessor;

        public AuthManager(IUserService userService, IHttpContextAccessor contextAccessor)
        {
            this.userService = userService;
            this.contextAccessor = contextAccessor;
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

		public User TryGetUser(string username, string password)
		{
            string encodedPassword = this.EncodePassword(password);

			try
			{
				var user = this.userService.GetByUsername(username);
				if (user.Password == encodedPassword)
				{
					return user;
				}
                throw new EntityUnauthorizatedException("Invalid username or password");
            }
			catch (EntityNotFoundException)
			{
                throw new EntityUnauthorizatedException("Invalid username or password");
            }
		}
        public string EncodePassword(string password)
        {
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
		}

		//For login auth
		public void Login(string username, string password)
        {
            this.CurrentUser = this.TryGetUser(username, password);

            if (this.CurrentUser == null)
            {
                int? loginAttempts = this.contextAccessor.HttpContext.Session.GetInt32("LOGIN_ATTEMPTS");

                if (loginAttempts.HasValue && loginAttempts == 5)
                {
                    // redirect
                }
                else
                {
                    this.contextAccessor.HttpContext.Session.SetInt32("LOGIN_ATTEMPTS", (int)loginAttempts + 1);
                }

            }
        }

        public void Logout()
        {
            this.CurrentUser = null;
        }

        public User CurrentUser
        {
            get
            {
                try
                {
                    string username = this.contextAccessor.HttpContext.Session.GetString(CURRENT_USER);
                    User user = this.userService.GetByUsername(username);
                    return user;
                }
                catch (EntityNotFoundException)
                {
                    return null;
                }
            }
            set
            {
                // User
                User user = value;
                if (user != null)
                {
                    // add username to session
                    this.contextAccessor.HttpContext.Session.SetString(CURRENT_USER, user.Username);
                }
                else
                {
                    this.contextAccessor.HttpContext.Session.Remove(CURRENT_USER);
                }
            }
        }



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

    }
}
