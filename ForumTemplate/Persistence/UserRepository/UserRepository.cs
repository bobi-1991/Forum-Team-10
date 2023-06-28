using ForumTemplate.Data;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumTemplate.Persistence.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new();

        private readonly ApplicationContext dbContext;

        public UserRepository(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public UserRepository()
        {
            this._users.Add(User.Create("borislav", "penchev", "bobi", "bobi@email", "MTIz", "Bulgaria"));
            this._users.Add(User.Create("strahil", "mladenov", "strahil", "strahil@email", "MTIz", "Bulgaria"));
            this._users.Add(User.Create("iliyan", "tsvetkov", "iliyan", "iliyan@email", "MTIz", "Bulgaria"));
        }
        public void AddUser(User user)
        {
            _users.Add(user);

            //dbContext.Users.Add(user);
            //dbContext.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            var user = _users.SingleOrDefault(u => u.Email == email);
            return user;

            //var user = dbContext.Users.SingleOrDefault(x => x.Email == email);
            //return user;
        }
        public List<User> GetAll()
        {
            return _users;

          //  return dbContext.Users
          //.ToList();
        }
        public User GetById(Guid id)
        {
            return _users.FirstOrDefault(u => u.UserId == id) ?? throw new EntityNotFoundException($"User with ID: {id} not found.");

            //return dbContext.Users.FirstOrDefault(x => x.UserId == id) ?? throw new EntityNotFoundException($"User with ID: {id} not found.");
        }

        public User GetByUsername(string username)
        {
            return _users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ?? throw new EntityNotFoundException($"User with username: {username} not found.");

            //return dbContext.Users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ?? throw new EntityNotFoundException($"User with username: {username} not found.");
        }
        public User Update(Guid id, User user)
        {
            User userToUpdate = GetById(id);
            userToUpdate.Update(user);

            return userToUpdate;

            //User userToUpdate = GetById(id);
            //var updatedUser = userToUpdate.Update(user);

            //dbContext.Update(updatedUser);
            //dbContext.SaveChanges();

            //return updatedUser;
        }

        public string Delete(Guid id)
        {
            User existingUser = GetById(id);
            _users.Remove(existingUser);

            return "User was successfully deleted.";


            //var user = dbContext.Users.FirstOrDefault(x => x.UserId == id);

            //if (user != null)
            //{
            //    user.IsDelete = true;

            //    dbContext.SaveChanges();
            //}

            //return "User was successfully deleted.";
        }

        public bool DoesExist(string usernme)
        {
            return _users.Any(x => x.Username.Equals(usernme, StringComparison.OrdinalIgnoreCase));

          //  return dbContext.Users.Any(x => x.Username.Equals(usernme));
        }

        //Authentication

        public User Login(string username, string encodedPassword)
        {
            ////For Configured DB
            ///
            User user;
            try
            {
                user = dbContext.Users.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(encodedPassword));
                if (user is null)
                {
                    throw new ValidationException("User not found");
                }
                user.IsLogged = true;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
            return user;

            //var user = _users.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(encodedPassword));
            //if (user is null)
            //{
            //    throw new ValidationException("User not found");
            //}
            //user.IsLogged = true;

            //return user;

        }

        public User Logout(string username)
        {
            ////For Configured DB
            ///
            User user;
            try
            {
                user = dbContext.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user is null)
                {
                    throw new ValidationException("User not found");
                }
                user.IsLogged = false;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
            return user;

            //var user = _users.FirstOrDefault(u => u.Username.Equals(username));
            //if (user is null)
            //{
            //    throw new ValidationException("User not found");
            //}
            //user.IsLogged = false;

            //return user;
        }

        public void RegisterUser(User user)
        {
            ////For Configured DB
            ///
            try
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //Must be made another custom exception
                throw new ValidationException(ex.Message);
            }

            //if (DoesExist(user.Username))
            //{
            //    throw new ValidationException("User Already Exists");
            //}
            //else
            //{
            //    _users.Add(user);
            //}
        }
    }
}
