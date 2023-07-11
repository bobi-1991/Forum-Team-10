namespace ForumTemplate.Models.ViewModels
{
    public class UserViewModel:User
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Country { get; set; }
        public string IsBlocked { get; set; }
        public string Role { get; set; }
    }
}
