using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Models;
using ForumTemplate.Models.Input;
using ForumTemplate.Models.Result;
using ForumTemplate.Repositories;
using ForumTemplate.Repositories.DTO_s;
using Microsoft.Extensions.Hosting;

namespace ForumTemplate.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository repository;
        private readonly ICommentService commentService;

        public PostService(IPostRepository repository, ICommentService commentService)
        {
            this.repository = repository;
            this.commentService = commentService;
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
            var post = this.repository.GetById(id);

            var commentsResult = this.commentService.GetComments(post.Id);
            return post.MapToPostResultModel(commentsResult);
            
        }

        public PostResultModel Create(PostInputModel post)
        {
            //validation
            var postDTO = post.MapToPostDTO(2);

            var createdPost = this.repository.Create(postDTO);

            return createdPost.MapToPostResultModel();
        }

        public PostResultModel Update(int id, PostInputModel post)
        {
            var postToUpdate = this.repository.GetById(id);

            if (postToUpdate == null)
            {
                throw new EntityNotFoundException($"Post with ID: {id} not found.");
            }

            var postDTO = post.MapToPostDTO();

            var updatedPost = this.repository.Update(id, postDTO);

            return updatedPost.MapToPostResultModel();
        }

        public string Delete(int id)
        {
            var postToDelete = this.repository.GetById(id);

            if (postToDelete == null)
            {
                throw new EntityNotFoundException($"Post with ID: {id} not found.");
            }

            commentService.DeleteByPostId(postToDelete.Id);

            return this.repository.Delete(id);
        }

    }
}
