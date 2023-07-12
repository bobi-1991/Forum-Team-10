using ForumTemplate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;

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


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.ConfigureWarnings(warnings =>
        //        warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        //}
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
                    Password = "MTIz", //Password is "123" but encoded
                    Country = "Bulgaria",
                    IsAdmin = true,
                 },
                 User.Create("borislav", "penchev", "bobi", "bobi@email", "MTIz", "Bulgaria"),
                 User.Create("strahil", "mladenov", "strahil", "strahil@email", "MTIz", "Bulgaria"),
                 User.Create("iliyan", "tsvetkov", "iliyan", "iliyan@email", "MTIz", "Bulgaria")
            };

            modelBuilder.Entity<User>().HasData(users);

            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<Post>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<Comment>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<Like>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<Tag>().HasQueryFilter(x => !x.IsDelete);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDelete"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDelete"] = true;
                        break;
                }
            }
        }
    }
}