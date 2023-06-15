using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Repositories;
using Microsoft.Extensions.Hosting;
using ForumTemplate.Validation;
using ForumTemplate.Repositories.PostPersistence;
using ForumTemplate.Services.CommentService;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Repositories.UserPersistence;

namespace ForumTemplate.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly ICommentService commentService;
        private readonly IPostsValidator postsValidator;
        private readonly PostMapper postMapper;

        public PostService(IPostRepository repository, ICommentService commentService, IPostsValidator postsValidator, PostMapper postMapper)
        {
            this.postRepository = repository;
            this.commentService = commentService;
            this.postsValidator = postsValidator;
            this.postMapper = postMapper;
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

            var post = postMapper.MapToPost(postRequest);
            var createdPost = postRepository.Create(post);

            return postMapper.MapToPostResponse(createdPost);
        }

        public PostResponse Update(Guid id, PostRequest postRequest)
        {
            //Validation
            postsValidator.Validate(id, postRequest);

            var post = postMapper.MapToPost(postRequest);
            var updatedPost = postRepository.Update(id, post);

            return postMapper.MapToPostResponse(updatedPost);
        }

        public string Delete(Guid id)
        {
            //Validation
            postsValidator.Validate(id);

            var postToDelete = postRepository.GetById(id);
            this.commentService.DeleteByPostId(postToDelete.Id);

            return postRepository.Delete(id);
        }

    }
}
