using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;
using ForumTemplate.Services.CommentService;

namespace ForumTemplate.Mappers
{
    public class PostMapper
    {
        private readonly ICommentService commentService;

        public PostMapper(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        public Post MapToPost(PostRequest postRequest)
        {
            return Post.CreatePost(
               postRequest.Title,
               postRequest.Content,
               postRequest.UserId
            );
        }
        public PostResponse MapToPostResponse(Post post)
        {
            var commentsResponses = this.commentService.GetComments(post.PostId);

            return new PostResponse
            (
                post.PostId,
                post.Title,
                post.Content,
                post.User.Username,
                commentsResponses,
                post.CreatedAt,
                post.UpdatedAt
            ); 
        }
        public List<PostResponse> MapToPostResponse(List<Post> posts)
        {
            var postResponses = new List<PostResponse>();

            foreach (var post in posts)
            {
                var commentsResponses = this.commentService.GetComments(post.PostId);

                var response = new PostResponse
                (
                    post.PostId,
                    post.Title,
                    post.Content,
                    post.User.Username,
                    commentsResponses,
                    post.CreatedAt,
                    post.UpdatedAt
                );

                postResponses.Add(response);
            }
           return postResponses;
        }
    }
}
