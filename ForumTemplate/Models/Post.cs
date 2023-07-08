
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumTemplate.Models
{
    public class Post
    {
        // Properties
        public Guid PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    //    public int LikePost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign key
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public bool IsDelete { get; set; }
        public Post()
        {
            // Private parameterless constructor for EF Core
            // to enable entity creation via reflection
        }
        private Post(
            string title,
            string content,
            Guid userId
            )
        {
            PostId = Guid.NewGuid();
            Title = title;
            Content = content;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        // Methods
        public static Post CreatePost(string title, string content, Guid userId)
        {
            return new Post(title, content, userId);
        }
        public Post Update(Post post)
        {
            this.Title = post.Title;
            this.Content = post.Content;
            this.UpdatedAt = DateTime.UtcNow;

            return this;
        }
    }
}
