using ForumTemplate.Exceptions;
using ForumTemplate.Models;

namespace ForumTemplate.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly List<User> users;

        public UserRepository()
        {
            this.users = new List<User>()
            {
                new User
                {
                    Id = 1,
                    FirstName = "Iliyan",
                    LastName = "Tsvetkov",
                    Username = "Power",
                    Password = "123",
                    Country = "Bulgaria",
                    Email = "Power@abv.bg"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Bobi",
                    LastName = "Penchev",
                    Username = "Power2",
                    Password = "123",
                    Country = "Bulgaria",
                    Email = "Power2@abv.bg"
                },
                new User
                {
                    Id = 3,
                    FirstName = "Strahil",
                    LastName = "Mladenov",
                    Username = "Power3",
                    Password = "123",
                    Country = "Bulgaria",
                    Email = "Power3@abv.bg"
                }
            };
        }

        public List<User> GetAll()
        {
            return users;
        }

        public User GetById(int id)
        {
            return users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByUsername(string name)
        {
            return users.FirstOrDefault(u => u.Username.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public User Create(User user)
        {
            users.Add(user);
            user.Id = users.Count;

            return user;
        }

        public User Update(int id, User user)
        {
            User userToUpdate = GetById(id);
            userToUpdate.Password = user.Password;

            return userToUpdate;
        }

        public string Delete(int id)
        {
            User existingUser = GetById(id);
            users.Remove(existingUser);

            return "User was successfully deleted.";
        }

        public bool DoesExist(string usernme)
        {
            return users.Any(x => x.Username.Equals(usernme, StringComparison.OrdinalIgnoreCase));
        }
    }
}
