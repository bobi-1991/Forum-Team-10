 using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Mappers
{
    public interface ICommentMapper
    {
        CommentResponse MapToCommentResponse(Comment comment);
        List<CommentResponse> MapToCommentResponse(List<Comment> comments);
        Comment MapToComment(CommentRequest commentRequest);
        CommentRequest MapToCommentRequest(CommentResponse commentResponse);
    }
}
