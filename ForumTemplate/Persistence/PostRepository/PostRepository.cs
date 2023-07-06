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
				.Where(x => !x.IsDelete)
				.Include(x => x.Likes)
                .Include(x => x.Comments)
                .ToList();
        }
        public IQueryable<Post> GetAllToQueriable()
        {
            return this.dbContext.Posts
                .Where(x => !x.User.IsDelete)
				.Where(x => !x.IsDelete)
				.Include(x => x.Likes);

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
            return dbContext.Posts
			    .Where(x => !x.User.IsDelete)
				.Where(x => !x.IsDelete)
				.Include(x => x.Likes)
				.Include(x => x.Comments)
				.FirstOrDefault(p => p.PostId == id);
        }
        public Post GetByTitle(string title)
        {
            return dbContext.Posts
			    .Where(x => !x.User.IsDelete)
				.Where(x => !x.IsDelete)
				.Include(x => x.Likes)
				.Include(x => x.Comments)
				.FirstOrDefault(p => p.Title == title);
        }

        public List<Post> GetByUserId(Guid id)
        {
            return dbContext.Posts
				  .Where(x => !x.User.IsDelete)
				.Where(x => !x.IsDelete)
				.Include(x => x.Likes)
				.Include(x => x.Comments)
				.Where(p => p.UserId == id)
                .ToList();
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
                return posts.FindAll(post => post.Title.Equals(title)).ToList();
            }

            return posts;
        }

        //not tested yet
        private static List<Post> FilterByMinLikes(List<Post> posts, string minLikes)
        {
            if (minLikes is not null)
            {
                var likes = Convert.ToInt32(minLikes);
                return posts.FindAll(post => post.Likes.Count() >= likes);
            }

            return posts;
        }

        //not tested yet
        private static List<Post> FilterByMaxLikes(List<Post> posts, string maxLikes)
        {
            if (maxLikes is not null)
            {
                var likes = Convert.ToInt32(maxLikes);
                return posts.FindAll(post => post.Likes.Count() <= likes);
            }

                return posts;
        }

        private static List<Post> SortBy(List<Post> posts, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "title":
                    return posts.OrderBy(post => post.Title).ToList();
                case "likes":
                    return posts.OrderBy(post => post.Likes.Count()).ToList();
                default:
                    return posts;
            }
        }

        private static List<Post> Order(List<Post> posts, string sortOrder)
        {
                switch (sortOrder)
                {
                case "desc":
                        return posts.OrderByDescending(x => x.Title).ToList();
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