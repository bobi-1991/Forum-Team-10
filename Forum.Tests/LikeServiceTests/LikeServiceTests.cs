using ForumTemplate.Models;
using ForumTemplate.Persistence.LikeRepository;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Services.LikeService;
using ForumTemplate.Services.LikeServiceHelper;
using ForumTemplate.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;

namespace ForumTemplate.Tests.LikeServiceTests
{
    [TestClass]
    public class LikeServiceTests
    {
        private LikeService sut;

        private Mock<IPostRepository> postRepositoryMock;
        private Mock<ILikeRepository> likeRepositoryMock;
        private Mock<IUserAuthenticationValidator> userValidatorMock;
        private Mock<IPostsValidator> postsValidatorMock;
        private Mock<IHelperWrapper> helperWrapperMock;

        private Guid userId;
        private Guid postId;

        [TestInitialize]
        public void Initialize()
        {
            postRepositoryMock = new Mock<IPostRepository>();
            likeRepositoryMock = new Mock<ILikeRepository>();
            userValidatorMock = new Mock<IUserAuthenticationValidator>();
            postsValidatorMock = new Mock<IPostsValidator>();
            helperWrapperMock = new Mock<IHelperWrapper>();

            userId = Guid.NewGuid();
            postId = Guid.NewGuid();

            //userValidatorMock
            //    .Setup(x => x.ValidateUserIsLogged());

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

            //helperWrapperMock
            //    .Setup(x => x.GetCurrentUserId())
            //    .Returns(userId);

            sut = new LikeService(postRepositoryMock.Object, likeRepositoryMock.Object, userValidatorMock.Object,
                postsValidatorMock.Object, helperWrapperMock.Object);
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
            //var result = sut.LikeUnlike(postId);

            ////Verify
            //userValidatorMock.Verify(x => x.ValidateUserIsLogged(), Times.Once);

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
            //var result = sut.LikeUnlike(postId);

            ////Verify
            //userValidatorMock.Verify(x => x.ValidateUserIsLogged(), Times.Once);

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

            ////Act
            //var result = sut.LikeUnlike(postId);

            ////Verify
            //userValidatorMock.Verify(x => x.ValidateUserIsLogged(), Times.Once);

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

            ////Act
            //var result = sut.LikeUnlike(postId);

            ////Verify
            //userValidatorMock.Verify(x => x.ValidateUserIsLogged(), Times.Once);

            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.Create(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.AddLikeInDatabase(It.IsAny<Like>()), Times.Once);

         //   StringAssert.Contains(result, "You like this post.");
        }

        [TestMethod]

        public void LikeUnLike_ShouldReturnMessage_WhenLikedIsFalse()
        {
            //Arrange
            likeRepositoryMock
                .Setup(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()))
                .Returns(new Like { Liked = false });

            ////Act
            //var result = sut.LikeUnlike(postId);

            ////Verify
            //userValidatorMock.Verify(x => x.ValidateUserIsLogged(), Times.Once);

            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.UpdateInDatabase(It.IsAny<Like>()), Times.Once);

          //  StringAssert.Contains(result, "You like this post.");
        }

        [TestMethod]

        public void LikeUnlike_ShouldReturnMessage_WhenUnLike()
        {
            //Arrange
            likeRepositoryMock
                .Setup(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()))
                .Returns(new Like { Liked = true });

            ////Act
            //var result = sut.LikeUnlike(postId);

            ////Verify
            //userValidatorMock.Verify(x => x.ValidateUserIsLogged(), Times.Once);

            postsValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.GetLikeByPostAndUserId(It.IsAny<Post>(), It.IsAny<Guid>()), Times.Once);

            likeRepositoryMock.Verify(x => x.UpdateInDatabase(It.IsAny<Like>()), Times.Once);

           // StringAssert.Contains(result, "You unlike this post.");
        }
    }
}
