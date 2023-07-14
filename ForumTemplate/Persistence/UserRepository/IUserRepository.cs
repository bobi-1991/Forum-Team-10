using ForumTemplate.Models;

namespace ForumTemplate.Persistence.UserRepository
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        void AddUser(User user);
        List<User> GetAll();
        List<User> SearchByAdminCriteria(string searchInfo);
        User GetById(Guid id);
        User GetByUsername(string name);
        User Update( Guid id, User user);
        User AdminEditionUpdate(Guid id, User adminEditViewModel);
        string Delete( Guid id);
        bool DoesExist(string usernme);
        bool EmailDoesExists(string email);
        string RegisterUser(User user);
        string PromoteUser(User user);
        string DemoteUser(User user);
        string BanUser(User user);
        string UnBanUser(User user);

    }
}
