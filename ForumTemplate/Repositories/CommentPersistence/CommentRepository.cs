using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Repositories.CommentPersistence;
using Microsoft.Extensions.Hosting;

namespace ForumTemplate.Repositories.CommentPersistence
{
    public class CommentRepository : ICommentRepository
    {
        private readonly List<Comment> comments = new();

        //public CommentRepository()
        //{
        //    this.comments.Add(Comment.Create("Comment1", Guid.NewGuid(), Guid.NewGuid()));
        //    this.comments.Add(Comment.Create("Comment2", Guid.NewGuid(), Guid.NewGuid()));
        //    this.comments.Add(Comment.Create("Comment3", Guid.NewGuid(), Guid.NewGuid()));
        //}

        public List<Comment> GetAll()
        {
            if (comments.Count == 0)
            {
                throw new EntityNotFoundException("Currently, there are no comments created.");
            }
            return comments;
        }

        public Comment GetById(Guid id)
        {
            return comments.Where(c => c.Id == id).FirstOrDefault();
        }

        public List<Comment> GetByPostId(Guid postId)
        {
            return comments.Where(c => c.PostId == postId).ToList();
        }

        public List<Comment> GetByUserId(Guid id)
        {
            return comments.Where(p => p.UserId == id).ToList();
        }

        public Comment Create(Comment comment)
        {
            comments.Add(comment);

            return comment;
        }

        public Comment Update(Guid id, Comment comment)
        {
            Comment commentToUpdate = GetById(id);
            return commentToUpdate.Update(comment);
        }

        public string Delete(Guid id)
        {
            Comment existingComment = GetById(id);
            comments.Remove(existingComment);

            return "Comment was successfully deleted.";
        }

        public void DeleteByPostId(Guid postId)
        {
            var commentToRemove = comments.Where(c => c.PostId == postId).ToList();
            foreach (var comment in commentToRemove)
            {
                comments.Remove(comment);
            }

            //return "Comments were successfully deleted.";
        }

    }
}
