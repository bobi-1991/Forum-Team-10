namespace ForumTemplate.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public ICollection<Post> Posts{ get; set; }
        public ICollection<Like> Likes{ get; set; }

        //  public IReadOnlyList<Post> Posts => this.posts.AsReadOnly(); // TODO: Implement
        //  public IReadOnlyList<Comment> Comments => this.comments.AsReadOnly(); // TODO: Implement
        //  public IReadOnlyList<Like> Likes => this.likes.AsReadOnly(); // TODO: Implement

        private User()
        {
            
        }

        private User(
            string firstName,
            string lastName,
            string username,
            string email,
            string password)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            Password = password;
            
        }

        public static User Create(
            string firstName,
            string lastName,
            string username,
            string email,
            string password)
        {
            return new User(firstName, lastName, username, email, password);
        }
        public User Update(User user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Password = user.Password;
            this.Email = user.Email;
            this.UpdatedAt = DateTime.UtcNow;

            return this;
        }        
    }           
}
