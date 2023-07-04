using ForumTemplate.DTOs.Authentication;
using ForumTemplate.DTOs.UserDTOs;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Services.UserService;
using ForumTemplate.Validation;
using Moq;

namespace ForumTemplate.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService sut;

        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IUserMapper> userMapperMock;
        private Mock<IUserAuthenticationValidator> userValidatorMock;

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

            userValidatorMock
                .Setup(x => x.ValidateUserIsLoggedAndAdmin());

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
                .Setup(x => x.Login("titi", It.IsAny<string>()))
                .Returns(user);

            userRepositoryMock
                .Setup(x => x.Logout(It.IsAny<string>()))
                .Returns(user);

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

            //sut = new UserService(userRepositoryMock.Object, userMapperMock.Object, userValidatorMock.Object);
        }

        //public UserResponse Update(Guid id, UpdateUserRequest updateUserRequest)
        //{
        //    userValidator.ValidateByGUIDUserLoggedAndAdmin(id);

        //    var userData = this.userMapper.MapToUser(updateUserRequest);
        //    var user = userRepository.Update(id, userData);

        //    return userMapper.MapToUserResponse(user);
        //}

        [TestMethod]
        public void UpdateUser_ShouldInvokeCorrectMethods()
        {
            //Arrange
            //userMapperMock
            //    .Setup(x => x.MapToUserResponse(It.IsAny<User>()))
            //    .Returns(It.IsAny<UserResponse>());

            //Act
            var result = sut.Update(id, GetUpdateUserRequest());

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
            userValidatorMock.Verify(x => x.ValidateUserIsLoggedAndAdmin(), Times.Once);

            userRepositoryMock.Verify(x => x.GetById(id), Times.Once);

            userMapperMock.Verify(x => x.MapToUserResponse(user), Times.Once);
        }

        [TestMethod]
        public void DeleteByID_ShouldReturn()
        {
            //Act
            var message = sut.Delete(id);

            //Assert
            StringAssert.Contains(message, "User was successfully deleted.");
        }

        [TestMethod]
        public void DeleteByID_ShouldInvokeCorrectMethods()
        {
            //Act
            var message = sut.Delete(id);

            //Verify
            userRepositoryMock.Verify(x => x.Delete(id), Times.Once);
        }

        [TestMethod]
        public void GetALL_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetAll();

            //Verify
            userValidatorMock.Verify(x => x.ValidateUserIsLoggedAndAdmin(), Times.Once);

            userRepositoryMock.Verify(x => x.GetAll(), Times.Once);

            userMapperMock.Verify(x => x.MapToUserResponse(It.IsAny<List<User>>()), Times.Once);
        }

        [TestMethod]

        public void Login_ShouldReturn()
        {
            //Act
            var result = sut.Login("titi", "123123");

            //Assert
            userRepositoryMock.Verify(x => x.Login("titi", "123123"), Times.Once);
        }

        [TestMethod]

        public void Logout_ShouldReturn()
        {
            //Act
            var result = sut.Logout("titi");

            //Assert
            userRepositoryMock.Verify(x => x.Logout("titi"), Times.Once);
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
            userRepositoryMock.Verify(x => x.RegisterUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public void PromoteUser_ShouldInvokeCorrectMethods()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.GetByUsername(It.IsAny<string>()))
                .Returns(new User());

            var updateUserRequestModel = GetUpdateUserRequestModel();

            //Act
            var user2 = sut.PromoteUser("TestTest", updateUserRequestModel);

            //Verify
            userRepositoryMock.Verify(x => x.GetByUsername(updateUserRequestModel.UserName), Times.Once);
            userRepositoryMock.Verify(x => x.PromoteUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void PromoteUser_ShouldReturn()
        {
            //Arrange
            var updateUserRequestModel = GetUpdateUserRequestModel();

            //Act
            var message = sut.PromoteUser("TestTest", updateUserRequestModel);

            //Assert
            StringAssert.Contains(message, "User successfully promoted");
        }

        [TestMethod]
        public void DemoteUser_ShouldReturn()
        {
            //Arrange
            var updateUserRequestModel = GetUpdateUserRequestModel();

            //Act
            var message = sut.DemoteUser("TestTest", updateUserRequestModel);

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

            var updateUserRequestModel = GetUpdateUserRequestModel();

            //Act
            var user2 = sut.DemoteUser("TestTest", updateUserRequestModel);

            //Verify
            userRepositoryMock.Verify(x => x.GetByUsername(updateUserRequestModel.UserName), Times.Once);
            userRepositoryMock.Verify(x => x.DemoteUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void BanUser_ShouldReturn()
        {
            //Arrange
            var updateUserRequestModel = GetUpdateUserRequestModel();

            //Act
            var message = sut.BanUser("TestTest", updateUserRequestModel);

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

            var updateUserRequestModel = GetUpdateUserRequestModel();

            //Act
            var user2 = sut.BanUser("TestTest", updateUserRequestModel);

            //Verify
            userRepositoryMock.Verify(x => x.GetByUsername(updateUserRequestModel.UserName), Times.Once);
            userRepositoryMock.Verify(x => x.BanUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void UnBanUser_ShouldReturn()
        {
            //Arrange
            var updateUserRequestModel = GetUpdateUserRequestModel();

            //Act
            var message = sut.UnBanUser("TestTest", updateUserRequestModel);

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

            var updateUserRequestModel = GetUpdateUserRequestModel();

            //Act
            var user2 = sut.UnBanUser("TestTest", updateUserRequestModel);

            //Verify
            userRepositoryMock.Verify(x => x.GetByUsername(updateUserRequestModel.UserName), Times.Once);
            userRepositoryMock.Verify(x => x.UnBanUser(It.IsAny<User>()), Times.Once);
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