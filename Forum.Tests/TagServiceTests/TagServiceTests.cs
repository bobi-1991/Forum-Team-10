using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.TagRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Services.TagService;
using ForumTemplate.Validation;
using Moq;

namespace ForumTemplate.Tests.TagServiceTests
{
    [TestClass]
    public class TagServiceTests
    {
        private TagService sut;

        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IPostRepository> postRepositoryMock;
        private Mock<IUserAuthenticationValidator> userValidatorMock;
        private Mock<ITagRepository> tagRepositoryMock;
        private Mock<ITagsValidator> tagValidatorMock;
        private Mock<ITagMapper> tagMapperMock;

        private Guid userId;
        private Guid tagId;
        private Guid postId;

        [TestInitialize]

        public void Initialize()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            postRepositoryMock = new Mock<IPostRepository>();
            userValidatorMock = new Mock<IUserAuthenticationValidator>();
            tagRepositoryMock = new Mock<ITagRepository>();
            tagValidatorMock = new Mock<ITagsValidator>();
            tagMapperMock = new Mock<ITagMapper>();

            userId = Guid.NewGuid();
            tagId = Guid.NewGuid();
            postId = Guid.NewGuid();

            SetupUserRepositoryMock();

            SetupPostRepositoryMock();

            SetupUserValidatorMock();

            SetupTagRepositoryMock();

            SetupTagValidatorMock();

            SetupTagMapperMock();

            sut = new TagService(userRepositoryMock.Object,
                postRepositoryMock.Object,
                userValidatorMock.Object,
                tagRepositoryMock.Object,
                tagValidatorMock.Object,
                tagMapperMock.Object);
        }

        private void SetupUserRepositoryMock()
        {
            userRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(new User());
        }

        private void SetupPostRepositoryMock()
        {
            postRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(new Post()
                );
        }

        private void SetupUserValidatorMock()
        {
            userValidatorMock
                .Setup(x => x.ValidateUserIdMatchAuthorIdTag(It.IsAny<User>(), It.IsAny<Guid?>()));

            userValidatorMock
                .Setup(x => x.ValidateUserIsNotBannedTagCreate(It.IsAny<User>()));
        }

        private void SetupTagRepositoryMock()
        {
            tagRepositoryMock
                .Setup(x => x.GetByPostId(It.IsAny<Guid>()))
                .Returns(new List<Tag>());

            tagRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(new Tag());

            tagRepositoryMock
                .Setup(x => x.DeleteByPostId(It.IsAny<Guid>()));

            tagRepositoryMock
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns("Tag was successfully deleted.");

            tagRepositoryMock
                .Setup(x => x.Create(It.IsAny<Tag>()))
                .Returns(new Tag());
        }

        private void SetupTagValidatorMock()
        {
            tagValidatorMock
                .Setup(x => x.Validate(It.IsAny<Guid>()));

            tagValidatorMock
                .Setup(x => x.Validate(It.IsAny<TagRequest>()));
        }

        private void SetupTagMapperMock()
        {
            tagMapperMock
                .Setup(x => x.MapToTagResponse(It.IsAny<List<Tag>>()))
                .Returns(new List<TagResponse>());

            tagMapperMock
                .Setup(x => x.MapToTagResponse(It.IsAny<Tag>()))
                .Returns(It.IsAny<TagResponse>());

            tagMapperMock
                .Setup(x => x.MapToTag(It.IsAny<TagRequest>()))
                .Returns(new Tag());
        }

        [TestMethod]

        public void GetTagsByPostId_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetTagsByPostId(postId);

            //Assert
            tagRepositoryMock.Verify(x => x.GetByPostId(It.IsAny<Guid>()), Times.Once);

            tagMapperMock.Verify(x => x.MapToTagResponse(It.IsAny<List<Tag>>()), Times.Once);
        }

        [TestMethod]

        public void GetById_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetById(tagId);

            //Assert
            tagRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            tagMapperMock.Verify(x => x.MapToTagResponse(It.IsAny<Tag>()), Times.Once);
        }

        [TestMethod]

        public void DeleteByPostId_ShouldInvokeCorrectMethods()
        {
            //Act
            sut.DeleteByPostId(tagId);

            //Assert
            tagRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            tagRepositoryMock.Verify(x => x.DeleteByPostId(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]

        public void Delete_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Delete(It.IsAny<User>(), It.IsAny<Guid>());

            //Assert
            tagValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            tagRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserIdMatchAuthorIdTag(It.IsAny<User>(), It.IsAny<Guid?>()), Times.Once);

            tagRepositoryMock.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]

        public void Delete_ShouldReturnCorrectResult()
        {
            //Act
            var result = sut.Delete(It.IsAny<User>(), It.IsAny<Guid>());

            //Assert
            Assert.AreEqual("Tag was successfully deleted.", result);
        }

        [TestMethod]

        public void Create_ShouldInvokeCorrectMethods_WhenAllIsValid()
        {
            //Arrange
            postRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(new Post()
                {
                    PostId = postId,
                    UserId = userId
                });

            //Act
            var result = sut.Create(GetUser(), GetTagRequest());

            //Assert
            userValidatorMock.Verify(x => x.ValidateUserIsNotBannedTagCreate(It.IsAny<User>()), Times.Once);

            tagValidatorMock.Verify(x => x.Validate(It.IsAny<TagRequest>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(postId), Times.Once);

            tagMapperMock.Verify(x => x.MapToTag(It.IsAny<TagRequest>()), Times.Once);

            tagRepositoryMock.Verify(x => x.Create(It.IsAny<Tag>()), Times.Once);

            tagMapperMock.Verify(x => x.MapToTagResponse(It.IsAny<Tag>()), Times.Once);
        }

        [TestMethod]

        public void Create_ShouldThrow_WhenAuthorOfPostIsNull()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(null as User);

            //Act && Assert
            //Assert
            var ex = Assert.ThrowsException<EntityNotFoundException>(
                 () => sut.Create(It.IsAny<User>(), GetTagRequest()));

            Assert.AreEqual($"User with ID: {userId} not found.", ex.Message);
        }

        [TestMethod]

        public void Create_ShouldThrow_WhenPostOfTagIsNull()
        {
            //Arrange
            postRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(null as Post);

            //Act && Assert
            //Assert
            var ex = Assert.ThrowsException<EntityNotFoundException>(
                 () => sut.Create(It.IsAny<User>(), GetTagRequest()));

            Assert.AreEqual($"Post with ID: {postId} not found.", ex.Message);
        }

        [TestMethod]

        public void Create_ShouldThrow_WhenPostOfTagUserId_DoNotMatch_TagRequestUserId()
        {
            //Assert && Act
            var ex = Assert.ThrowsException<EntityNotFoundException>(
                () => sut.Create(GetUser(), GetTagRequest()));

            Assert.AreEqual($"You Cannot Write Tag on someone else's post", ex.Message);
        }
        
        private TagRequest GetTagRequest()
        {
            return new TagRequest
            {
                Content = "Fitness",
                UserId = userId,
                PostId = postId
            };
        }

        private User GetUser()
        {
            return new User()
            {
                UserId = userId,
                FirstName = "TestTestUser",
                LastName = "TestTestUserLast",
                Username = "TestUsername",
                Email = "TestMail@abv.bg",
                Password = "1234Passw0rd@",
                Country = "BG",
            };
        }
    }
}
