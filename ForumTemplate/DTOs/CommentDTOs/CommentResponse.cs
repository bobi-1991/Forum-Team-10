namespace ForumTemplate.DTOs.CommentDTOs
{
    public record CommentResponse(
       Guid Id,
       string Content,
       string Username,
       string PostTitle,
       DateTime CreatedDate,
       DateTime UpdatedDate);

}
