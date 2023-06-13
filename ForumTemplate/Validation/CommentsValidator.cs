using ForumTemplate.Exceptions;
using ForumTemplate.Models.Input;
using ForumTemplate.Repositories;


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



        public void Validate(CommentInputModel commentInput)
        {
            var errors = new List<string>();

            if (commentInput is null)
            {
                errors.Add("Comment input cannot be null");
            }

            if (string.IsNullOrWhiteSpace(commentInput.Content))
            {
                errors.Add("Comment content cannot be null or whitespace");
            }

            if (commentInput.PostId <= 0)
            {
                errors.Add("PostId must be a positive number");
            }

            if (!this.postRepository.Exist(commentInput.PostId))
            {
                errors.Add($"Post with Id {commentInput.PostId} does not exist");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }

        public void Validate(int id)
        {
            var comment = this.commentRepository.GetById(id);

            if (id <= 0)
            {
                throw new ValidationException($"Comment ID cannot be 0 or negative number");
            }

            if (comment == null)
            {
                throw new ValidationException($"Comment with ID: {id} not found.");
            }
        }

        public void Validate(int id,  CommentInputModel commentInput) 
        {
            var errors = new List<string>();

            if (id <= 0)
            {
                errors.Add("Comment Id cannot be a negative number or 0");
            }
            if (id > 0)
            {
                var commentToUpdate = this.commentRepository.GetById(id);
                if (commentToUpdate == null)
                {
                    errors.Add($"Comment with ID: {id} not found.");
                }
            }
            if (commentInput is null)
            {
                errors.Add("Comment input cannot be null");
            }

            if (string.IsNullOrWhiteSpace(commentInput.Content))
            {
                errors.Add("Comment content cannot be null or whitespace");
            }

            if (commentInput.PostId <= 0)
            {
                errors.Add("PostId must be a positive number");
            }

            if (!this.postRepository.Exist(commentInput.PostId))
            {
                errors.Add($"Post with Id {commentInput.PostId} does not exist");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }



    }
}
