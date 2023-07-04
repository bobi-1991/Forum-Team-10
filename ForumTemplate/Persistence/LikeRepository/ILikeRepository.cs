using ForumTemplate.Models;

namespace ForumTemplate.Persistence.LikeRepository
{
    public interface ILikeRepository
    {
        Like GetLikeByPostAndUserId(Post post, Guid userId);
        Like Create(Guid userId, Guid postId);
        void AddLikeInDatabase(Like like);
        void UpdateInDatabase(Like like);
        List<Like> GetByPostId(Guid postId);
        List<Like> GetByUserId(Guid userId);
        List<Like> DeleteByPostId(Guid postId);
        List<Like> DeleteByUserId(Guid userId);
        void DeleteLikes(IEnumerable<ICollection<Like>> likes);
    }
}
