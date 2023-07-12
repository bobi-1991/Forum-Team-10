using ForumTemplate.Data;
using ForumTemplate.Models;

namespace ForumTemplate.Persistence.TagRepository
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationContext dbContext;

        public TagRepository(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Tag GetById(Guid id)
        {
            return dbContext.Tags
                .Where(x => !x.User.IsDelete)
                .Where(x => !x.IsDelete)
                .Where(x => !x.Post.IsDelete)
                .FirstOrDefault(t => t.TagId == id);
        }
        public List<Tag> GetByPostId(Guid postId)
        {
            return dbContext.Tags
                .Where(x => !x.IsDelete)
                .Where(x => !x.Post.IsDelete)
                .Where(x => x.PostId == postId).ToList();
        }

        public List<Tag> GetByUserId(Guid id)
        {
            return dbContext.Tags
                 .Where(x => !x.IsDelete)
                 .Where(x => !x.Post.IsDelete)
                 .Where(x => !x.User.IsDelete)
                 .Where(p => p.UserId == id).ToList();
        }

        public Tag Create(Tag tag)
        {
            this.dbContext.Tags.Add(tag);
            dbContext.SaveChanges();
            return tag;
        }

        public string Delete(Guid id)
        {
            var tag = dbContext.Tags.FirstOrDefault(x => x.TagId == id);

            if (tag != null)
            {
                tag.IsDelete = true;
                dbContext.SaveChanges();
            }

            return "Tag was successfully deleted.";
        }

        public void DeleteByPostId(Guid postId)
        {
            var tagToRemove = dbContext.Tags.Where(t => t.PostId == postId).ToList();

            foreach (var tag in tagToRemove)
            {
                tag.IsDelete = true;
                dbContext.Update(tag);
            }

            dbContext.SaveChanges();
        }
    }
}
