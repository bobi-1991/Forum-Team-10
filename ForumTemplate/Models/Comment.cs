namespace ForumTemplate.Models
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Foreign keys
        public Guid? UserId { get; set; }
        public Guid PostId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Post Post { get; set; }

        private Comment()
        {
            // Private parameterless constructor for EF Core
            // to enable entity creation via reflection
        }

        private Comment(         
            string content,
            Guid userId,
            Guid postId)
        {
            Id = Guid.NewGuid();
            Content = content;
            CreatedAt = DateTime.UtcNow;
            UserId = userId;
            PostId = postId;
        }

        public static Comment Create(
            string content,
            Guid userId,
            Guid postId)
        {
            // Enforce invariants
            return new Comment(content, userId, postId);
        }

        public Comment Update(Comment comment)
        {
            this.Content = comment.Content;
            this.UpdatedAt = DateTime.UtcNow;

            return this;
        }


    }
}
