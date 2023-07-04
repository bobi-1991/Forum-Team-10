using ForumTemplate.Authorization;
using ForumTemplate.Exceptions;
using ForumTemplate.Models;
using ForumTemplate.Persistence.LikeRepository;
using ForumTemplate.Persistence.PostRepository;
using ForumTemplate.Validation;
using Microsoft.AspNetCore.Identity;

namespace ForumTemplate.Services.LikeService
{
    public class LikeService:ILikeService
    {
        private readonly IPostRepository postRepository;
        private readonly ILikeRepository likeRepository;
        private readonly IUserAuthenticationValidator userValidator;
        private readonly IPostsValidator postValidator;
        public LikeService(IPostRepository postRepository, ILikeRepository likeRepository, IUserAuthenticationValidator userValidator, IPostsValidator postValidator)
        {
            this.postRepository = postRepository;
            this.likeRepository = likeRepository;
            this.userValidator = userValidator;
            this.postValidator = postValidator;
        }

        public string LikeUnlike(Guid postId)
        {
            userValidator.ValidateUserIsLogged();
            postValidator.Validate(postId);

            var userId = CurrentLoggedUser.LoggedUser.UserId;
            var post = postRepository.GetById(postId);


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
