using ForumTemplate.Authorization;
using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Mappers;
using ForumTemplate.Persistence.CommentRepository;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Persistence.UserRepository;
using ForumTemplate.Validation;


namespace ForumTemplate.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly ICommentsValidator commentsValidator;
        private readonly ICommentMapper commentMapper;
        private readonly IUserRepository userRepositoty;
        private readonly IPostRepository postRepository;
        private readonly IUserAuthenticationValidator userValidator;


        public CommentService(ICommentRepository commentRepository, ICommentsValidator commentsValidator, 
            ICommentMapper commentMapper, IUserRepository userRepositoty, IPostRepository postRepository,
            IUserAuthenticationValidator userValidator)
        {
            this.commentRepository = commentRepository;
            this.commentsValidator = commentsValidator;
            this.commentMapper = commentMapper;
            this.userRepositoty = userRepositoty;
            this.postRepository = postRepository;
            this.userValidator = userValidator;
        }

        public List<CommentResponse> GetAll()
        {
            var comments = commentRepository.GetAll();

            return this.commentMapper.MapToCommentResponse(comments);
        }

        public List<CommentResponse> GetComments(Guid postId)
        {
            userValidator.ValidateUserIsLogged();

            commentsValidator.GetCommentsByPostID(postId);

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
            userValidator.ValidateUserIsLoggedAndNotBannedCommentCreate();

            //Validation
            this.commentsValidator.Validate(commentRequest);

            _ = userRepositoty.GetById(commentRequest.UserId);

            var postOfComment = postRepository.GetById(commentRequest.PostId);

            if (postOfComment == null)
            {
                throw new EntityNotFoundException("Post with ID: {id} not found.");
            }
            
            var comment = this.commentMapper.MapToComment(commentRequest);
            var createdComment = commentRepository.Create(comment);

            return this.commentMapper.MapToCommentResponse(createdComment);
        }

        public CommentResponse Update(Guid id, CommentRequest commentRequest)
        {
            userValidator.ValidateUserIsLogged();

            //Validation
            commentsValidator.Validate(id, commentRequest);

            var commentToUpdate = commentRepository.GetById(id);
            var authorId = commentToUpdate.UserId;

            userValidator.ValidateUserIdMatchAuthorIdComment(authorId);

            _ = userRepositoty.GetById(commentRequest.UserId);
            var postOfComment = postRepository.GetById(commentRequest.PostId);

            if (postOfComment == null)
            {
                throw new EntityNotFoundException("Post with ID: {id} not found.");
            }

            var comment = this.commentMapper.MapToComment(commentRequest);
            var updatedComment = commentRepository.Update(id, comment);

            return this.commentMapper.MapToCommentResponse(updatedComment);
        }

        public string Delete(Guid id)
        {
            userValidator.ValidateUserIsLogged();

            var commentToDelete = commentRepository.GetById(id);
            var authorId = commentToDelete.UserId;

            userValidator.ValidateUserIdMatchAuthorIdComment(authorId);
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
