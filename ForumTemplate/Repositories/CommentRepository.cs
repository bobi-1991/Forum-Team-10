using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Repositories.DTO_s;
using Microsoft.Extensions.Hosting;

namespace ForumTemplate.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly List<CommentDTO> comments;

        public CommentRepository()
        {
            this.comments = new List<CommentDTO>()
            {
                new CommentDTO { Id = 1, Content = "Comment1", UserId = 1, PostId = 1 },
                new CommentDTO { Id = 2, Content = "Comment2",  UserId = 2, PostId = 2 },
                new CommentDTO { Id = 3, Content = "Comment3",  UserId = 2, PostId = 2 }
            };
        }

        public List<CommentDTO> GetAll()
        {
            return this.comments;
        }

        public CommentDTO GetById(int id)
        {
            return comments.Where(c => c.Id == id).FirstOrDefault();
        }

        public List<CommentDTO> GetByPostId(int postId)
        {
            return comments.Where(c => c.PostId == postId).ToList();
        }

        public List<CommentDTO> GetByUserId(int id)
        {
            return comments.Where(p => p.UserId == id).ToList();
        }

        public CommentDTO Create(CommentDTO comment)
        {
            comment.Id = this.comments.Count + 1;
            this.comments.Add(comment);

            return comment;
        }

        public CommentDTO Update(int id, CommentDTO comment)
        {
            CommentDTO commentToUpdate = this.GetById(id);
            commentToUpdate.Content = comment.Content;

            return commentToUpdate;
        }

        public string Delete(int id)
        {
            CommentDTO existingComment = GetById(id);
            comments.Remove(existingComment);

            return "Comment was successfully deleted.";
        }

        public void DeleteByPostId(int postId)
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
