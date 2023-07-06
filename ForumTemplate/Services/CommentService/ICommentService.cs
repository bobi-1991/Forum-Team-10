using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Services.CommentService
{
    public interface ICommentService
    {
        List<CommentResponse> GetAll();

        CommentResponse GetById(Guid id);

        CommentResponse Create(User loggedUser,CommentRequest commentRequest);

        CommentResponse Update(User loggedUser, Guid id, CommentRequest commentRequest);

        string Delete(User loggedUser,Guid id);

        List<CommentResponse> GetComments(Guid postId);

        public void DeleteByPostId(Guid postId);

    }
}
