namespace ForumTemplate.Models
{
    public class Like
    {
        // Properties
        public Guid Id { get; private set; }
        // Foreign keys
        public Guid UserId { get; private set; }
        public Guid PostId { get; private set; }

        // Navigation properties
        public User User { get; private set; }
        public Post Post { get; private set; }

        private Like()
        {

        }

        private Like(Guid userId, Guid postId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            PostId = postId;
        }

        // Methods
        public Like Create(Guid userId, Guid postId)
        {
            return new Like(userId,postId);
        }

    }
}
