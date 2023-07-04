namespace ForumTemplate.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign keys
        public Guid? UserId { get; set; } 
        public User User { get; set; } = null!;
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;
        public bool IsDelete { get; set; }

        public Comment()
        {
        }
        public Comment(
            string content,
            Guid userId,
            Guid postId)
        {
            CommentId = Guid.NewGuid();
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
            this.UserId = comment.UserId;
            this.PostId = comment.PostId;
            this.UpdatedAt = DateTime.UtcNow;

            return this;
        }
    }
}
