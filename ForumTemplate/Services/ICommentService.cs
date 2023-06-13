using ForumTemplate.Models;
using ForumTemplate.Models.Input;
using ForumTemplate.Models.Result;

namespace ForumTemplate.Services
{
    public interface ICommentService
    {
        List<CommentResultModel> GetAll();

        CommentResultModel GetById(int id);

        CommentResultModel Create(CommentInputModel comment);

        CommentResultModel Update(int id, CommentInputModel comment);

        string Delete(int id);

        List<CommentResultModel> GetComments(int postId);

        public void DeleteByPostId(int postId);
        
     }
}
