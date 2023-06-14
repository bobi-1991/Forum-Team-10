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
            if(userRepository.GetUserByEmail(request.Email) is not UserNew user)
            {
                return null;
            }
            
            var response = new AuthenticationResponse(
                user.Id.ToString(),
                user.FirstName,
                user.LastName,
                user.Username,
                user.Email,
                "toukenche");

            return response;
        }

        public AuthenticationResponse Register(RegisterRequest request)
        {
            var user = UserNew.Create(
                request.FirstName,
                request.LastName,
                request.Username,
                request.Email,
                request.Password);

           userRepository.AddUser(user);

            var response = new AuthenticationResponse(
                user.Id.ToString(),
                user.FirstName,
                user.LastName,
                user.Username,
                user.Email,
                "toukenche");

            return response;
        }
    }
}
