using ForumTemplate.Models;
using ForumTemplate.Models.Input;
using ForumTemplate.Models.Result;

namespace ForumTemplate.Services
{
    public interface IPostService
    {
        List<PostResultModel> GetAll();

        PostResultModel GetById(int id);

        PostResultModel Create(PostInputModel post);

        PostResultModel Update(int id, PostInputModel post);

        string Delete(int id);
    }
}
