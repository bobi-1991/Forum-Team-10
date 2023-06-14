

using ForumTemplate.Models;

namespace ForumTemplate.Repositories.UserNewPersistence

{
    public interface IUserNewRepository
    {
        // implement crud operations
        UserNew GetUserByEmail(string email);
        void AddUser(UserNew user);
    }
}
