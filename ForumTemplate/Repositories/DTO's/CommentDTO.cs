namespace ForumTemplate.Repositories.DTO_s
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

    }
}
