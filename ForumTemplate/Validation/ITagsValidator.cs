using ForumTemplate.DTOs.TagDTOs;

namespace ForumTemplate.Validation
{
    public interface ITagsValidator
    {
        void Validate(TagRequest tagRequest);

        void Validate(Guid id);

        void GetTagsByPostID(Guid postId);
    }
}
