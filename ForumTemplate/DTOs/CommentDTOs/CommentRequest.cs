namespace ForumTemplate.DTOs.CommentDTOs
{
    public record CommentRequest(
       string Content,
       Guid UserId,
       Guid PostId);


}
