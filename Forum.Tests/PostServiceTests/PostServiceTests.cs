using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Models.Pagination;
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
			userValidatorMock.Verify(x => x.ValidatePostCreateIDMatchAndNotBlocked(It.IsAny<User>(), It.IsAny<PostRequest>()), Times.Once);

			postMapperMock.Verify(x => x.MapToPost(It.IsAny<PostRequest>()), Times.Once);

			postRepositoryMock.Verify(x => x.Create(It.IsAny<Post>()), Times.Once);

			postMapperMock.Verify(x => x.MapToPostResponse(It.IsAny<Post>()));
		}

		[TestMethod]

		public void Update_ShouldInvokeCorrectMethods()
		{
			//Act
			var result = sut.Update(GetUser(), id, GetPostRequest());

			//Verify
			postRepositoryMock.Verify(x => x.GetById(id), Times.Once);

			userValidatorMock.Verify(x => x.ValidateUserIdMatchAuthorIdPost(It.IsAny<User>(), It.IsAny<Guid>()), Times.Once);

			postMapperMock.Verify(x => x.MapToPost(It.IsAny<PostRequest>()), Times.Once);

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

		[TestMethod]

		public void GetAllPosts_ShouldInvoke()
		{
			//Act
			var result = sut.GetAllPosts();

			//Assert
			postRepositoryMock.Verify(x => x.GetAll(), Times.Once);
		}

		[TestMethod]

		public void GetByPostId_ShouldInvoke()
		{
			//Act
			var result = sut.GetByPostId(It.IsAny<Guid>());

			//Assert
			postValidatorMock.Verify(x => x.Validate(It.IsAny<Guid>()), Times.Once);

			postRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
		}

		[TestMethod]

		public void GetTopCommentedPosts_ShouldInvoke()
		{
			//Arrange
			postRepositoryMock
				.Setup(x => x.GetTopCommentedPosts(It.IsAny<int>()))
				.Returns(new List<Post>());

			//Act
			var result = sut.GetTopCommentedPosts(It.IsAny<int>());

			//Assert
			postRepositoryMock.Verify(x => x.GetTopCommentedPosts(It.IsAny<int>()), Times.Once);
		}

        [TestMethod]

        public void GetRecentlyCreatedPosts_ShouldInvoke()
        {
            //Arrange
            postRepositoryMock
                .Setup(x => x.GetRecentlyCreatedPosts(It.IsAny<int>()))
                .Returns(new List<Post>());

            //Act
            var result = sut.GetRecentlyCreatedPosts(It.IsAny<int>());

            //Assert
            postRepositoryMock.Verify(x => x.GetRecentlyCreatedPosts(It.IsAny<int>()), Times.Once);
        }

		[TestMethod]

		public void FilterBy_ShouldInvoke()
		{
			//Arrange
			postRepositoryMock
				.Setup(x => x.FilterBy(It.IsAny<PostQueryParameters>()))
				.Returns(new List<Post>());

			postMapperMock
				.Setup(x => x.MapToPostResponse(It.IsAny<List<Post>>()))
				.Returns(new List<PostResponse>());

			//Act
			var result = sut.FilterBy(It.IsAny<PostQueryParameters>());

			//Assert
			postRepositoryMock.Verify(x => x.FilterBy(It.IsAny<PostQueryParameters>()), Times.Once);

			postMapperMock.Verify(x => x.MapToPostResponse(It.IsAny<List<Post>>()), Times.Once);
		}

		[TestMethod]

		public void SearchBy_ShouldInvoke()
		{
			//Arrange
			postRepositoryMock
				.Setup(x => x.SearchBy(It.IsAny<PostQueryParameters>()))
				.Returns(new PaginatedList<Post>(new List<Post>(), It.IsAny<int>(), It.IsAny<int>()));

			//Act
			var result = sut.SearchBy(It.IsAny<PostQueryParameters>());

			//Assert
			postRepositoryMock.Verify(x => x.SearchBy(It.IsAny<PostQueryParameters>()), Times.Once);
		}

        private PostRequest GetPostRequest()
		{
			return new PostRequest
			{
				Title = "Title",
				Content = "content",
				UserId = id
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

		private void SetupUserValidatorMock()
		{
			userValidatorMock
				.Setup(x => x.ValidatePostCreateIDMatchAndNotBlocked(It.IsAny<User>(), GetPostRequest()));

			userValidatorMock
				.Setup(x => x.ValidateUserIdMatchAuthorIdPost(It.IsAny<User>(), It.IsAny<Guid>()));
		}
	}
}
