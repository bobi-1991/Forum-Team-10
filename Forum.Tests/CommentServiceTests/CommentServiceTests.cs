using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.CommentRepository;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Services.CommentService;
using ForumTemplate.Validation;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace ForumTemplate.Tests.CommentServiceTests
{
    [TestClass]
    public class CommentServiceTests
    {
        private CommentService sut;

        private Mock<ICommentRepository> commentRepositoryMock;
        private Mock<ICommentsValidator> commentsValidatorMock;
        private Mock<ICommentMapper> commentMapperMock;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IPostRepository> postRepositoryMock;
        private Mock<IUserAuthenticationValidator> userValidatorMock;

        private Guid id;
        private Guid postId;
        private Guid commentId;

        [TestInitialize]
        public void Initialize()
        {
            commentRepositoryMock = new Mock<ICommentRepository>();
            commentsValidatorMock = new Mock<ICommentsValidator>();
            commentMapperMock = new Mock<ICommentMapper>();
            userRepositoryMock = new Mock<IUserRepository>();
            postRepositoryMock = new Mock<IPostRepository>();
            userValidatorMock = new Mock<IUserAuthenticationValidator>();

            id = Guid.NewGuid();
            postId = Guid.NewGuid();
            commentId = Guid.NewGuid();

            SetupCommentRepositoryMock();

            SetupCommentsValidatorMock();

            SetupCommentMapperMock();

            SetupUserValidatorMock();

            SetupUserRepositoryMock();

            SetupPostRepositoryMock();

            sut = new CommentService(commentRepositoryMock.Object, commentsValidatorMock.Object,
                commentMapperMock.Object, userRepositoryMock.Object, postRepositoryMock.Object, userValidatorMock.Object);
        }

        private void SetupCommentRepositoryMock()
        {
            commentRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Comment>());

            commentRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(new Comment { PostId = postId });

            commentRepositoryMock
                .Setup(x => x.GetByPostId(It.IsAny<Guid>()))
                .Returns(new List<Comment>());

            commentRepositoryMock
                .Setup(x => x.Create(It.IsAny<Comment>()))
                .Returns(new Comment { UserId = id, PostId = postId });

            commentRepositoryMock
                .Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Comment>()))
                .Returns(new Comment());

            commentRepositoryMock
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns("Comment was successfully deleted.");

            commentRepositoryMock
                .Setup(x => x.DeleteByPostId(It.IsAny<Guid>()));
        }

        private void SetupCommentsValidatorMock()
        {
            commentsValidatorMock
                .Setup(x => x.GetCommentsByPostID(It.IsAny<Guid>()));

            commentsValidatorMock
                .Setup(x => x.Validate(It.IsAny<Guid>()));

            commentsValidatorMock
                .Setup(x => x.Validate(GetCommentRequest()));

            commentsValidatorMock
                .Setup(x => x.Validate(It.IsAny<Guid>(), GetCommentRequest()));
        }

        private void SetupCommentMapperMock()
        {
            commentMapperMock
                .Setup(x => x.MapToCommentResponse(It.IsAny<List<Comment>>()))
                .Returns(new List<CommentResponse>());

            commentMapperMock
                .Setup(x => x.MapToCommentResponse(It.IsAny<Comment>()))
                .Returns(It.IsAny<CommentResponse>);

            commentMapperMock
                .Setup(x => x.MapToComment(GetCommentRequest()))
                .Returns(new Comment());
        }

        private void SetupUserValidatorMock()
        {
            userValidatorMock
                .Setup(x => x.ValidateUserIsNotBannedCommentCreate(It.IsAny<User>()));

            userValidatorMock
                .Setup(x => x.ValidateUserIdMatchAuthorIdComment(It.IsAny<User>(), It.IsAny<Guid?>()));
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
                .Returns(new Post());

        }

        [TestMethod]

        public void GetAll_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetAll();

            //Verify
            commentRepositoryMock.Verify(x => x.GetAll(), Times.Once);

            commentMapperMock.Verify(x => x.MapToCommentResponse(It.IsAny<List<Comment>>()), Times.Once);
        }

        [TestMethod]

        public void GetComments_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetComments(postId);

            //Verify
            commentsValidatorMock.Verify(x => x.GetCommentsByPostID(postId), Times.Once);

            commentRepositoryMock.Verify(x => x.GetByPostId(postId), Times.Once);

            commentMapperMock.Verify(x => x.MapToCommentResponse(It.IsAny<List<Comment>>()), Times.Once);
        }

        [TestMethod]

        public void GetById_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetById(commentId);

            //Verify
            commentsValidatorMock.Verify(x => x.Validate(commentId), Times.Once);

            commentRepositoryMock.Verify(x => x.GetById(commentId), Times.Once);

            commentMapperMock.Verify(x => x.MapToCommentResponse(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]

        public void Create_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Create(GetUser(), GetCommentRequest());

            //Created Post Repository to simulate existing Post to Create Comment

            //Verify
            userValidatorMock.Verify(x => x.ValidateUserIsNotBannedCommentCreate(It.IsAny<User>()), Times.Once);

            commentsValidatorMock.Verify(x => x.Validate(It.IsAny<CommentRequest>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetById(id), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(postId), Times.Once);

            commentMapperMock.Verify(x => x.MapToComment(It.IsAny<CommentRequest>()), Times.Once);

            commentRepositoryMock.Verify(x => x.Create(It.IsAny<Comment>()), Times.Once);

            commentMapperMock.Verify(x => x.MapToCommentResponse(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]

        public void Create_ShouldThrowWhen_AuthorOfCommentIsNull()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(null as User);

            var sut = new CommentService(
                commentRepositoryMock.Object,
                commentsValidatorMock.Object,
                commentMapperMock.Object,
                userRepositoryMock.Object,
                postRepositoryMock.Object,
                userValidatorMock.Object);

            //Assert
            var ex = Assert.ThrowsException<EntityNotFoundException>(
                 () => sut.Create(GetUser(), GetCommentRequest()));

            Assert.AreEqual($"User with ID: {id} not found.", ex.Message);
        }

        [TestMethod]

        public void Create_ShouldThrowWhen_PostOfCommentIsNull()
        {
            //Arrange
            var postRepositoryMock = new Mock<IPostRepository>();

            postRepositoryMock
                .Setup(x => x.GetById(postId))
                .Returns(null as Post);

            var sut = new CommentService(
                commentRepositoryMock.Object,
                commentsValidatorMock.Object,
                commentMapperMock.Object,
                userRepositoryMock.Object,
                postRepositoryMock.Object,
                userValidatorMock.Object);

            //Assert
            var ex = Assert.ThrowsException<EntityNotFoundException>(
                 () => sut.Create(GetUser(), GetCommentRequest()));

            Assert.AreEqual($"Post with ID: {postId} not found.", ex.Message);
        }

        [TestMethod]

        public void Update_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Update(GetUser(), commentId, GetCommentRequest());

            //Verify
            commentsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>(), It.IsAny<CommentRequest>()), Times.Once);

            commentRepositoryMock.Verify(x => x.GetById(commentId), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserIdMatchAuthorIdComment(It.IsAny<User>(), It.IsAny<Guid?>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(postId), Times.Once);

            commentMapperMock.Verify(x => x.MapToComment(It.IsAny<CommentRequest>()), Times.Once);

            commentRepositoryMock.Verify(x => x.Update(commentId, It.IsAny<Comment>()), Times.Once);

            commentMapperMock.Verify(x => x.MapToCommentResponse(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]

        public void Update_ShouldThrowWhen_PostOfCommentIsNull()
        {
            //Arrange
            var postRepositoryMock = new Mock<IPostRepository>();

            postRepositoryMock
                .Setup(x => x.GetById(postId))
                .Returns(null as Post);

            var sut = new CommentService(
                commentRepositoryMock.Object,
                commentsValidatorMock.Object,
                commentMapperMock.Object,
                userRepositoryMock.Object,
                postRepositoryMock.Object,
                userValidatorMock.Object);

            //Assert
            var ex = Assert.ThrowsException<EntityNotFoundException>(
                 () => sut.Update(GetUser(), It.IsAny<Guid>(), GetCommentRequest()));

            Assert.AreEqual($"Post with ID: {postId} not found.", ex.Message);
        }

        [TestMethod]

        public void Delete_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Delete(GetUser(), commentId);

            //Verify
            commentRepositoryMock.Verify(x => x.GetById(commentId), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserIdMatchAuthorIdComment(It.IsAny<User>(), It.IsAny<Guid?>()), Times.Once);

            commentsValidatorMock.Verify(x => x.Validate(commentId), Times.Once);

            commentRepositoryMock.Verify(x => x.Delete(commentId), Times.Once);

            StringAssert.Contains(result, "Comment was successfully deleted.");
        }

        [TestMethod]

        public void DeleteByPostId_ShouldInvokeCorrectMethods()
        {
            //Act
            sut.DeleteByPostId(It.IsAny<Guid>());

            //Verify
            commentRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            commentRepositoryMock.Verify(x => x.DeleteByPostId(It.IsAny<Guid>()), Times.Once);
        }

        private CommentRequest GetCommentRequest()
        {
            return new CommentRequest
            {
                Content = "ContentTestTest",
                UserId = id,
                PostId = postId
            };
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
    }
}
