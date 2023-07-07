using ForumTemplate.Models;
using ForumTemplate.Persistence.LikeRepository;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Validation;

namespace ForumTemplate.Services.LikeService
{
    public class LikeService:ILikeService
    {
        private readonly IPostRepository postRepository;
        private readonly ILikeRepository likeRepository;
        private readonly IPostsValidator postValidator;

        public LikeService(IPostRepository postRepository, ILikeRepository likeRepository, IPostsValidator postValidator)
        {
            this.postRepository = postRepository;
            this.likeRepository = likeRepository;
            this.postValidator = postValidator;
        }

        public string LikeUnlike(User loggedUser, Guid postId)
        {
            postValidator.Validate(postId);
            
            var post = postRepository.GetById(postId);
            var like = likeRepository.GetLikeByPostAndUserId(post, loggedUser.UserId);

            if (like is null)
            {
                var newLike = likeRepository.Create(loggedUser.UserId, post.PostId);
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
