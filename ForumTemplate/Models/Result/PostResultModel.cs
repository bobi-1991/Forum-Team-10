namespace ForumTemplate.Models.Result
{
    public class PostResultModel
    {
        public string Description { get; set; }

        public string Title { get; set; }

        public List<CommentResultModel> Comments { get; set; } = new();

    }
}
