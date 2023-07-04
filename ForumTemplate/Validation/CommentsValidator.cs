using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Persistence.CommentRepository;
using ForumTemplate.Persistence.PostRepository;

namespace ForumTemplate.Validation
{
    public class CommentsValidator : ICommentsValidator
    {
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        public CommentsValidator(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
        }
        public void Validate(CommentRequest commentRequest)
        {
            var errors = new List<string>();

            // DEPRECATED - replaced by Fluid Validation
            if (commentRequest is null)
            {
                errors.Add("Comment input cannot be null");
            }
            // DEPRECATED - replaced by Fluid Validation
            if (string.IsNullOrWhiteSpace(commentRequest.Content))
            {
                errors.Add("Comment content cannot be null or whitespace");
            }

            if (!this.postRepository.Exist(commentRequest.PostId))
            {
                errors.Add($"Post with Id {commentRequest.PostId} does not exist");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }

        public void Validate(Guid id)
        {
            var comment = this.commentRepository.GetById(id);

            if (comment == null)
            {
                throw new ValidationException($"Comment with ID: {id} not found.");
            }
        }

        public void Validate(Guid id, CommentRequest commentRequest)
        {
            var errors = new List<string>();

            // DEPRECATED - replaced by Fluid Validation
            if (commentRequest is null)
            {
                errors.Add("Comment input cannot be null");
            }

            if (string.IsNullOrWhiteSpace(commentRequest.Content))
            {
                errors.Add("Comment content cannot be null or whitespace");
            }

            if (!this.postRepository.Exist(commentRequest.PostId))
            {
                errors.Add($"Post with Id {commentRequest.PostId} does not exist");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }

        public void GetCommentsByPostID(Guid postId)
        {
            var commentsById = this.commentRepository.GetByPostId(postId);

            if (commentsById == null)
            {
                throw new EntityNotFoundException($"Comment with ID: {postId} not found.");
            }
        }
    }
}
