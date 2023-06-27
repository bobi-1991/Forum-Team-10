using ForumTemplate.Exceptions;
using ForumTemplate.Models;

namespace ForumTemplate.Persistence.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new();

        public UserRepository()
        {
            this._users.Add(User.Create("borislav", "penchev", "bobi", "bobi@email", "MTIz"));
            this._users.Add(User.Create("strahil", "mladenov", "strahil", "strahil@email", "MTIz"));
            this._users.Add(User.Create("iliyan", "tsvetkov", "iliyan", "iliyan@email", "MTIz"));
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

        //Authentication

        public User Login(string username, string encodedPassword)
        {
            ////For Configured DB
            ///
            //User user;
            //try
            //{
            //    user = context.User.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(encodedPassword));
            //    if (user is null)
            //    {
            //        throw new ValidationException("User not found");
            //    }
            //    user.IsLogged = true;
            //    context.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    throw new ValidationException(ex.Message);
            //}
            //return user;

            var user = _users.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(encodedPassword));
            if (user is null)
            {
                throw new ValidationException("User not found");
            }
            user.IsLogged = true;

            return user;

        }

        public User Logout(string username)
        {
            ////For Configured DB
            ///
            //User user;
            //try
            //{
            //    user = context.User.FirstOrDefault(u => u.Username.Equals(username));
            //    if (user is null)
            //    {
            //        throw new ValidationException("User not found");
            //    }
            //    user.IsLogged = false;
            //    context.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    throw new ValidationException(ex.Message);
            //}
            //return user;

            var user = _users.FirstOrDefault(u => u.Username.Equals(username));
            if (user is null)
            {
                throw new ValidationException("User not found");
            }
            user.IsLogged = false;

            return user;
        }

        public void RegisterUser(User user)
        {
            ////For Configured DB
            ///
            //try
            //{
            //    context.User.Add(user);
            //    context.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    //Must be made another custom exception
            //    throw new ValidationException(ex.Message);
            //}

            if (DoesExist(user.Username))
            {
                throw new ValidationException("User Already Exists");
            }
            else
            {
                _users.Add(user);
            }
        }
    }
}
