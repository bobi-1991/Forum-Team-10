namespace ForumTemplate.DTOs.CommentDTOs
{
    public class CommentRequest
    { 
    public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
