
# Forum API
- [Forum API](#forum-api)
  - [Models](#models)
      - [User Model](#user-model)
      - [Post Model](#post-model)
        - [:question: IReadOnlyList](#question-ireadonlylist)
    - [Comment Model](#comment-model)
      - [Like Model](#like-model)
      - [Tag Model](#tag-model)

## Models

#### User Model

```csharp
public sealed class User
{
    // Members
    private List<Post> _posts = new();
    private List<Comment> _comments = new();
    private List<Like> _likes = new();

    // Properties
    public int UserId {get; private set;}
    public string FirstName {get; private set;}
    public string LastName {get; private set;}
    public string Username {get; private set;}
    public string Email {get; private set;}
    public string Password {get; private set;} // to be hashed

    // Navigation properties
    public IReadOnlyList<Post> Posts => _posts.AsReadOnly();
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();
    public IReadOnlyList<Like> Likes => _likes.AsReadOnly();

    private User()
    {
        // Private parameterless constructor for EF Core
        // to enable entity creation via reflection
    }

    private User(
        string firstName,
        string LastName,
        string username,
        string email,
        string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = userName;
        Password = password;
    }

    public User Create(
        string firstName,
        string lastName
        string username,
        string email,
        string password);

    // Methods
    public void AddPost(string title, string content);
    public void EditPost(Post post, string newContent);
    public void DeletePost(Post post);

    public void AddComment(Comment comment);
    public void EditComment(Comment comment, string newContent);
    public void DeleteComment(Comment comment);

    public void LikePost(Post post);
    public void UnlikePost(Post post);
}
```

#### Post Model

```csharp
public sealed class Post
{
    // Members
    private List<Comment> _comments = new();
    private List<Like> _likes = new();
    private List<Tag> _tags = new();

    // Properties
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Foreign key
    public int UserId { get; set; }

    // Navigation properties
    public User User { get; set; }
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();
    public IReadOnlyList<Like> Likes => _likes.AsReadOnly();
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();

    private Post()
    {
        // Private parameterless constructor for EF Core
        // to enable entity creation via reflection
    }

    private Post(
        string title,
        string content,
        int userId)
    {
        Title = title;
        Content = content;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    // Methods
    public Post CreatePost(
        string title,
        string content
        User user
    );

    public void AddComment(Comment comment);

    public void AddLike(User user);

    public void RemoveLike(User user);

    public void AddTag(Tag tag);
}
```

##### :question: IReadOnlyList
> **Пояснение:**
    - **`IreadOnlyList<T>`** е интерфейс, който можем да ползваме в нашите модели за да предоставим read-only access за нашите колекции.
    - Този интерфейс се грижи за това колекциите да не могат да бъдат модифицирани, извън нашия модел.
    - Имеплементира се като дефинираме property в нашия модел с return type **`IReadOnlyList<T>`**, след това конвертираме нашата колекция (*тази, която искаме да върнем като readonly*) с **`AsReadOnly()`** метода.

### Comment Model

```csharp
public sealed class Comment
{
    public int Id {get; private set;}
    public string Content {get; private set;}
    public DateTime CreatedAt {get; private set;}
    public DateTime UpdatedAt {get; private set;}

    // Foreign keys
    public int UserId {get; set;}
    public int PostId {get; set;}

    // Navigation properties
    public User User {get; set;}
    public Post Post {get; set;}

    private Comment()
    {
        // Private parameterless constructor for EF Core
        // to enable entity creation via reflection
    }

    private Comment(
        string content,
        int userId,
        int postId)
    {
        Content = content;
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
        PostId = postId;
    }

    public Comment Create(
        string content,
        int userId,
        int postId)
    {
        // Enforce invariants
        var comment = new Comment(
        content,
        int userId,
        int postId);

        return comment;
    }

    public void Update(Comment comment);
}
```

#### Like Model

```csharp
public sealed class Like
{
    // Properties
    public int Id { get; private set; } 
    // Foreign keys
    public int UserId { get; private set; }
    public int PostId { get; private set; }

    // Navigation properties
    public User User { get; private set; }
    public Post Post { get; private set; }

    private Like()
    {

    }

    private Like(int userId, int postId)
    {
        UserId = userId;
        PostId = postId;
    }

    // Methods
    public Like Create(int userId, int postId)
    {
        var like = new Like(
            userId,
            postId);

        return like;
    }
}
```

#### Tag Model

```csharp
public sealed class Tag
{
    public int Id {get; private set;}
    public string Description {get; private set;}

    private Tag()
    {

    }

    private Tag(string description)
    {
        Description = description;
    }

    public Tag Create(string description)
    {
        var tag = new Tag(description);

        return tag;
    }
}
```


