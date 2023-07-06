using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;


namespace ForumTemplate.Services.PostService
{
    public interface IPostService
    {
        List<PostResponse> GetAll();
        List<PostResponse> FilterBy(PostQueryParameters filterParameters);
        PostResponse GetById(Guid id);
        PostResponse Create(User loggedUser, PostRequest postRequest);
        PostResponse Update(User loggedUser,Guid id, PostRequest postRequest);
        string Delete(User loggedUser, Guid userId);
        void DeleteByUserId(Guid UserId);
    }
}
