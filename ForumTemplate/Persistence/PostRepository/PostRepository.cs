using ForumTemplate.Common.FilterModels;
using ForumTemplate.Data;
using ForumTemplate.Models;
using ForumTemplate.Services.PostService;
using Microsoft.EntityFrameworkCore;

namespace ForumTemplate.Persistence.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly List<Post> posts = new List<Post>();

        //private readonly ApplicationContext dbContext;
        //public PostRepository(ApplicationContext dbContext)
        //{
        //    this.dbContext = dbContext;
        //}

        public List<Post> GetAll()
        {
            return this.posts.ToList();

            //return this.dbContext.Posts.Where(x=>!x.User.IsDelete)
            //    .ToList();
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

           // return dbContext.Posts.FirstOrDefault(p => p.PostId == id);
        }
        public Post GetByTitle(string title)
        {
            return posts.Where(p => p.Title == title).FirstOrDefault();

            // return dbContext.Posts.FirstOrDefault(p => p.Title == title);
        }

        public List<Post> GetByUserId(Guid id)
        {
            return posts.Where(p => p.UserId == id).ToList();

          //  return dbContext.Posts.Where(p => p.UserId == id).ToList();
        }

        public Post Create(Post post)
        {
            this.posts.Add(post);
            return post;

            //this.dbContext.Posts.Add(post);
            //dbContext.SaveChanges();
            //return post;
        }

        public Post Update(Guid id, Post post)
        {
            Post postToUpdate = GetById(id);
            postToUpdate.Update(post);

            return postToUpdate;


           // Post postToUpdate = GetById(id);
            //var updatedPost = postToUpdate.Update(post);

            //dbContext.Update(updatedPost);
            //dbContext.SaveChanges();

            //return updatedPost;
        }
        public string Delete(Guid id)
        {
            Post existingPost = GetById(id);
            posts.Remove(existingPost);

            return "Post was successfully deleted.";

            //var post = dbContext.Posts.FirstOrDefault(x => x.PostId == id);

            //if (post != null)
            //{
            //    post.IsDelete = true;
            //    dbContext.SaveChanges();
            //}

            //return "Post was successfully deleted.";
        }

        public bool Exist(Guid id)
        {
            return posts.Any(p => p.PostId == id);

          //  return dbContext.Posts.Any(p => p.PostId == id);
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


        // Must be tested
        //private IQueryable<Post> GetPosts()
        //{
        //    return this.dbContext.Posts
        //        .Include(x => x.Likes);

        //}



    }

}