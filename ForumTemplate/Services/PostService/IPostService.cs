using ForumTemplate.DTOs.PostDTOs;
using ForumTemplate.Models;


namespace ForumTemplate.Services.PostService
{
    public interface IPostService
    {
        List<PostResponse> GetAll();
        PostResponse GetById(Guid id);
        PostResponse Create(PostRequest postRequest);
        PostResponse Update(Guid id, PostRequest postRequest);
        string Delete(Guid id);
    }
}
