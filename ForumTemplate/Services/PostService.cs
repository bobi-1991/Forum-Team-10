using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Models.Input;
using ForumTemplate.Models.Result;
using ForumTemplate.Repositories;
using ForumTemplate.Repositories.DTO_s;
using Microsoft.Extensions.Hosting;
using ForumTemplate.Validation;

namespace ForumTemplate.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository repository;
        private readonly ICommentService commentService;
        private readonly IPostsValidator postsValidator;

        public PostService(IPostRepository repository, ICommentService commentService, IPostsValidator postsValidator)
        {
            this.repository = repository;
            this.commentService = commentService;
            this.postsValidator = postsValidator;
        }

        public List<PostResultModel> GetAll()
        {
            var posts = this.repository.GetAll();

            var result = new List<PostResultModel>();
            foreach (var post in posts)
            {
                var commentsResult = this.commentService.GetComments(post.Id);
                var postResult = post.MapToPostResultModel(commentsResult);
                result.Add(postResult);
            }

            return result;

            //var result = new List<PostResultModel>();
            //foreach (var post in posts)
            //{
            //    var postresult = new PostResultModel()
            //    {
            //        Description = post.Description,
            //        Title = post.Title,
            //    };
            //    result.Add(postresult);
            //}

            //return result;
        }


        public PostResultModel GetById(int id)
        {
            //Validation
            postsValidator.Validate(id);

            var post = this.repository.GetById(id);

            var commentsResult = this.commentService.GetComments(post.Id);
            return post.MapToPostResultModel(commentsResult);

        }

        public PostResultModel Create(PostInputModel post)
        {
            //Validation
            postsValidator.Validate(post);

            var postDTO = post.MapToPostDTO(2);

            var createdPost = this.repository.Create(postDTO);

            return createdPost.MapToPostResultModel();
        }

        public PostResultModel Update(int id, PostInputModel post)
        {
            //Validation
            postsValidator.Validate(id, post);

            var postDTO = post.MapToPostDTO();

            var updatedPost = this.repository.Update(id, postDTO);

            return updatedPost.MapToPostResultModel();
        }

        public string Delete(int id)
        {
            //Validation
            postsValidator.Validate(id);

            var postToDelete = this.repository.GetById(id);

            commentService.DeleteByPostId(postToDelete.Id);

            return this.repository.Delete(id);
        }

    }
}
