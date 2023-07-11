namespace ForumTemplate.Models.ViewModels
{
	public class CommentViewModel
	{
        public Guid CommentViewModelId { get; set; }

        public Guid Id { get; set; }
		public string Content { get; set; }
	}
}
