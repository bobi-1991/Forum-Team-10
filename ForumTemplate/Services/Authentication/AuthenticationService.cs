using ForumTemplate.DTOs.Authentication;
using ForumTemplate.Models;
using ForumTemplate.Repositories.UserPersistence;


namespace ForumTemplate.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public AuthenticationResponse Login(LoginRequest request)
        {
            var user = userRepository.GetUserByEmail(request.Email);

            if (user == null)
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
            var user = User.Create(
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
