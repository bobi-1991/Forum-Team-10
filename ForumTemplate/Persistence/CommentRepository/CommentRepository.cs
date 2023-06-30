using ForumTemplate.Data;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumTemplate.Persistence.CommentRepository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext dbContext;
        public CommentRepository(ApplicationContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Comment> GetAll()
        {
            if (dbContext.Comments.Count() == 0)
            {
                throw new EntityNotFoundException("Currently, there are no comments created.");
            }
            return dbContext.Comments.Where(x => !x.User.IsDelete).ToList();
        }

        public Comment GetById(Guid id)
        {
            return dbContext.Comments.FirstOrDefault(c => c.CommentId == id);
        }

        public List<Comment> GetByPostId(Guid postId)
        {
            return dbContext.Comments.Where(x => x.PostId == postId).ToList();      
        }

        public List<Comment> GetByUserId(Guid id)
        {
           return dbContext.Comments.Where(p => p.UserId == id).ToList();
        }

        public Comment Create(Comment comment)
        {
            this.dbContext.Comments.Add(comment);
            dbContext.SaveChanges();
            return comment;
        }

        public Comment Update(Guid id, Comment comment)
        {
            Comment commentToUpdate = GetById(id);
            var updatedComment = commentToUpdate.Update(comment);

            dbContext.Update(updatedComment);
            dbContext.SaveChanges();

            return updatedComment;
        }

        public string Delete(Guid id)
        {
            //  var comment = dbContext.Users.FirstOrDefault(x => x.UserId == id);
            var comment = dbContext.Comments.FirstOrDefault(x => x.CommentId == id);

            if (comment != null)
            {
                comment.IsDelete = true;
                dbContext.SaveChanges();
            }

            return "Comment was successfully deleted.";
        }

        public void DeleteByPostId(Guid postId)
        {
            var commentToRemove = dbContext.Comments.Where(c => c.PostId == postId).ToList();

            foreach (var comment in commentToRemove)
            {
                dbContext.Remove(comment);
            }

            dbContext.SaveChanges();
        }
    }
}
