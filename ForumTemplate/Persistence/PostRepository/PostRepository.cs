using ForumTemplate.Data;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Services.PostService;
using Microsoft.EntityFrameworkCore;

namespace ForumTemplate.Persistence.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationContext dbContext;
        public PostRepository(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Post> GetAll()
        {
            return this.dbContext.Posts
                .Where(x => !x.User.IsDelete)
                .Include(x=>x.Likes)
                .Include(x=>x.Comments)
                .ToList();
        }
        public List<Post> FilterBy(PostQueryParameters filterParameters)
        {
            List<Post> result = this.GetAll();
            result = FilterByTitle(result, filterParameters.Title);
            result = FilterByMinLikes(result, filterParameters.MinLikes);
            result = FilterByMaxLikes(result, filterParameters.MaxLikes);
            result = SortBy(result, filterParameters.SortBy);
            result = Order(result, filterParameters.SortOrder);

            return result.ToList();
        }

        public Post GetById(Guid id)
        {
            return dbContext.Posts.FirstOrDefault(p => p.PostId == id);
        }
        public Post GetByTitle(string title)
        {
             return dbContext.Posts.FirstOrDefault(p => p.Title == title);
        }

        public List<Post> GetByUserId(Guid id)
        {
            return dbContext.Posts.Where(p => p.UserId == id).ToList();
        }

        public Post Create(Post post)
        {
            this.dbContext.Posts.Add(post);
            dbContext.SaveChanges();
            return post;
        }

        public Post Update(Guid id, Post post)
        {
            Post postToUpdate = GetById(id);
            var updatedPost = postToUpdate.Update(post);

            dbContext.Update(updatedPost);
            dbContext.SaveChanges();

            return updatedPost;
        }
        public string Delete(Guid id)
        {
            var post = dbContext.Posts.FirstOrDefault(x => x.PostId == id);

            if (post != null)
            {
                post.IsDelete = true;
                dbContext.SaveChanges();
            }

            return "Post was successfully deleted.";
        }

        public void DeletePosts(List<Post> postsToDelete)
        {
            foreach (var post in postsToDelete)
            {
                post.IsDelete = true;
                dbContext.Update(post);
            }
            dbContext.SaveChanges();
        }

        public bool Exist(Guid id)
        {
            return dbContext.Posts.Any(p => p.PostId == id);
        }



        //   Queryable methods
        //   Not tested yet


        private static List<Post> FilterByTitle(List<Post> posts, string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var sortedPosts = posts.Where(post => post.Title.Equals(title)).ToList();
                return sortedPosts;
            }
            else
            {
                return posts;
            }
        }

        private static List<Post> FilterByContent(List<Post> posts, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var sortedPosts = posts.Where(post => post.Content.Equals(content)).ToList();
                return sortedPosts;
            }
            else
            {
                return posts;
            }
        }

        //not tested yet
        private static List<Post> FilterByMinLikes(List<Post> posts, double? minLikes)
        {
            if (minLikes.HasValue)
            {
                return posts.Where(post => post.Likes.Count() >= minLikes).ToList();
            }
            else
            {
                return posts;
            }
        }

        //not tested yet
        private static List<Post> FilterByMaxLikes(List<Post> posts, double? maxLikes)
        {
            if (maxLikes.HasValue)
            {
                return posts.Where(post => post.Likes.Count() <= maxLikes).ToList();
            }
            else
            {
                return posts;
            }
        }

        private static List<Post> SortBy(List<Post> posts, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "title":
                    return posts.OrderBy(post => post.Title).ToList();
                default:
                    return posts;
            }
        }

        private static List<Post> Order(List<Post> posts, string sortOrder)
        {

            switch (sortOrder)
            {
                case "desc":
                    return posts.OrderByDescending(x=>x.Title).ToList();
                default:
                    return posts;
            }
        }

        // Must be tested
        private List<Post> GetPosts()
        {
            return this.dbContext.Posts
                .Include(x => x.Likes)
                .ToList();
        }
    }
}