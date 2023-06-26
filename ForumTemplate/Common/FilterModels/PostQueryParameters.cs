using ForumTemplate.Models;

namespace ForumTemplate.Common.FilterModels
{
    public class PostQueryParameters
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public double?  MinLikes { get; set; }
        public double? MaxLikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string SortBy { get; set; } = null!;
        public string SortOrder { get; set; } = null!;

    }
}
