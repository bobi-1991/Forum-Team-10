using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;
using ForumTemplate.Services.CommentService;

namespace ForumTemplate.Mappers
{
    public class PostMapper : IPostMapper
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
                post.UserId,
                post.Likes.Count(),
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
                    post.UserId,
                    post.Likes.Count(),
                    commentsResponses,
                    post.CreatedAt,
                    post.UpdatedAt
                );

                postResponses.Add(response);
            }
            return postResponses;
        }

        public PostRequest MapToPostRequest(PostViewModel postViewModel, Guid userId)
        {
            return new PostRequest
            {
                Title = postViewModel.Title,
                Content = postViewModel.Content,
                UserId = userId
            };
        }
        public PostViewModel MapToPostViewModel(Post post)
        {
            return new PostViewModel
            {
                Title = post.Title,
                Content = post.Content,
                UserId = post.UserId
            };
        }
        public PostViewModel MapToPostViewModel(PostResponse postResponse)
        {
            return new PostViewModel
            {
                Title = postResponse.Title,
                Content = postResponse.Content,
                UserId = postResponse.UserId
            };
        }

    }
}
