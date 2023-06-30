﻿using ForumTemplate.Authorization;
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
        private readonly CommentsValidator commentsValidator;
        private readonly CommentMapper commentMapper;
        private readonly IUserRepository userRepositoty;
        private readonly IPostRepository postRepository;


        public CommentService(ICommentRepository commentRepository, CommentsValidator commentsValidator, CommentMapper commentMapper, IUserRepository userRepositoty, IPostRepository postRepository)
        {
            this.commentRepository = commentRepository;
            this.commentsValidator = commentsValidator;
            this.commentMapper = commentMapper;
            this.userRepositoty = userRepositoty;
            this.postRepository = postRepository;
        }

        public List<CommentResponse> GetAll()
        {
            var comments = commentRepository.GetAll();

            return this.commentMapper.MapToCommentResponse(comments);
        }

        public List<CommentResponse> GetComments(Guid postId)
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }

            var commentsById = this.commentRepository.GetByPostId(postId);

            if (commentsById == null)
            {
                throw new EntityNotFoundException($"Comment with ID: {postId} not found.");
            }

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
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (CurrentLoggedUser.LoggedUser.IsBlocked)
            {
                throw new EntityBannedException("I'm sorry, but you cannot write a comment. You are permanently banned.");
            }

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
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }

            //Validation
            commentsValidator.Validate(id, commentRequest);

            if (!CurrentLoggedUser.LoggedUser.Comments.Any(x => x.CommentId.Equals(id)) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("The id you entered does not match yours comment(s) id");
            }

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
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }
            if (!CurrentLoggedUser.LoggedUser.Comments.Any(x => x.CommentId.Equals(id)) && !CurrentLoggedUser.LoggedUser.IsAdmin)
            {
                throw new ValidationException("The id you entered does not match yours comment(s) id");
            }
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
