using ForumTemplate.DTOs.CommentDTOs;
using ForumTemplate.DTOs.TagDTOs;
using ForumTemplate.Models;

namespace ForumTemplate.Mappers
{
    public interface ITagMapper
    {
        TagResponse MapToTagResponse(Tag tag);
        List<TagResponse> MapToTagResponse(List<Tag> tags);
        Tag MapToTag(TagRequest tagRequest);
        TagRequest MapToTagRequest(TagResponse tagResponse);
    }
}
