using ForumTemplate.Models;
using ForumTemplate.Repositories.DTO_s;

namespace ForumTemplate.Repositories
{
    public interface IPostRepository
    {
        List<PostDTO> GetAll();

        PostDTO GetById(int id);

        PostDTO GetByTitle(string title);

        PostDTO Create(PostDTO post);

        PostDTO Update(int id, PostDTO post);

        string Delete(int id);

        List<PostDTO> GetByUserId(int id);

        bool Exist(int id);
    }
}
