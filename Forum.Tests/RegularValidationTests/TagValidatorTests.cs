using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.TagRepository;
using ForumTemplate.Validation;
using Moq;

namespace ForumTemplate.Tests.RegularValidationTests
{
    [TestClass]
    public class TagValidatorTests
    {
        private TagsValidator validator;

        private Mock<ITagRepository> tagRepositoryMock;
        private Mock<IPostRepository> postRepositoryMock;

        private Guid tagId;
        private Guid postId;

        [TestInitialize]

        public void Initialize()
        {
            tagRepositoryMock = new Mock<ITagRepository>();
            postRepositoryMock = new Mock<IPostRepository>();

            tagId = Guid.NewGuid();
            postId = Guid.NewGuid();

            postRepositoryMock
                .Setup(x => x.GetById(postId))
                .Returns(new Post());

            tagRepositoryMock
                .Setup(x => x.GetById(tagId))
                .Returns(new Tag());

            tagRepositoryMock
                .Setup(x => x.GetByPostId(postId))
                .Returns(new List<Tag>());

            validator = new TagsValidator(postRepositoryMock.Object, tagRepositoryMock.Object);
        }

        [TestMethod]

        public void GetById_ShouldReturn_WhenTagFound()
        {
            //Act
            validator.Validate(tagId);

            //Verify
            tagRepositoryMock.Verify(x => x.GetById(tagId), Times.Once);
        }

        [TestMethod]

        public void GetById_ShouldThrow_WhenTagNotFound()
        {
            //Arrange
            var invalidId = Guid.NewGuid();

            //Act
            var ex = Assert.ThrowsException<ValidationException>(
                () => validator.Validate(invalidId));

            //Assert
            Assert.AreEqual($"Tag with ID: {invalidId} not found.", ex.Message);
        }

        [TestMethod]

        public void GetTagByPostId_ShouldReturn_WhenTagFound()
        {
            //Act
            validator.GetTagsByPostID(postId);

            //Verify
            tagRepositoryMock.Verify(x => x.GetByPostId(postId), Times.Once);
        }

        [TestMethod]

        public void GetTagByPostId_ShouldThrow_WhenTagNotFound()
        {
            //Arrange
            tagRepositoryMock
                .Setup(x => x.GetByPostId(postId))
                .Returns(null as List<Tag>);

            //Act
            var ex = Assert.ThrowsException<EntityNotFoundException>(
                () => validator.GetTagsByPostID(postId));

            //Verify
            Assert.AreEqual($"Tag with ID: {postId} not found.", ex.Message);
        }
    }
}
