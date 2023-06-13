using ForumTemplate.Models;

namespace ForumTemplate.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();

        User GetById(int id);

        User GetByUsername(string name);

        User Create(User user);

        User Update(int id, User user);

        string Delete(int id);

        bool DoesExist(string usernme);

    }
}
