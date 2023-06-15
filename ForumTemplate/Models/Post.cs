using System.Xml.Linq;

namespace ForumTemplate.Models
{
    public class Post
    {
        // Members
        //private List<Comment> _comments = new();
        //private List<Like> _likes = new();
      //  private List<Tag> _tags = new();

        // Properties
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign key
        public Guid UserId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<Like> Likes { get; set; }

        //public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();
        //public IReadOnlyList<Like> Likes => _likes.AsReadOnly();
        //   public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();

        public Post()
        {
            // Private parameterless constructor for EF Core
            // to enable entity creation via reflection
        }

        private Post(
            string title,
            string content,
            Guid userId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Content = content;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        // Methods
        public static Post CreatePost(string title, string content, Guid id)
        {
            return new Post(title, content, id);
        }
        public Post Update(Post post)
        {
            this.Content = post.Content;
            this.UpdatedAt = DateTime.UtcNow;

            return this;
        }


        //public void AddComment(Comment comment)
        //{
        //    this._comments.Add(comment);
        //}




        //public void AddLike(User user)
        //{
        //    this._likes.Add(new Like(this.Id, user.Id));
        //}


        //public void RemoveLike(User user)
        //{ 


        //}

        //public void AddTag(Tag tag);

    }
}
