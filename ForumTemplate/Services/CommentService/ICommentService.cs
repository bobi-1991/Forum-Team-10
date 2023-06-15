using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Services.CommentService
{
    public interface ICommentService
    {
        List<CommentResponse> GetAll();

        CommentResponse GetById(Guid id);

        CommentResponse Create(CommentRequest commentRequest);

        CommentResponse Update(Guid id, CommentRequest commentRequest);

        string Delete(Guid id);

        List<CommentResponse> GetComments(Guid postId);

        public void DeleteByPostId(Guid postId);

    }
}
