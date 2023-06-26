namespace ForumTemplate.Models
{
    public class Tag
    {
        public Guid TagId { get; set; }
        public string Title { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public bool IsDelete { get; set; }
        public Tag()
        {
        }
        private Tag(string title)
        {
            Title = title;
        }
        public Tag Create(string description)
        {
            var tag = new Tag(description);

            return tag;
        }
    }
}
