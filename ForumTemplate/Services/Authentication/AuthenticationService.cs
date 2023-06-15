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
            // TODO: 

            if(userRepository.GetUserByEmail(request.Email) is not User user)
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

            var user = User.Create(
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
