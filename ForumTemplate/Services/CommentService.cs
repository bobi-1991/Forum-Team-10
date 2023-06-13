using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Models.Input;
using ForumTemplate.Models.Result;
using ForumTemplate.Repositories;
using ForumTemplate.Repositories.DTO_s;
using ForumTemplate.Validation;

namespace ForumTemplate.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly ICommentsValidator commentsValidator;

        public CommentService(ICommentRepository commentRepository, ICommentsValidator commentsValidator)
        {
            this.commentRepository = commentRepository;
            this.commentsValidator = commentsValidator;
        }

        public List<CommentResultModel> GetAll()
        {
            var comments = this.commentRepository.GetAll();

            var result = new List<CommentResultModel>();
            foreach (var comment in comments)
            {
                var commentResult = new CommentResultModel()
                {
                    Content = comment.Content,
                };
                result.Add(commentResult);
            }

            return result;
        }

        public List<CommentResultModel> GetComments(int postId)
        {

            var comments = this.commentRepository.GetByPostId(postId);

            var result = new List<CommentResultModel>();
            foreach (var comment in comments)
            {
                var commentResult = new CommentResultModel()
                {
                    Content = comment.Content
                };
                result.Add(commentResult);
            }

            return result;
        }

        public CommentResultModel GetById(int id)
        {
            //Validation
            commentsValidator.Validate(id);

            var comment = this.commentRepository.GetById(id);

            var commentResult = new CommentResultModel()
            {
                Content = comment.Content,
            };

            return commentResult;
        }

        public CommentResultModel Create(CommentInputModel comment)
        {
            //Validation
            commentsValidator.Validate(comment);
            
            var commentDTO = new CommentDTO()
            {
                Content = comment.Content,
                PostId = comment.PostId,
                UserId = 2
            };

            var createdComment = this.commentRepository.Create(commentDTO);

            return new CommentResultModel
            {
                Content = createdComment.Content
            };
        }

        public CommentResultModel Update(int id, CommentInputModel comment)
        {
            //Validation
            commentsValidator.Validate(id, comment);

            var commentDTO = new CommentDTO
            {
                Content = comment.Content
            };

            var updatedComment = this.commentRepository.Update(id, commentDTO);

            return new CommentResultModel()
            {
                Content = updatedComment.Content
            };
        }

        public string Delete(int id)
        {
            //Validation
            commentsValidator.Validate(id);

            return this.commentRepository.Delete(id);
        }

        public void DeleteByPostId(int postId)
        {
            var commentToDelete = this.commentRepository.GetById(postId);

            this.commentRepository.DeleteByPostId(postId);
        }
    }
}
