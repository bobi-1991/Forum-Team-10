
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

        public PostService(IPostRepository repository, ICommentService commentService, PostsValidator postsValidator, PostMapper postMapper)
        {
            this.postRepository = repository;
            this.commentService = commentService;
            this.postsValidator = postsValidator;
            this.postMapper = postMapper;
           // this.userRepository = userRepository;
        }

        public List<PostResponse> GetAll()
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }

            var posts = postRepository.GetAll();
            return postMapper.MapToPostResponse(posts);
        }


        public PostResponse GetById(Guid id)
        {
            //Validation
            postsValidator.Validate(id);

            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }

            var post = postRepository.GetById(id);

            if (post == null)
            {
                throw new EntityNotFoundException($"Post with ID: {id} not found.");
            }

          
            return postMapper.MapToPostResponse(post);
        }

        public PostResponse Create(PostRequest postRequest)
        {
            //Validation
            postsValidator.Validate(postRequest);

            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.UserId.Equals(postRequest.UserId))
            {
                throw new ValidationException("The id you entered does not match your id");
            }
            if (CurrentLoggedUser.LoggedUser.IsBlocked)
            {
                throw new EntityBannedException("I'm sorry, but you cannot create a new post. You are permanently banned.");
            }

            var post = postMapper.MapToPost(postRequest);
            var createdPost = postRepository.Create(post);

            return postMapper.MapToPostResponse(createdPost);
        }

        public PostResponse Update(Guid id, PostRequest postRequest)
        {
            //Validation
            postsValidator.Validate(id, postRequest);


            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.Posts.Any(x => x.PostId.Equals(id)) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("The id you entered does not match yours post(s) id");
            }

            var post = postMapper.MapToPost(postRequest);
            var updatedPost = postRepository.Update(id, post);

            return postMapper.MapToPostResponse(updatedPost);
        }

        public string Delete(Guid id)
        {
            //Validation
            postsValidator.Validate(id);

            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.Posts.Any(x=>x.PostId.Equals(id)) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("The id you entered does not match yours post(s) id");
            }

            var postToDelete = postRepository.GetById(id);
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
