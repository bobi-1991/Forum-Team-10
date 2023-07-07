using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.CommentRepository;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Services.CommentService;
using ForumTemplate.Validation;
using Moq;

namespace ForumTemplate.Tests.CommentServiceTests
{
    [TestClass]
    public class CommentServiceTests
    {
        private CommentService sut;

        private Mock<ICommentRepository> commentRepositoryMock;
        private Mock<ICommentsValidator> commentsValidatorMock;
        private Mock<ICommentMapper> commentMapperMock;
        private Mock<IUserRepository> userRepositotyMock;
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
            userRepositotyMock = new Mock<IUserRepository>();
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
                commentMapperMock.Object, userRepositotyMock.Object, postRepositoryMock.Object, userValidatorMock.Object);
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
            userRepositotyMock
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

            commentsValidatorMock.Verify(x => x.Validate(GetCommentRequest()), Times.Once);

            userRepositotyMock.Verify(x => x.GetById(id), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(postId), Times.Once);

            commentMapperMock.Verify(x => x.MapToComment(GetCommentRequest()), Times.Once);

            commentRepositoryMock.Verify(x => x.Create(It.IsAny<Comment>()), Times.Once);

            commentMapperMock.Verify(x => x.MapToCommentResponse(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]

        public void Update_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Update(GetUser(), commentId, GetCommentRequest());

            //Verify
            commentsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>(), GetCommentRequest()), Times.Once);

            commentRepositoryMock.Verify(x => x.GetById(commentId), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserIdMatchAuthorIdComment(It.IsAny<User>(), It.IsAny<Guid?>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(postId), Times.Once);

            commentMapperMock.Verify(x => x.MapToComment(GetCommentRequest()), Times.Once);

            commentRepositoryMock.Verify(x => x.Update(commentId, It.IsAny<Comment>()), Times.Once);

            commentMapperMock.Verify(x => x.MapToCommentResponse(It.IsAny<Comment>()), Times.Once);
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

        private CommentRequest GetCommentRequest()
        {
            return new CommentRequest
            (
                "ContentTestTest",
                id,
                postId
            );
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
