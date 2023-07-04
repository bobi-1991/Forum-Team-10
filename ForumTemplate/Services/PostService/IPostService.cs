using ForumTemplate.Common.FilterModels;
using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;


namespace ForumTemplate.Services.PostService
{
    public interface IPostService
    {
        List<PostResponse> GetAll();
        List<PostResponse> FilterBy(PostQueryParameters filterParameters);
        PostResponse GetById(Guid id);
        PostResponse Create(PostRequest postRequest);
        PostResponse Update(Guid id, PostRequest postRequest);
        string Delete(Guid userId);
        void DeleteByUserId(Guid UserId);
    }
}
