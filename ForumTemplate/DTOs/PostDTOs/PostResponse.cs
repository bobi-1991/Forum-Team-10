using ForumTemplate.DTOs.CommentDTOs;

namespace ForumTemplate.DTOs.PostDTOs
{
    public record PostResponse(
        Guid Id,
        string Title,
        string Content,
        Guid UserId,
        int Likes,
        List<CommentResponse> Comments,
        DateTime CreatedDate,
        DateTime LastEditedDate);
}
