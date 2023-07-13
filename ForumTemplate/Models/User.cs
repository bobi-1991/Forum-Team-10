using ForumTemplate.Models.ViewModels;

namespace ForumTemplate.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;        
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Country { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime UpdatedAt { get; private set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public bool IsDelete { get; set; }

        public User()
        {      
        }
        private User(
            string firstName,
            string lastName,
            string username,
            string email,
            string password,
            string country)
        {
            UserId = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            Password = password;
            Country = country;
        }
        public static User Create(
            string firstName,
            string lastName,
            string username,
            string email,
            string password,
            string country)
        {
            return new User(firstName, lastName, username, email, password, country);
        }
        public User Update(User user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Country = user.Country;
            this.Password = user.Password;
            this.Email = user.Email;
            this.UpdatedAt = DateTime.UtcNow;

            return this;
        }
        public User AdminEditionUpdatee(User user)
        {
            this.Username = user.Username;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Country = user.Country;
            this.Password = user.Password;
            this.Email = user.Email;
            this.IsAdmin = user.IsAdmin;
            this.IsBlocked = user.IsBlocked;
            this.UpdatedAt = DateTime.UtcNow;

            return this;
        }
    }           
}
