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
            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id);

            if (user is null)
            {
                throw new EntityNotFoundException($"User with ID: {id} not found.");
            }

            return user;
        }

        public User GetByUsername(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username.Equals(username));

            if(user is  null) 
            {
                throw new EntityNotFoundException($"User with username: {username} not found.");
            }

            return user;
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
                if (user.IsBlocked)
                {
                    throw new ValidationException("User is banned and cannot login, please contact support");
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
        
        public void PromoteUser(User user)
        {

            try
            {
                user.IsAdmin = true;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        public void DemoteUser(User user)
        {

            try
            {
                user.IsAdmin = false;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        public void BanUser(User user)
        {

            try
            {
                user.IsBlocked = true;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        public void UnBanUser(User user)
        {
            try
            {
                user.IsBlocked = false;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
    }
}
