using ForumTemplate.Common.FilterModels;
using ForumTemplate.Models;

namespace ForumTemplate.Persistence.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly List<Post> posts = new();
        //public PostRepository()
        //{
        //    this.posts.Add(Post.CreatePost("Post1", "First Post", Guid));
        //    this.posts.Add(Post.CreatePost("Post1", "First Post", Guid.NewGuid()));
        //    this.posts.Add(Post.CreatePost("Post1", "First Post", Guid.NewGuid()));
        //}
        public List<Post> GetAll()
        {
            return this.posts;
        }
        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            IQueryable<Post> result = (IQueryable<Post>)this.GetAll();
            result = FilterByTitle(result, filterParameters.Title);
            result = FilterByContent(result, filterParameters.Content);
            result = FilterByMinLikes(result, filterParameters.MinLikes);
            result = FilterByMaxLikes(result, filterParameters.MaxLikes);
            result = SortBy(result, filterParameters.SortBy);
            result = Order(result, filterParameters.SortOrder);

            return result.ToList();
        }

        public Post GetById(Guid id)
        {
            return posts.Where(p => p.PostId == id).FirstOrDefault();
        }
        public Post GetByTitle(string title)
        {
            return posts.Where(p => p.Title == title).FirstOrDefault();
        }

        public List<Post> GetByUserId(Guid id)
        {
            return posts.Where(p => p.UserId == id).ToList();
        }

        public Post Create(Post post)
        {
            this.posts.Add(post);
            return post;
        }

        public Post Update(Guid id, Post post)
        {
            Post postToUpdate = GetById(id);
            postToUpdate.Update(post);

            return postToUpdate;
        }
        public string Delete(Guid id)
        {
            Post existingPost = GetById(id);
            posts.Remove(existingPost);

            return "Post was successfully deleted.";
        }

        public bool Exist(Guid id)
        {
            return posts.Any(p => p.PostId == id);
        }



        //   Queryable methods
        //   Not tested yet


        private static IQueryable<Post> FilterByTitle(IQueryable<Post> posts, string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                return posts.Where(post => post.Title.Contains(title));
            }
            else
            {
                return posts;
            }
        }

        private static IQueryable<Post> FilterByContent(IQueryable<Post> posts, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                return posts.Where(post => post.Content.Contains(content));
            }
            else
            {
                return posts;
            }
        }
        
        private static IQueryable<Post> FilterByMinLikes(IQueryable<Post> posts, double? minLikes)
        {
            if (minLikes.HasValue)
            {
                return posts.Where(post => post.Likes.Count() >= minLikes);
            }
            else
            {
                return posts;
            }
        }

        private static IQueryable<Post> FilterByMaxLikes(IQueryable<Post> posts, double? maxLikes)
        {
            if (maxLikes.HasValue)
            {
                return posts.Where(post => post.Likes.Count() <= maxLikes);
            }
            else
            {
                return posts;
            }
        }

        private static IQueryable<Post> SortBy(IQueryable<Post> posts, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "title":
                    return posts.OrderBy(post => post.Title);
                default:
                    return posts;
            }
        }

        private static IQueryable<Post> Order(IQueryable<Post> posts, string sortOrder)
        {
            switch (sortOrder)
            {
                case "desc":
                    return posts.Reverse();
                default:
                    return posts;
            }
        }

        //private IQueryable<Post> GetPosts()
        //{
        //    return this.context.Posts

        //        // To Do
        //}



    }

}