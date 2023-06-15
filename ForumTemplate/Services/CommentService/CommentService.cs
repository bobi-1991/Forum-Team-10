using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Repositories;
using ForumTemplate.Repositories.CommentPersistence;
using ForumTemplate.Validation;
using System.Runtime.InteropServices;

namespace ForumTemplate.Services.CommentService
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

        public List<CommentResponse> GetAll()
        {
            var comments = commentRepository.GetAll();
            var result = new List<CommentResponse>();

            foreach (var comment in comments)
            {
                CommentResponse commentResult = new CommentResponse
                (
                    comment.Id,
                    comment.Content,
                    comment.UserId,
                    comment.PostId,
                    comment.CreatedAt,
                    comment.UpdatedAt
                );

                result.Add(commentResult);
            }

            return result;
        }

        public List<CommentResponse> GetComments(Guid postId)
        {

            var commentsById = commentRepository.GetByPostId(postId);
            var result = new List<CommentResponse>();

            foreach (var comment in commentsById)
            {
                var commentResult = new CommentResponse
                (
                   comment.Id,
                    comment.Content,
                    comment.UserId,
                    comment.PostId,
                    comment.CreatedAt,
                    comment.UpdatedAt
                );

                result.Add(commentResult);
            }

            return result;
        }

        public CommentResponse GetById(Guid id)
        {
            //Validation
            commentsValidator.Validate(id);

            var comment = commentRepository.GetById(id);

            var commentResult = new CommentResponse
            (
                    comment.Id,
                    comment.Content,
                    comment.UserId,
                    comment.PostId,
                    comment.CreatedAt,
                    comment.UpdatedAt
            );

            return commentResult;
        }

        public CommentResponse Create(CommentRequest commentRequest)
        {
            //Validation
            commentsValidator.Validate(commentRequest);

            var comment = Comment.Create
            (        
               commentRequest.Content,
               commentRequest.UserId,
               commentRequest.PostId
            );

           var createdComment = commentRepository.Create(comment);

           return new CommentResponse
                (
                createdComment.Id,
                createdComment.Content,
                createdComment.UserId,
                createdComment.PostId,
                createdComment.CreatedAt,
                createdComment.UpdatedAt
                );
        }

        public CommentResponse Update(Guid id, CommentRequest commentRequest)
        {
            //Validation
            commentsValidator.Validate(id, commentRequest);

            var comment = Comment.Create
            (
               commentRequest.Content,
               commentRequest.UserId,
               commentRequest.PostId
            );

            var updatedComment = commentRepository.Update(id, comment);

            return new CommentResponse
              (
              updatedComment.Id,
              updatedComment.Content,
              updatedComment.UserId,
              updatedComment.PostId,
              updatedComment.CreatedAt,
              updatedComment.UpdatedAt
              );
        }

        public string Delete(Guid id)
        {
            //Validation
            commentsValidator.Validate(id);

            return commentRepository.Delete(id);
        }

        public void DeleteByPostId(Guid postId)
        {
            var commentToDelete = commentRepository.GetById(postId);

            commentRepository.DeleteByPostId(postId);
        }
    }
}
