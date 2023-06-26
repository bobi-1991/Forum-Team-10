using ForumTemplate.Exceptions;
using ForumTemplate.Models;

namespace ForumTemplate.Persistence.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new();

        public UserRepository()
        {
            this._users.Add(User.Create("borislav", "penchev", "bobi", "bobi@email", "strongPass"));
            this._users.Add(User.Create("strahil", "mladenov", "strahil", "strahil@email", "veryStrongPass"));
            this._users.Add(User.Create("iliyan", "tsvetkov", "iliyan", "iliyan@email", "strongestPass"));
        }
        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public User GetUserByEmail(string email)
        {
            var user = _users.SingleOrDefault(u => u.Email == email);
            return user;
        }
        public List<User> GetAll()
        {
            return _users;
        }
        public User GetById(Guid id)
        {
            return _users.FirstOrDefault(u => u.Id == id) ?? throw new EntityNotFoundException($"User with ID: {id} not found.");
        }

        public User GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ?? throw new EntityNotFoundException($"User with username: {username} not found.");
        }
        public User Update(Guid id, User user)
        {
            User userToUpdate = GetById(id);
            userToUpdate.Update(user);

            return userToUpdate;
        }

        public string Delete(Guid id)
        {
            User existingUser = GetById(id);
            _users.Remove(existingUser);

            return "User was successfully deleted.";
        }

        public bool DoesExist(string usernme)
        {
            return _users.Any(x => x.Username.Equals(usernme, StringComparison.OrdinalIgnoreCase));
        }
    }
}
