using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Mappers
{
    public interface IPostMapper
    {
        Post MapToPost(PostRequest postRequest);

        PostResponse MapToPostResponse(Post post);

        List<PostResponse> MapToPostResponse(List<Post> posts);


    }
}
