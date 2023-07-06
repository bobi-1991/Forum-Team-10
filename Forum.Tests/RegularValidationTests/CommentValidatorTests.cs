using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Persistence.CommentRepository;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Validation;
using Moq;

namespace ForumTemplate.Tests.RegularValidationTests
{
    [TestClass]
    public class CommentValidatorTests
    {
        private CommentsValidator validator;

        private Mock<ICommentRepository> commentRepositoryMock;
        private Mock<IPostRepository> postRepositoryMock;

        private Guid commentId;
        private Guid postId;

        [TestInitialize]

        public void Initialize()
        {
            commentRepositoryMock = new Mock<ICommentRepository>();
            postRepositoryMock = new Mock<IPostRepository>();

            commentId = Guid.NewGuid();
            postId = Guid.NewGuid();

            postRepositoryMock
                .Setup(x => x.GetById(postId))
                .Returns(new Post());

            commentRepositoryMock
                .Setup(x => x.GetById(commentId))
                .Returns(new Comment());

            commentRepositoryMock
                .Setup(x => x.GetByPostId(postId))
                .Returns(new List <Comment>());

            validator = new CommentsValidator(postRepositoryMock.Object, commentRepositoryMock.Object);
        }

        [TestMethod]

        public void GetById_ShouldReturn_WhenCommentFound()
        {
            //Act
            validator.Validate(commentId);

            //Verify
            commentRepositoryMock.Verify(x => x.GetById(commentId), Times.Once);
        }

        [TestMethod]

        public void GetById_ShouldThrow_WhenCommentNotFound()
        {
            //Arrange
            var invalidId = Guid.NewGuid();

            //Act
            var ex = Assert.ThrowsException<ValidationException>(
                () => validator.Validate(invalidId));

            //Assert
            Assert.AreEqual($"Comment with ID: {invalidId} not found.", ex.Message);
        }

        [TestMethod]

        public void GetCommentByPostId_ShouldReturn_WhenCommentFound()
        {
            //Act
            validator.GetCommentsByPostID(postId);

            //Verify
            commentRepositoryMock.Verify(x => x.GetByPostId(postId), Times.Once);
        }

        [TestMethod]

        public void GetCommentByPostId_ShouldThrow_WhenCommentNotFound()
        {
            //Arrange
            commentRepositoryMock
                .Setup(x => x.GetByPostId(postId))
                .Returns(null as List<Comment>);

            //Act
            var ex = Assert.ThrowsException<EntityNotFoundException>(
                () => validator.GetCommentsByPostID(postId));

            //Verify
            Assert.AreEqual($"Comment with ID: {postId} not found.", ex.Message);
        }
    }
}
