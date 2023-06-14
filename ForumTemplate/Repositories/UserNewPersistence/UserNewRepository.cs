using ForumTemplate.Models;

namespace ForumTemplate.Repositories.UserNewPersistence
{
    public class UserNewRepository : IUserNewRepository
    {
        private static List<UserNew> _users = new(); 
        public void AddUser(UserNew user)
        {
            _users.Add(user);
        }

        public UserNew GetUserByEmail(string email)
        {
            var user = _users.SingleOrDefault(u => u.Email == email);
            return user;
        }
    }
}
