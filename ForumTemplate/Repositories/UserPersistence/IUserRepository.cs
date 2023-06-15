

using ForumTemplate.Models;

namespace ForumTemplate.Repositories.UserPersistence

{
    public interface IUserRepository
    {
        // implement crud operations
        User GetUserByEmail(string email);
        void AddUser(User user);

        //after update
        List<User> GetAll();
        User GetById(Guid id);
        User GetByUsername(string name);
        User Update(Guid id, User user);

        string Delete(Guid id);

        bool DoesExist(string usernme);
    }
}
