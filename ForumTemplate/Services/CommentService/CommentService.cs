using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Mappers;
using ForumTemplate.Persistence.CommentRepository;
using ForumTemplate.Validation;


namespace ForumTemplate.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly CommentsValidator commentsValidator;
        private readonly CommentMapper commentMapper;

        public CommentService(ICommentRepository commentRepository, CommentsValidator commentsValidator, CommentMapper commentMapper)
        {
            this.commentRepository = commentRepository;
            this.commentsValidator = commentsValidator;
            this.commentMapper = commentMapper;
        }

        public List<CommentResponse> GetAll()
        {
            var comments = commentRepository.GetAll();

            return this.commentMapper.MapToCommentResponse(comments);
        }

        public List<CommentResponse> GetComments(Guid postId)
        {

            var commentsById = this.commentRepository.GetByPostId(postId);
            return this.commentMapper.MapToCommentResponse(commentsById);
        }

        public CommentResponse GetById(Guid id)
        {
            //Validation
            this.commentsValidator.Validate(id);

            var comment = commentRepository.GetById(id);

            return this.commentMapper.MapToCommentResponse(comment);
        }

        public CommentResponse Create(CommentRequest commentRequest)
        {
            //Validation
            this.commentsValidator.Validate(commentRequest);

            var comment = this.commentMapper.MapToComment(commentRequest);
            var createdComment = commentRepository.Create(comment);

            return this.commentMapper.MapToCommentResponse(createdComment);
        }

        public CommentResponse Update(Guid id, CommentRequest commentRequest)
        {
            //Validation
            commentsValidator.Validate(id, commentRequest);
            var comment = this.commentMapper.MapToComment(commentRequest);

            var updatedComment = commentRepository.Update(id, comment);

            return this.commentMapper.MapToCommentResponse(updatedComment);
        }

        public string Delete(Guid id)
        {
            //Validation
            commentsValidator.Validate(id);

            return this.commentRepository.Delete(id);
        }

        public void DeleteByPostId(Guid postId)
        {
            var commentToDelete = commentRepository.GetById(postId);

            this.commentRepository.DeleteByPostId(postId);
        }
    }
}
