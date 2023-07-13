namespace ForumTemplate.Models.ViewModels
{
	public class AdminEditViewModel
	{
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Country { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
    }
}
