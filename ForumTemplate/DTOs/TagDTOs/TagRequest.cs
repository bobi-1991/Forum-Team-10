namespace ForumTemplate.DTOs.TagDTOs
{
    public class TagRequest
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
