using System.ComponentModel.DataAnnotations.Schema;

namespace ForumTemplate.Models
{
    public class Like
    {
        // Properties
        public Guid LikeId { get; set; }
        public bool Liked { get; set; }
        // Foreign keys
        public Guid? UserId { get; set; }
        public Guid PostId { get; set; }


        // Navigation properties
        public User User { get; set; } = null!;
        public Post Post { get; set; } = null!;
        public bool IsDelete { get; set; }

        public Like()
        {
        }

        public Like(Guid userId, Guid postId)
        {
            LikeId = Guid.NewGuid();
            UserId = userId;
            PostId = postId;
        }

        // Methods
        public static Like Create(Guid userId, Guid postId)
        {
            return new Like(userId,postId);
        }
    }
}
