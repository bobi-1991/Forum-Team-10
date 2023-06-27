
using ForumTemplate.Mappers;
using ForumTemplate.Validation;
using ForumTemplate.Services.CommentService;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Common.FilterModels;
using ForumTemplate.Models;
using ForumTemplate.Persistence.UserRepository;

namespace ForumTemplate.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly ICommentService commentService;
        private readonly IUserRepository userRepository;
        private readonly PostsValidator postsValidator;
        private readonly PostMapper postMapper;

        public PostService(IPostRepository repository, ICommentService commentService, PostsValidator postsValidator, PostMapper postMapper, IUserRepository userRepository)
        {
            this.postRepository = repository;
            this.commentService = commentService;
            this.postsValidator = postsValidator;
            this.postMapper = postMapper;
            this.userRepository = userRepository;
        }

        public List<PostResponse> GetAll()
        {
            var posts = postRepository.GetAll();
            return postMapper.MapToPostResponse(posts);
        }


        public PostResponse GetById(Guid id)
        {
            //Validation
            postsValidator.Validate(id);
            var post = postRepository.GetById(id);
            return postMapper.MapToPostResponse(post);
        }

        public PostResponse Create(PostRequest postRequest)
        {
            //Validation
            postsValidator.Validate(postRequest);

            var author = userRepository.GetById(postRequest.UserId);
            var post = postMapper.MapToPost(postRequest);//author);
            var createdPost = postRepository.Create(post);

            return postMapper.MapToPostResponse(createdPost);
        }

        public PostResponse Update(Guid id, PostRequest postRequest)
        {
            //Validation
            postsValidator.Validate(id, postRequest);

           
         //   var author = userRepository.GetById(postRequest.UserId);
            var post = postMapper.MapToPost(postRequest);//,author);
            var updatedPost = postRepository.Update(id, post);

            return postMapper.MapToPostResponse(updatedPost);
        }

        public string Delete(Guid id)
        {
            //Validation
            postsValidator.Validate(id);

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
