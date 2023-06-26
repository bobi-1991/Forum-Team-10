using ForumTemplate.Models;
using ForumTemplate.Repositories.PostPersistence;

namespace ForumTemplate.Repositories.PostPersistence
{
    public interface IPostRepository
    {
        List<Post> GetAll();

        Post GetById(Guid id);

        Post GetByTitle(string title);

        Post Create(Post post);

        Post Update(Guid id, Post post);

        string Delete(Guid id);

        List<Post> GetByUserId(Guid id);

        bool Exist(Guid id);
    }
}
