using ForumTemplate.DTOs.CommentDTOs;

namespace ForumTemplate.DTOs.PostDTOs
{
    public record PostResponse(
        Guid Id,
        string Title,
        string Content,
        string Username,
        List<CommentResponse> Comments,
        DateTime CreatedDate,
        DateTime LastEditedDate);
}
