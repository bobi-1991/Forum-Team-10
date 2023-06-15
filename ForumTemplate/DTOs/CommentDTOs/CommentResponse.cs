namespace ForumTemplate.DTOs.CommentDTOs
{
    public record CommentResponse(
       Guid Id,
       string Content,
       Guid UserId,
       Guid PostId,
       DateTime CreatedDate,
       DateTime UpdatedDate);

}
