using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;
using ForumTemplate.Services;
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
            var commentsResponses = this.commentService.GetComments(post.Id);

            return new PostResponse
            (
                post.Id,
                post.Title,
                post.Content,
                post.UserId,
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
                var commentsResponses = this.commentService.GetComments(post.Id);

                var response = new PostResponse
                (
                    post.Id,
                    post.Title,
                    post.Content,
                    post.UserId,
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
