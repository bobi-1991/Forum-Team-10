using ForumTemplate.Models;
using ForumTemplate.Repositories.DTO_s;

namespace ForumTemplate.Repositories
{
    public interface ICommentRepository
    {
        List<CommentDTO> GetAll();

        CommentDTO GetById(int id);
        
        CommentDTO Create(CommentDTO comment);

        CommentDTO Update(int id, CommentDTO comment);

        string Delete(int id);

        List<CommentDTO> GetByUserId(int id);

        List<CommentDTO> GetByPostId(int postId);

        void DeleteByPostId(int postId);

    }
}
