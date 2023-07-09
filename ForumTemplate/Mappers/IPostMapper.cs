using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;

namespace ForumTemplate.Mappers
{
    public interface IPostMapper
    {
        Post MapToPost(PostRequest postRequest);
        PostResponse MapToPostResponse(Post post);
        List<PostResponse> MapToPostResponse(List<Post> posts);
        PostRequest MapToPostRequest(PostViewModel postViewModel, Guid userId);
        PostViewModel MapToPostViewModel(Post post);
        PostViewModel MapToPostViewModel(PostResponse postResponse);

    }
}
