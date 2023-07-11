namespace ForumTemplate.DTOs.CommentDTOs
{
    //public record CommentRequest(
    //   string? Content,
    //   Guid UserId,
    //   Guid PostId);
    public class CommentRequest
    { 
    public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
