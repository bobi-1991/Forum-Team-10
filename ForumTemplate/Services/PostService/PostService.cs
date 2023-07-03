
using ForumTemplate.Mappers;
using ForumTemplate.Validation;
using ForumTemplate.Services.CommentService;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Common.FilterModels;
using ForumTemplate.Models;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Authorization;
using ForumTemplate.Exceptions;

namespace ForumTemplate.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly ICommentService commentService;
      //  private readonly IUserRepository userRepository;
        private readonly PostsValidator postsValidator;
        private readonly PostMapper postMapper;
        private readonly IUserAuthenticationValidator userValidator;

        public PostService(IPostRepository repository, ICommentService commentService, PostsValidator postsValidator, PostMapper postMapper, IUserAuthenticationValidator userValidator)
        {
            this.postRepository = repository;
            this.commentService = commentService;
            this.postsValidator = postsValidator;
            this.postMapper = postMapper;
            this.userValidator = userValidator;
           // this.userRepository = userRepository;
        }

        public List<PostResponse> GetAll()
        {
            userValidator.ValidateUserIsLogged();

            var posts = postRepository.GetAll();
            return postMapper.MapToPostResponse(posts);
        }


        public PostResponse GetById(Guid id)
        {
            //Validation
            postsValidator.Validate(id);

            userValidator.ValidateUserIsLogged();

            var post = postRepository.GetById(id);
          
            return postMapper.MapToPostResponse(post);
        }

        public PostResponse Create(PostRequest postRequest)
        {
            //Validation
            postsValidator.Validate(postRequest);

            userValidator.ValidateUserIsLogged();

            userValidator.ValidatePostCreateIDMatchAndNotBlocked(postRequest);

            var post = postMapper.MapToPost(postRequest);
            var createdPost = postRepository.Create(post);

            return postMapper.MapToPostResponse(createdPost);
        }

        public PostResponse Update(Guid id, PostRequest postRequest)
        {
            //Validation
            postsValidator.Validate(id, postRequest);

            userValidator.ValidateUserIsLogged();

            var postToUpdate = postRepository.GetById(id);

            var authorId = postToUpdate.UserId;

            userValidator.ValidateUserIdMatchAuthorIdPost(authorId);

            var currentPost = postMapper.MapToPost(postRequest);
            var updatedPost = postRepository.Update(id, currentPost);

            return postMapper.MapToPostResponse(updatedPost);
        }

        public string Delete(Guid id)
        {
            //Validation
            postsValidator.Validate(id);

            userValidator.ValidateUserIsLogged();

            var postToDelete = postRepository.GetById(id);
            var authorId = postToDelete.UserId;

            userValidator.ValidateUserIdMatchAuthorIdPost(authorId);

            this.commentService.DeleteByPostId(postToDelete.PostId);

            return postRepository.Delete(id);
        }



        //   Not tested yet
        public List<PostResponse> FilterBy(PostQueryParameters filterParameters)
        {
            List<Post> filteredPosts = this.postRepository.FilterBy(filterParameters);

            return postMapper.MapToPostResponse(filteredPosts);
        }
    }
}
