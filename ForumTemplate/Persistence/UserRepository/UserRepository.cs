using ForumTemplate.Data;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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
                .Where(x=>!x.IsDelete)
                .Include(x=>x.Likes)
                .Include(x=>x.Posts)
                .Include(x=>x.Comments)
                .Include(x => x.Tags)
                        .ToList();
        }
        public User GetById(Guid userId)
        {
            var user = dbContext.Users
				 .Where(x => !x.IsDelete)
				.Include(x => x.Likes)
				.Include(x => x.Posts)
				.Include(x => x.Comments)
                .Include(x => x.Tags)
                .FirstOrDefault(u => u.UserId == userId);

            if (user is null)
            {
                throw new EntityNotFoundException($"User with ID: {userId} not found.");
            }

            return user;
        }

        public User GetByUsername(string username)
        {
            var user = dbContext.Users
			    .Where(x => !x.IsDelete)
				.Include(x => x.Likes)
				.Include(x => x.Posts)
				.Include(x => x.Comments)
                .Include(x => x.Tags)
                .FirstOrDefault(u => u.Username.Equals(username));

            return user;
        }
        public List<User> SearchByAdminCriteria(string searchInfo)
        {
            var isSearch = false;
            List<User> result = this.GetAll();
            result = SearchByUsername(result, searchInfo,ref isSearch);
            result = SearchByFirstName(result, searchInfo,ref isSearch);
            result = SearchByEmail(result, searchInfo,ref isSearch);
            if (!isSearch)
            {
                return new List<User>();
            }
            return result.ToList();
        }
        public User Update(Guid id, User user)
        {
            User userToUpdate = GetById(id);
            var updatedUser = userToUpdate.Update(user);

            dbContext.Update(updatedUser);
            dbContext.SaveChanges();

            return updatedUser;
        }
        public User AdminEditionUpdate(Guid id, User user)
        {
            User userToUpdate = GetById(id);
            var updatedUser = userToUpdate.AdminEditionUpdatee(user);

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
            return dbContext.Users
                .Where(x=>!x.IsDelete)
                .Any(x => x.Username.Equals(usernme));
        }
        public bool EmailDoesExists(string email)
        {
            return dbContext.Users
                   .Where(x => !x.IsDelete)
                   .Any(x => x.Email.Equals(email));
        }
        public string RegisterUser(User user)
        {
            
            try
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return "User successfully registered.";
            }
            catch (Exception ex)
            {
                //Must be made another custom exception
                throw new ValidationException(ex.Message);
            }

        }
        
        public string PromoteUser(User user)
        {

            try
            {
                user.IsAdmin = true;
                dbContext.SaveChanges();
                return "User successfully promoted";
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        public string DemoteUser(User user)
        {

            try
            {
                user.IsAdmin = false;
                dbContext.SaveChanges();
                return "User successfully demoted";
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        public string BanUser(User user)
        {

            try
            {
                user.IsBlocked = true;
                dbContext.SaveChanges();
                return "User successfully banned";
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        public string UnBanUser(User user)
        {
            try
            {
                user.IsBlocked = false;
                dbContext.SaveChanges();
                return "User successfully UnBanned";
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

    
        public static List<User> SearchByUsername(List<User> users, string searchInfo,ref bool isSearch)
        {
            if (users.Any(x => x.Username.Equals(searchInfo)))
            {
                isSearch = true;
                return users.FindAll(x => x.Username.Equals(searchInfo));
            }

            return users;
        }
        public static List<User> SearchByFirstName(List<User> users, string searchInfo, ref bool isSearch)
        {
            if (users.Any(x => x.FirstName.Equals(searchInfo)))
            {
                isSearch = true;
                return users.FindAll(x => x.FirstName.Equals(searchInfo));
            }

            return users;
        }
        public static List<User> SearchByEmail(List<User> users, string searchInfo,ref bool isSearch)
        {

            if (users.Any(x => x.Email.Equals(searchInfo)))
            {
                isSearch = true;
                return users.FindAll(x => x.Email.Equals(searchInfo));
            }

            return users;
        }

  
    }
}
