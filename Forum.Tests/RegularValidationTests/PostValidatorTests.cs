using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Validation;
using Moq;

namespace ForumTemplate.Tests.RegularValidationTests
{
    [TestClass]
    public class PostValidatorTests
    {

        private PostsValidator validator;

        private Mock<IPostRepository> postRepositoryMock;

        private Guid id;

        [TestInitialize]

        public void Initialize()
        {
            postRepositoryMock = new Mock<IPostRepository>();

            id = Guid.NewGuid();

            postRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(new Post());

            validator = new PostsValidator(postRepositoryMock.Object);
        }

        [TestMethod]

        public void GetById_ExistingPost_ShouldReturn()
        {
            //Act
            validator.Validate(id);

            //Verify
            postRepositoryMock.Verify(x => x.GetById(id), Times.Once);
        }

        [TestMethod]

        public void GetById_UnexistingPost_ShouldThrow()
        {
            var invalidId = Guid.NewGuid();

            var ex = Assert.ThrowsException<ValidationException>(
                () => validator.Validate(invalidId));

            Assert.AreEqual($"Post with ID: {invalidId} not found.", ex.Message);
        }
    }
}
