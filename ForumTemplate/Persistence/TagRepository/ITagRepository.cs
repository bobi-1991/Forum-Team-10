using ForumTemplate.Models;

namespace ForumTemplate.Persistence.TagRepository
{
    public interface ITagRepository
    {
        Tag GetById(Guid id);

        Tag GetByContent(string content);

        List<Tag> GetByUserId(Guid id);

        List<Tag> GetByPostId(Guid postId);

        Tag Create(Tag tag);

        string Delete(Guid id);

        void DeleteByPostId(Guid postId);
    }
}
