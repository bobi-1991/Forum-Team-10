using ForumTemplate.Models;

namespace ForumTemplate.Services.LikeService
{
    public interface ILikeService
    {
        string LikeUnlike(User loggedUser, Guid postId);
        void DeleteByPostId(Guid postId);
        void DeleteByUserId(Guid userId);
    }
}
