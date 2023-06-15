

using ForumTemplate.Models;

namespace ForumTemplate.Repositories.UserPersistence

{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        void AddUser(User user);
        List<User> GetAll();
        User GetById(Guid id);
        User GetByUsername(string name);
        User Update(Guid id, User user);
        string Delete(Guid id);
        bool DoesExist(string usernme);
    }
}
