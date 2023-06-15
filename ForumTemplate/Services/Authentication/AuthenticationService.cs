using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;
using ForumTemplate.Repositories.UserNewPersistence;
using ForumTemplate.Services.Interfaces;

namespace ForumTemplate.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserNewRepository userRepository;

        public AuthenticationService(IUserNewRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public AuthenticationResponse Login(LoginRequest request)
        {
            // TODO: 

            if(userRepository.GetUserByEmail(request.Email) is not UserNew user)
            {
                return null;
            }

            if(!user.Password.Equals(request.Password))
            {
                return null;
            }

            // TODO: Generate token

            return new AuthenticationResponse(
                user,
                "toukenche");
        }

        public AuthenticationResponse Register(RegisterRequest request)
        {
            if (userRepository.GetUserByEmail(request.Email) != null)
            {
                return null;
            }

            var user = UserNew.Create(
                request.FirstName,
                request.LastName,
                request.Username,
                request.Email,
                request.Password);

           userRepository.AddUser(user);

            // TODO: Generate token

            return new AuthenticationResponse(
                user,
                "toukenche");
        }
    }
}
