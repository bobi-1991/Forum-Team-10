using ForumTemplate.Models;
using ForumTemplate.Persistence.LikeRepository;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Services.LikeService;
using ForumTemplate.Validation;
using Moq;

namespace ForumTemplate.Tests.LikeServiceTests
{
    [TestClass]
    public class LikeServiceTests
    {
        private LikeService sut;

        private Mock<IPostRepository> postRepositoryMock;
        private Mock<ILikeRepository> likeRepositoryMock;
        private Mock<IPostsValidator> postsValidatorMock;

        private Guid userId;
        private Guid postId;

        [TestInitialize]
        public void Initialize()
        {
            postRepositoryMock = new Mock<IPostRepository>();
            likeRepositoryMock = new Mock<ILikeRepository>();
            postsValidatorMock = new Mock<IPostsValidator>();

            userId = Guid.NewGuid();
            postId = Guid.NewGuid();

            postsValidatorMock
                .Setup(x => x.Validate(It.IsAny<Guid>()));

            postRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>()))
                .Returns(new Post { PostId = postId });

            likeRepositoryMock
                .Setup(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()))
                .Returns(new Like());

            likeRepositoryMock
                .Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(new Like());

            likeRepositoryMock
                .Setup(x => x.AddLikeInDatabase(It.IsAny<Like>()));

            likeRepositoryMock
                .Setup(x => x.UpdateInDatabase(It.IsAny<Like>()));

            sut = new LikeService(postRepositoryMock.Object, likeRepositoryMock.Object, postsValidatorMock.Object);
        }

        [TestMethod]

        public void DeleteByPostId_ShouldInvoke()
        {
            likeRepositoryMock
                .Setup(x => x.DeleteByPostId(It.IsAny<Guid>()))
                .Returns(new List<Like>());

            //Act
            sut.DeleteByPostId(postId);

            //verify
            likeRepositoryMock.Verify(x => x.DeleteByPostId(postId), Times.Once);
        }

        [TestMethod]

        public void DeleteByUserId_ShouldInvoke()
        {
            likeRepositoryMock
                .Setup(x => x.DeleteByUserId(It.IsAny<Guid>()))
                .Returns(new List<Like>());

            //Act
            sut.DeleteByUserId(userId);

            //verify
            likeRepositoryMock.Verify(x => x.DeleteByUserId(userId), Times.Once);
        }

        [TestMethod]

        public void LikeUnlike_ShouldInvokeCorrectMethods_WhenNotNullOrLikedFalse()
        {
            //Act
            var result = sut.LikeUnlike(GetUser(), postId);

            //Verify
            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.UpdateInDatabase(It.IsAny<Like>()), Times.Once);
        }

        [TestMethod]

        public void LikeUnLike_ShouldInvokeCorrectMethods_WhenLikeIsNull()
        {
            //Arrange
            likeRepositoryMock
                .Setup(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()))
                .Returns(null as Like);

            //Act
            var result = sut.LikeUnlike(GetUser(), postId);

            //Verify
            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.Create(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.AddLikeInDatabase(It.IsAny<Like>()), Times.Once);
        }

        [TestMethod]

        public void LikeUnLike_ShouldInvokeCorrectMethods_WhenLikedIsFalse()
        {
            //Arrange
            likeRepositoryMock
                .Setup(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()))
                .Returns(new Like { Liked = false });

            //Act
            var result = sut.LikeUnlike(GetUser(), postId);

            //Verify
            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.UpdateInDatabase(It.IsAny<Like>()), Times.Once);
        }

        [TestMethod]

        public void LikeUnLike_ShouldReturnMessage_WhenLikeNull()
        {
            //Arrange
            likeRepositoryMock
                .Setup(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()))
                .Returns(null as Like);

            //Act
            var result = sut.LikeUnlike(GetUser(), postId);

            //Verify
            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.Create(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.AddLikeInDatabase(It.IsAny<Like>()), Times.Once);

            StringAssert.Contains(result, "You like this post.");
        }

        [TestMethod]

        public void LikeUnLike_ShouldReturnMessage_WhenLikedIsFalse()
        {
            //Arrange
            likeRepositoryMock
                .Setup(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()))
                .Returns(new Like { Liked = false });

            //Act
            var result = sut.LikeUnlike(GetUser(), postId);

            //Verify
            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.UpdateInDatabase(It.IsAny<Like>()), Times.Once);

            StringAssert.Contains(result, "You like this post.");
        }

        [TestMethod]

        public void LikeUnlike_ShouldReturnMessage_WhenUnLike()
        {
            //Arrange
            likeRepositoryMock
                .Setup(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()))
                .Returns(new Like { Liked = true });

            //Act
            var result = sut.LikeUnlike(GetUser(), postId);

            //Verify
            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.UpdateInDatabase(It.IsAny<Like>()), Times.Once);

            StringAssert.Contains(result, "You unlike this post.");
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
