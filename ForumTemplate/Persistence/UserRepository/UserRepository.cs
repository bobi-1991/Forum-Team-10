using ForumTemplate.Data;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumTemplate.Persistence.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext dbContext;

        public UserRepository(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            var user = dbContext.Users.SingleOrDefault(x => x.Email == email);
            return user;
        }
        public List<User> GetAll()
        {
            return dbContext.Users
          .ToList();
        }
        public User GetById(Guid id)
        {
            return dbContext.Users.FirstOrDefault(x => x.UserId == id) ?? throw new EntityNotFoundException($"User with ID: {id} not found.");
        }

        public User GetByUsername(string username)
        {
            return dbContext.Users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ?? throw new EntityNotFoundException($"User with username: {username} not found.");
        }
        public User Update(Guid id, User user)
        {
            User userToUpdate = GetById(id);
            var updatedUser = userToUpdate.Update(user);

            dbContext.Update(updatedUser);
            dbContext.SaveChanges();

            return updatedUser;
        }

        public string Delete(Guid id)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.UserId == id);

            if (user != null)
            {
                user.IsDelete = true;

                dbContext.SaveChanges();
            }

            return "User was successfully deleted.";
        }

        public bool DoesExist(string usernme)
        {
            return dbContext.Users.Any(x => x.Username.Equals(usernme));
        }

        //Authentication

        public User Login(string username, string encodedPassword)
        {  
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
        }

        public User Logout(string username)
        {    
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
        }

        public void RegisterUser(User user)
        {
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

        }
    }
}
