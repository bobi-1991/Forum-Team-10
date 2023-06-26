using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Mappers
{
    public class CommentMapper
    {
        public CommentResponse MapToCommentResponse(Comment comment)
        {
               return new CommentResponse(
                    comment.CommentId,
                    comment.Content,
                    comment.User.Username,
                    comment.Post.Title,
                    comment.CreatedAt,
                    comment.UpdatedAt);
        }
        public List<CommentResponse> MapToCommentResponse(List<Comment> comments)
        {
            var commentResponses = new List<CommentResponse>();

            foreach (var comment in comments)
            {
                CommentResponse commentResponse = new CommentResponse(
                    comment.CommentId,
                    comment.Content,
                    comment.User.Username,
                    comment.Post.Title,
                    comment.CreatedAt,
                    comment.UpdatedAt);

                commentResponses.Add(commentResponse);
            }

            return commentResponses;
        }

        public Comment MapToComment(CommentRequest commentRequest)       
        {
           var comment = Comment.Create(
                 commentRequest.Content,
                 commentRequest.UserId,
                 commentRequest.PostId);

            return comment; 
        }
    }
}
