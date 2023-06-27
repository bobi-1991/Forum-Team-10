using ForumTemplate.Common.FilterModels;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Persistence.UserRepository
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

        //Authentication

        User Login(string username, string encodedPassword);

        User Logout(string username);

        void RegisterUser(User user);
    }
}
