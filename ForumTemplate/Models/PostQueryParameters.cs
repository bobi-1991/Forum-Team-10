namespace ForumTemplate.Models
{
    public class PostQueryParameters
    {
        public string Title { get; set; } = null!;
        public int MinLikes { get; set; }
        public int MaxLikes { get; set; }
        public string SortBy { get; set; } = null!;
        public string SortOrder { get; set; } = null!;
    }
}
