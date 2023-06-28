using ForumTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumTemplate.Data
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //seed users
            var users = new List<User>()
            {
                 new User()
                 {
                    UserId = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "Adminov",
                    Username = "admin",
                    Email = "admin@forum.com",
                    Password = "strongPass",
                    Country = "Bulgaria",
                    IsAdmin = true,
                 },
                 User.Create("borislav", "penchev", "bobi", "bobi@email", "MTIz", "Bulgaria"),
                 User.Create("strahil", "mladenov", "strahil", "strahil@email", "MTIz", "Bulgaria"),
                 User.Create("iliyan", "tsvetkov", "iliyan", "iliyan@email", "MTIz", "Bulgaria")
            };

            modelBuilder.Entity<User>().HasData(users);
        }
    }
}