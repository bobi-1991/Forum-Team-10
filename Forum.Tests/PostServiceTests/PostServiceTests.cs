using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Services.CommentService;
using ForumTemplate.Services.LikeService;
using ForumTemplate.Services.PostService;
using ForumTemplate.Validation;
using Moq;

namespace ForumTemplate.Tests.PostServiceTests
{
    [TestClass]
    public class PostServiceTests
    {
        private PostService sut;

        private Mock<IPostRepository> postRepositoryMock;
        private Mock<ICommentService> commentServiceMock;
        private Mock<IUserAuthenticationValidator> userValidatorMock;
        private Mock<IPostsValidator> postValidatorMock;
        private Mock<IPostMapper> postMapperMock;
        private Mock<ILikeService> likeServiceMock;

        private Guid id;
        private Guid postId;

        [TestInitialize]

        public void Initialize()
        {
            postRepositoryMock = new Mock<IPostRepository>();
            commentServiceMock = new Mock<ICommentService>();
            userValidatorMock = new Mock<IUserAuthenticationValidator>();
            postValidatorMock = new Mock<IPostsValidator>();
            postMapperMock = new Mock<IPostMapper>();
            likeServiceMock = new Mock<ILikeService>();

            id = Guid.NewGuid();
            postId = Guid.NewGuid();

            SetupUserValidatorMock();


            postValidatorMock
                .Setup(x => x.Validate(id));

            postValidatorMock
                .Setup(x => x.Validate(GetPostRequest()));

            postValidatorMock
                .Setup(x => x.Validate(id, GetPostRequest()));

            postRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(new List<Post>());

            postRepositoryMock
                .Setup(x => x.GetById(id))
                .Returns(new Post { UserId = id, PostId = postId });

            postRepositoryMock
                .Setup(x => x.Create(It.IsAny<Post>()))
                .Returns(new Post());

            postRepositoryMock
                .Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Post>()))
                .Returns(new Post());

            postRepositoryMock
                .Setup(x => x.GetByUserId(It.IsAny<Guid>()))
                .Returns(new List<Post>());

            postRepositoryMock
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns("Post was successfully deleted.");

            postMapperMock
                .Setup(x => x.MapToPostResponse(It.IsAny<List<Post>>()))
                .Returns(new List<PostResponse>());

            postMapperMock
                .Setup(x => x.MapToPostResponse(It.IsAny<Post>()))
                .Returns(It.IsAny<PostResponse>());

            postMapperMock
                .Setup(x => x.MapToPost(GetPostRequest()))
                .Returns(new Post());

            sut = new PostService(postRepositoryMock.Object, commentServiceMock.Object,
               postValidatorMock.Object, postMapperMock.Object, userValidatorMock.Object, likeServiceMock.Object);
        }
        
        [TestMethod]

        public void DeleteByUserId_ShouldInvokeCorrectMethods()
        {
            //Arrange
            postRepositoryMock
                .Setup(x => x.DeletePosts(It.IsAny<List<Post>>()));

            //Act
            sut.DeleteByUserId(It.IsAny<Guid>());

            //Verify
            postRepositoryMock.Verify(x => x.GetByUserId(It.IsAny<Guid>()), Times.Once);

            postRepositoryMock.Verify(x => x.DeletePosts(It.IsAny<List<Post>>()), Times.Once);
        }

        [TestMethod]
        public void GetAll_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetAll();

            //Verify
            postRepositoryMock.Verify(x => x.GetAll(), Times.Once);

            postMapperMock.Verify(x => x.MapToPostResponse(It.IsAny<List<Post>>()));

        }

        [TestMethod]
        public void GetById_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.GetById(id);

            //Verify
            postValidatorMock.Verify(x => x.Validate(id), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(id), Times.Once);

            postMapperMock.Verify(x => x.MapToPostResponse(It.IsAny<Post>()));
        }

        [TestMethod]

        public void Create_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Create(GetUser(), GetPostRequest());

            //Verify
            postValidatorMock.Verify(x => x.Validate(GetPostRequest()), Times.Once);

            userValidatorMock.Verify(x => x.ValidatePostCreateIDMatchAndNotBlocked(It.IsAny<User>(), GetPostRequest()), Times.Once);

            postMapperMock.Verify(x => x.MapToPost(GetPostRequest()), Times.Once);

            postRepositoryMock.Verify(x => x.Create(It.IsAny<Post>()), Times.Once);

            postMapperMock.Verify(x => x.MapToPostResponse(It.IsAny<Post>()));
        }

        [TestMethod]

        public void Update_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Update(GetUser(), id, GetPostRequest());

            //Verify
            postValidatorMock.Verify(x => x.Validate(id, GetPostRequest()), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(id), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserIdMatchAuthorIdPost(It.IsAny<User>(), It.IsAny<Guid>()), Times.Once);

            postMapperMock.Verify(x => x.MapToPost(GetPostRequest()), Times.Once);

            postRepositoryMock.Verify(x => x.Update(id, It.IsAny<Post>()), Times.Once);

            postMapperMock.Verify(x => x.MapToPostResponse(It.IsAny<Post>()), Times.Once);
        }

        [TestMethod]

        public void Delete_ShouldInvokeCorrectMethods()
        {
            //Act
            var result = sut.Delete(GetUser(), id);

            //Verify
            postValidatorMock.Verify(x => x.Validate(id), Times.Once);

            postRepositoryMock.Verify(x => x.GetById(id), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserIdMatchAuthorIdPost(It.IsAny<User>(), It.IsAny<Guid>()), Times.Once);

            commentServiceMock.Verify(x => x.DeleteByPostId(postId), Times.Once);

            postRepositoryMock.Verify(x => x.Delete(id), Times.Once);
        }

        [TestMethod]

        public void Delete_ShouldReturn()
        {
            //act
            var message = sut.Delete(GetUser(), id);

            //Assert
            StringAssert.Contains(message, "Post was successfully deleted.");
        }

        private PostRequest GetPostRequest()
        {
            return new PostRequest
            (
                "Title",
                "content",
                id
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

        private void SetupUserValidatorMock()
        {
            userValidatorMock
                .Setup(x => x.ValidatePostCreateIDMatchAndNotBlocked(It.IsAny<User>(), GetPostRequest()));

            userValidatorMock
                .Setup(x => x.ValidateUserIdMatchAuthorIdPost(It.IsAny<User>(), It.IsAny<Guid>()));
        }
    }
}
