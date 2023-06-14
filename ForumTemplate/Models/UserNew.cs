namespace ForumTemplate.Models
{
    public sealed class UserNew
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        //  public IReadOnlyList<Post> Posts => this.posts.AsReadOnly(); // TODO: Implement
        //  public IReadOnlyList<Comment> Comments => this.comments.AsReadOnly(); // TODO: Implement
        //  public IReadOnlyList<Like> Likes => this.likes.AsReadOnly(); // TODO: Implement

        private UserNew()
        {
            
        }

        private UserNew(
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

        public static UserNew Create(
            string firstName,
            string lastName,
            string username,
            string email,
            string password)
        {
            return new UserNew(firstName, lastName, username, email, password);
        }
    }
}
