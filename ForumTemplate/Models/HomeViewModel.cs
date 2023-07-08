namespace ForumTemplate.Models
{
    public class HomeViewModel
    {
        public List<Post> TopCommentedPosts { get; set; }
        public List<Post> RecentlyCreatedPosts { get; set; }
        public int TotalPostCount { get; set; }
        public int TotalUserCount { get; set; }
    }
}
