namespace ForumTemplate.Services.LikeService
{
    public interface ILikeService
    {
        string LikeUnlike(Guid postId);
        void DeleteByPostId(Guid postId);
        void DeleteByUserId(Guid userId);
    }
}
