using ForumTemplate.Data;
using ForumTemplate.Models;
using ForumTemplate.Persistence.PostRepository;

namespace ForumTemplate.Persistence.LikeRepository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationContext dbContext;
        public LikeRepository(ApplicationContext dbContext, IPostRepository postRepository)
        {
            this.dbContext = dbContext;
        }
        public Like GetLikeByPostAndUserId(Post post, Guid userId)
        {
            return dbContext.Likes
                .Where(x => !x.IsDelete)
                .FirstOrDefault(x => x.UserId.Equals(userId) && x.PostId.Equals(post.PostId));
        }
        public Like Create(Guid userId, Guid postId)
        {
            return Like.Create(userId, postId);
        }

        public void AddLikeInDatabase(Like like)
        {
            like.Liked = true;
            dbContext.Likes.Add(like);
            dbContext.SaveChanges();
        }

        public void UpdateInDatabase(Like like)
        {
            if (like.Liked)
            {
                like.Liked = false;
                dbContext.Update(like);
                dbContext.SaveChanges();
            }
            else
            {
                like.Liked = true;
                dbContext.Update(like);
                dbContext.SaveChanges();

            }
        }

        public List<Like> GetByPostId(Guid postId)
        {
            return dbContext.Likes
                .Where(x => !x.IsDelete)
                .Where(x => x.PostId == postId)
                .ToList();
        }
        public List<Like> GetByUserId(Guid userId)
        {
            return dbContext.Likes
                            .Where(x => !x.IsDelete)
                            .Where(x => x.UserId == userId)
                            .ToList();
        }

        public List<Like> DeleteByPostId(Guid postId)
        {
            var likeToRemove = GetByPostId(postId);
            var result = new List<Like>();

            foreach (var like in likeToRemove)
            {
                like.IsDelete = true;
                result.Add(like);
            }

            dbContext.SaveChanges();
            return result;
        }

        public List<Like> DeleteByUserId(Guid userId)
        {
            var likeToRemove = GetByUserId(userId);
            var result = new List<Like>();

            foreach (var like in likeToRemove)
            {
                like.IsDelete = true;
                result.Add(like);
            }

            dbContext.SaveChanges();
            return result;
        }
        public void DeleteLikes(IEnumerable<ICollection<Like>> likes)
        {
            foreach (var listOfLikes in likes)
            {
                foreach (var like in listOfLikes)
                {
                    like.IsDelete = true;
                }
            }
            dbContext.SaveChanges();
        }
    }
}