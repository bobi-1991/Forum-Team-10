using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;


namespace ForumTemplate.Services.PostService
{
    public interface IPostService
    {
        List<PostResponse> GetAll();
        List<Post> GetAllPosts();
        List<PostResponse> FilterBy(PostQueryParameters filterParameters);
        PostResponse GetById(Guid id);
        Post GetByPostId(Guid id);
        PostResponse Create(User loggedUser, PostRequest postRequest);
        PostResponse Update(User loggedUser,Guid id, PostRequest postRequest);
        string Delete(User loggedUser, Guid userId);
        void DeleteByUserId(Guid UserId);
        List<Post> GetTopCommentedPosts(int count);
        List<Post> GetRecentlyCreatedPosts(int count);
    }
}
