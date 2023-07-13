using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;
using ForumTemplate.Models.ViewModels;

namespace ForumTemplate.Services.PostService
{
    public interface IPostService
    {
        List<PostResponse> GetAll();
        List<Post> GetAllPosts();
        List<PostResponse> FilterBy(PostQueryParameters filterParameters);
        List<Post> SearchBy(PostQueryParameters search);
        PostResponse GetById(Guid id);
        Post GetByPostId(Guid id);
        PostResponse Create(User loggedUser, PostRequest postRequest);
        PostResponse Update(User loggedUser,Guid id, PostRequest postRequest);
        string Delete(User loggedUser, Guid postId);
        void DeleteByUserId(Guid UserId);
        List<Post> GetTopCommentedPosts(int count);
        List<Post> GetRecentlyCreatedPosts(int count);
    }
}
