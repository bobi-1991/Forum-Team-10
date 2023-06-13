using ForumTemplate.Models;

namespace ForumTemplate.Services
{
    public interface IUserService
    {
        List<User> GetAll();

        User GetById(int id);

        User Create(User beer);

        User Update(int id, User beer);

        string Delete(int id);


    }
}
