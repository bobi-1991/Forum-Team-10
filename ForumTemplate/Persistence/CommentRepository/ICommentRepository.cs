using ForumTemplate.Models;

namespace ForumTemplate.Persistence.CommentRepository
{
    public interface ICommentRepository
    {
        List<Comment> GetAll();
        Comment GetById(Guid id);
        Comment Create(Comment comment);
        Comment Update(Guid id, Comment comment);
        string Delete(Guid id);
        List<Comment> GetByUserId(Guid id);
        List<Comment> GetByPostId(Guid postId);
        void DeleteByPostId(Guid postId);
    }
}
