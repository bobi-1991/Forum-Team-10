using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Services.LikeService;
using ForumTemplate.Services.PostService;
using ForumTemplate.Services.UserService;
using ForumTemplate.Validation;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ForumTemplate.Tests.UserServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService sut;

        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IUserMapper> userMapperMock;
        private Mock<IUserAuthenticationValidator> userValidatorMock;
        private Mock<ILikeService> likeServiceMock;
        private Mock<IPostService> postServiceMock;

        private User user;
        private Guid id;

        private const string FirstName = "Admin";
        private const string LastName = "Adminov";
        private const string UserName = "admin";
        private const string Email = "admin@forum.com";
        private const string Password = "123";
        private const string Country = "Bulgaria";
        private const bool IsAdmin = true;

        [TestInitialize]

        public void Initialize()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userMapperMock = new Mock<IUserMapper>();
            userValidatorMock = new Mock<IUserAuthenticationValidator>();
            likeServiceMock = new Mock<ILikeService>();
            postServiceMock = new Mock<IPostService>();

            id = Guid.NewGuid();

            user = new User
            {
                UserId = id,
                FirstName = FirstName,
                LastName = LastName,
                Username = UserName,
                Email = Email,
                Password = Password,
                Country = Country,
                IsAdmin = IsAdmin
            };

            userRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(user);

            userRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<User>());

            userRepositoryMock
                .Setup(x => x.Delete(id))
                .Returns("User was successfully deleted.");

            userRepositoryMock
             .Setup(x => x.RegisterUser(It.IsAny<User>()))
             .Returns("User successfully registered.");

            userRepositoryMock
                .Setup(x => x.PromoteUser(It.IsAny<User>()))
                .Returns("User successfully promoted");

            userRepositoryMock
                .Setup(x => x.DemoteUser(It.IsAny<User>()))
                .Returns("User successfully demoted");

            userRepositoryMock
                .Setup(x => x.BanUser(It.IsAny<User>()))
                .Returns("User successfully banned");

            userRepositoryMock
               .Setup(x => x.UnBanUser(It.IsAny<User>()))
               .Returns("User successfully UnBanned");

            userRepositoryMock
                .Setup(x => x.Update(id, It.IsAny<User>()))
                .Returns(new User());

            userMapperMock
                .Setup(x => x.MapToUserResponse(user))
                .Returns(new UserResponse(id, FirstName, LastName, Country, UserName, Email, new DateTime()));

            userMapperMock
                .Setup(x => x.MapToUserResponse(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse>());

            userMapperMock
                .Setup(x => x.MapToUser(It.IsAny<UpdateUserRequest>()))
                .Returns(new User());

            userValidatorMock
                .Setup(x => x.ValidateDoesExist(It.IsAny<string>()));

            userValidatorMock
                .Setup(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()));

            userValidatorMock
                .Setup(x => x.ValidateIfUsernameExist(It.IsAny<string>()));

            userValidatorMock
                .Setup(x => x.ValidateUserAlreadyAdmin(It.IsAny<User>()));

            userValidatorMock
                .Setup(x => x.ValidateUserAlreadyRegular(It.IsAny<User>()));

            userValidatorMock
                .Setup(x => x.ValidateUserAlreadyBanned(It.IsAny<User>()));

            userValidatorMock
                .Setup(x => x.ValidateUserNotBanned(It.IsAny<User>()));

            sut = new UserService(userRepositoryMock.Object, userMapperMock.Object, userValidatorMock.Object,
                likeServiceMock.Object, postServiceMock.Object);
        }

        [TestMethod]

        public void GetByUsername_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetByUsername(It.IsAny<string>());

            //Verify
            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsername(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_ShouldInvokeCorrectMethods()
        {
            //Arrange
            userMapperMock
                .Setup(x => x.MapToUserResponse(It.IsAny<User>()))
                .Returns(It.IsAny<UserResponse>());

            //Act
            var result = sut.Update(GetUser(), id, GetUpdateUserRequest());

            //Verify
            userMapperMock.Verify(x => x.MapToUser(It.IsAny<UpdateUserRequest>()), Times.Once);
            userRepositoryMock.Verify(x => x.Update(id, It.IsAny<User>()), Times.Once);
            userMapperMock.Verify(x => x.MapToUserResponse(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void GetById_ShouldReturn()
        {
            //Act
            var result = sut.GetById(id);

            //Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(FirstName, result.FirstName);
            Assert.AreEqual(LastName, result.LastName);
            Assert.AreEqual(UserName, result.Username);
            Assert.AreEqual(Email, result.Email);
            Assert.AreEqual(Country, result.Country);
        }

        [TestMethod]
        public void GetById_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetById(id);

            //Verify
            userRepositoryMock.Verify(x => x.GetById(id), Times.Once);

            userMapperMock.Verify(x => x.MapToUserResponse(user), Times.Once);
        }

        [TestMethod]
        public void DeleteByID_ShouldReturn()
        {
            //Act
            var message = sut.Delete(GetUser(), id);

            //Assert
            StringAssert.Contains(message, "User was successfully deleted.");
        }

        [TestMethod]
        public void DeleteByID_ShouldInvokeCorrectMethods()
        {
            //Act
            var message = sut.Delete(GetUser(), id);

            //Verify
            userRepositoryMock.Verify(x => x.Delete(id), Times.Once);
        }

        [TestMethod]
        public void GetALL_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetAll();

            //Verify
            userRepositoryMock.Verify(x => x.GetAll(), Times.Once);

            userMapperMock.Verify(x => x.MapToUserResponse(It.IsAny<List<User>>()), Times.Once);
        }

        [TestMethod]

        public void RegisterUser_ShouldReturn()
        {
            //Arrange
            var registerUserRequest = GetRegisterUserRequestModel();

            //Act
            var result = sut.RegisterUser(registerUserRequest, Password);

            //Assert
            StringAssert.Contains(result, "User successfully registered.");
        }

        [TestMethod]

        public void RegisterUser_ShouldInvokeCorrectMethods()
        {
            //Arrange
            var registerUserRequest = GetRegisterUserRequestModel();

            //Act
            sut.RegisterUser(registerUserRequest, Password);

            //Assert
            userValidatorMock.Verify(x => x.ValidateDoesExist(user.Username), Times.Once);

            userRepositoryMock.Verify(x => x.RegisterUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public void PromoteUser_ShouldInvokeCorrectMethods()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.GetByUsername(It.IsAny<string>()))
                .Returns(new User());

            //Act
            var user2 = sut.PromoteUser(GetUser(), GetUpdateUserRequestModel());

            //Verify
            userValidatorMock.Verify(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsername(GetUpdateUserRequestModel().UserName), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserAlreadyAdmin(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.PromoteUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void PromoteUser_ShouldReturn()
        {
            //Act
            var message = sut.PromoteUser(GetUser(), GetUpdateUserRequestModel());

            //Assert
            StringAssert.Contains(message, "User successfully promoted");
        }

        [TestMethod]
        public void DemoteUser_ShouldReturn()
        {
            //Act
            var message = sut.DemoteUser(GetUser(), GetUpdateUserRequestModel());

            //Assert
            StringAssert.Contains(message, "User successfully demoted");
        }

        [TestMethod]
        public void DemoteUser_ShouldInvokeCorrectMethods()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.GetByUsername(It.IsAny<string>()))
                .Returns(new User());

            //Act
            var user2 = sut.DemoteUser(GetUser(), GetUpdateUserRequestModel());

            //Verify
            userValidatorMock.Verify(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsername(GetUpdateUserRequestModel().UserName), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserAlreadyRegular(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.DemoteUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void BanUser_ShouldReturn()
        {
            //Act
            var message = sut.BanUser(GetUser(), GetUpdateUserRequestModel());

            //Assert
            StringAssert.Contains(message, "User successfully banned");
        }

        [TestMethod]
        public void BanUser_ShouldInvokeCorrectMethods()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.GetByUsername(It.IsAny<string>()))
                .Returns(new User());

            //Act
            var result = sut.BanUser(GetUser(), GetUpdateUserRequestModel());

            //Verify
            userValidatorMock.Verify(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsername(GetUpdateUserRequestModel().UserName), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserAlreadyBanned(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.BanUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void UnBanUser_ShouldReturn()
        {
            //Act
            var message = sut.UnBanUser(GetUser(), GetUpdateUserRequestModel());

            //Assert
            StringAssert.Contains(message, "User successfully UnBanned");
        }

        [TestMethod]
        public void UnBanUser_ShouldInvokeCorrectMethods()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.GetByUsername(It.IsAny<string>()))
                .Returns(new User());

            //Act
            var user2 = sut.UnBanUser(GetUser(), GetUpdateUserRequestModel());

            //Verify
            userValidatorMock.Verify(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsername(GetUpdateUserRequestModel().UserName), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserNotBanned(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.UnBanUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public void GetAllUsers_ShouldInvoke()
        {
            //Act
            var result = sut.GetAllUsers();

            //Assert
            userRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [TestMethod]

        public void UsernameExists_ShouldInvoke()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.DoesExist(It.IsAny<string>()))
                .Returns(new bool());

            //Act
            var result = sut.UsernameExists(It.IsAny<string>());

            //Assert
            userRepositoryMock.Verify(x => x.DoesExist(It.IsAny<string>()), Times.Once);
        }

        private User GetUser()
        {
            return new User()
            {
                UserId = id,
                FirstName = "TestTestUser",
                LastName = "TestTestUserLast",
                Username = "TestUsername",
                Email = "TestMail@abv.bg",
                Password = "1234Passw0rd@",
                Country = "BG",
            };
        }

        private UpdateUserRequest GetUpdateUserRequest()
        {
            return new UpdateUserRequest()
            {
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                Email = Email,
                Country = Country,
            };
        }
        private UpdateUserRequestModel GetUpdateUserRequestModel()
        {
            return new UpdateUserRequestModel()
            {
                UserName = "TestTest"
            };
        }

        private RegisterUserRequestModel GetRegisterUserRequestModel()
        {
            return new RegisterUserRequestModel
            {
                FirstName = FirstName,
                LastName = LastName,
                Username = UserName,
                Email = Email,
                Password = Password,
                Country = Country
            };
        }
    }
}