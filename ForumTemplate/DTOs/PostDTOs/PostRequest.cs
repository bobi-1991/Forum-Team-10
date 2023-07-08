namespace ForumTemplate.DTOs.PostDTOs
{
    public class PostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }
}
