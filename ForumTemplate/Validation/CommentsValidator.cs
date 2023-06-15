﻿using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Exceptions;
using ForumTemplate.Repositories.CommentPersistence;
using ForumTemplate.Repositories.PostPersistence;

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

            if (commentRequest is null)
            {
                errors.Add("Comment input cannot be null");
            }

            if (string.IsNullOrWhiteSpace(commentRequest.Content))
            {
                errors.Add("Comment content cannot be null or whitespace");
            }

            //if (commentRequest.PostId <= 0)
            //{
            //    errors.Add("PostId must be a positive number");
            //}

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

            //if (id <= 0)
            //{
            //    throw new ValidationException($"Comment ID cannot be 0 or negative number");
            //}

            if (comment == null)
            {
                throw new ValidationException($"Comment with ID: {id} not found.");
            }
        }

        public void Validate(Guid id, CommentRequest commentRequest) 
        {
            var errors = new List<string>();

            //if (id <= 0)
            //{
            //    errors.Add("Comment Id cannot be a negative number or 0");
            //}
            //if (id > 0)
            //{
            //    var commentToUpdate = this.commentRepository.GetById(id);
            //    if (commentToUpdate == null)
            //    {
            //        errors.Add($"Comment with ID: {id} not found.");
            //    }
            //}
            if (commentRequest is null)
            {
                errors.Add("Comment input cannot be null");
            }

            if (string.IsNullOrWhiteSpace(commentRequest.Content))
            {
                errors.Add("Comment content cannot be null or whitespace");
            }

            //if (commentRequest.PostId <= 0)
            //{
            //    errors.Add("PostId must be a positive number");
            //}

            if (!this.postRepository.Exist(commentRequest.PostId))
            {
                errors.Add($"Post with Id {commentRequest.PostId} does not exist");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException($"Following Validation Errors Occured: {string.Join(", ", errors)}");

            }
        }



    }
}
