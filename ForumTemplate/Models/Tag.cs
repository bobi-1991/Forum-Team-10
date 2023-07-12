using System.ComponentModel.Design;

namespace ForumTemplate.Models
{
    public class Tag
    {
        public Guid TagId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign keys
        public Guid? UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;
        public bool IsDelete { get; set; }
        public Tag()
        {
        }
        public Tag(
            string content,
            Guid userId,
            Guid postId)
        {
            TagId = Guid.NewGuid();
            Content = content;
            CreatedAt = DateTime.UtcNow;
            UserId = userId;
            PostId = postId;
        }
        public static Tag Create(
            string content,
            Guid userId,
            Guid postId)
        {

            return new Tag(content, userId, postId);
        }
    }
}
