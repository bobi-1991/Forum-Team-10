using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Services.TagService
{
    public interface ITagService
    {
        List<TagResponse> GetTagsByPostId(Guid postId);

        TagResponse GetById(Guid id);

        //List<TagResponse> GetAll();

        TagResponse Create(User loggedUser, TagRequest tagRequest);

        string Delete(User loggedUser, Guid id);

        public void DeleteByPostId(Guid postId);

    }
}
