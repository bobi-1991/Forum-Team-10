using ForumTemplate.Authorization;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Persistence.LikeRepository;
using ForumTemplate.Persistence.PostRepository;

namespace ForumTemplate.Services.LikeService
{
    public class LikeService:ILikeService
    {
        private readonly IPostRepository postRepository;
        private readonly ILikeRepository likeRepository;
        public LikeService(IPostRepository postRepository, ILikeRepository likeRepository)
        {
            this.postRepository = postRepository;
            this.likeRepository = likeRepository;
        }

        public string LikeUnlike(Guid postId)
        {
            if (CurrentLoggedUser.LoggedUser is null || !CurrentLoggedUser.LoggedUser.IsLogged)
            {
                throw new EntityLoginException("Please log in first.");
            }

            var userId = CurrentLoggedUser.LoggedUser.UserId;
            var post = postRepository.GetById(postId);

            if (post is null)
            {
                throw new EntityNotFoundException("The current post not found.");
            }

            var like = likeRepository.GetLikeByPostAndUserId(post, userId);

            if (like is null)
            {
                var newLike = likeRepository.Create(userId, post.PostId);
                likeRepository.AddLikeInDatabase(newLike);

                return "You like this post.";
            }
            if (!like.Liked)
            {
                likeRepository.UpdateInDatabase(like);

                return "You like this post.";
            }

            likeRepository.UpdateInDatabase(like);

            return "You unlike this post.";
        }

        public void DeleteByPostId(Guid postId)
        {

            var deletedLikes = likeRepository.DeleteByPostId(postId);
        }

        public void DeleteByUserId(Guid userId)
        {
            var deletedLikes = likeRepository.DeleteByUserId(userId);
        }
    }
}
