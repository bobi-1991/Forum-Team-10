using ForumTemplate.Data;
using ForumTemplate.Models;
using ForumTemplate.Persistence.UserRepository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumTemplate.Tests.RepositoryTests
{
    [TestClass]
    public class RepoTest
    {
        //[TestMethod]

        //public void Test_Create_User()
        //{
        //    var mockSet = new Mock<DbSet<User>>();

        //    var mockContext = new Mock<IApplicationContext>();

        //    mockContext
        //        .Setup(x => x.Users)
        //        .Returns(mockSet.Object);
                

        //    var user = new User();

        //    var sut = new UserRepository(mockContext.Object);

        //    sut.AddUser(user);

        //    mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());

        //    mockContext.Verify(m => m.SaveChanges(), Times.Once());
            
        //}

        //private DbContext GetDatabaseContext()
        //{
        //    var options = new DbContextOptionsBuilder<DbContext>()
        //        .UseInMemoryDatabase(ApplicationContext: Guid.NewGuid().ToString())
        //        .Options;
        //    var databaseContext = new DbContext(options);
        //    databaseContext.Database.EnsureCreated();
        //    if (awaiDbContext.Users.Count <= 0)
        //    {
        //        for (int i = 1; i <= 10; i++)
        //        {
        //            databaseContext.Users.Add(new User()
        //            {
        //                Id = i,
        //                Email = $"testuser{i}@example.com",
        //                IsLocked = false,
        //                CreatedBy = "SYSTEM",
        //                CreatedDate = DateTime.UtcNow
        //            });
        //            await databaseContext.SaveChangesAsync();
        //        }
        //    }
        //    return databaseContext;
        //}
    }
}
